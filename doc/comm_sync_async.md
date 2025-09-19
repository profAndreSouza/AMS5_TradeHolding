# **Comunicações Síncronas (REST/HTTP via API Gateway)**

Essas chamadas são **request/response imediatas**, tipicamente do **Frontend → API Gateway → Microserviço**.


## 1. **Login (API User)**

Usuário tenta logar → precisa de resposta imediata.

**Request (POST /user/login):**

```json
{
  "email": "andre@example.com",
  "password": "123456"
}
```

**Response:**

```json
{
  "userId": "u123",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI...",
  "mfaRequired": true,
  "mfaType": "sms"
}
```


## 2. **Consultar saldo total (API Wallet)**

Frontend carrega a **Home** e exibe o saldo.

**Request (GET /wallet/balance?userId=u123):**

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...
```

**Response:**

```json
{
  "userId": "u123",
  "totalBalance": 15200.50,
  "currency": "USD",
  "wallets": [
    { "type": "spot", "balance": 8500.00 },
    { "type": "funding", "balance": 4700.50 },
    { "type": "overview", "balance": 2000.00 }
  ]
}
```


## 3. **Consulta de preço em tempo real (API Currency)**

Usada na tela de **Trade**.

**Request (GET /currency/price?symbol=BTC-USD):**

```http
Authorization: Bearer <token>
```

**Response:**

```json
{
  "symbol": "BTC-USD",
  "price": 27500.40,
  "timestamp": "2025-09-19T15:05:00Z"
}
```


## 4. **Troca de ativos (API Wallet + API Currency)**

Usuário faz trade de **1 BTC → USDT**.

**Request (POST /wallet/trade):**

```json
{
  "userId": "u123",
  "fromAsset": "BTC",
  "toAsset": "USDT",
  "amount": 1.0
}
```

**Response:**

```json
{
  "tradeId": "t98765",
  "fromAsset": "BTC",
  "toAsset": "USDT",
  "executedPrice": 27500.40,
  "status": "SUCCESS",
  "newBalances": {
    "BTC": 0.0,
    "USDT": 27500.40
  }
}
```


## 5. **Interação com o Chatbot (API Chatbot)**

Usuário pergunta: “Qual meu saldo?”

**Request (POST /chatbot/message):**

```json
{
  "userId": "u123",
  "message": "Qual meu saldo?"
}
```

**Response:**

```json
{
  "reply": "Seu saldo total é de $15,200.50, dividido entre Spot, Funding e Overview.",
  "context": {
    "totalBalance": 15200.50,
    "currency": "USD"
  }
}
```


# **Comunicações Assíncronas (Eventos via BrokerAPI)**

Aqui temos **eventos disparados** entre microserviços. Não há resposta imediata, mas **notificações** e **atualizações em tempo real**.


## 1. **Usuário autenticado (API User → outros serviços)**

Após login, API User publica evento para outros microserviços.

**Exchange:** `user.events`
**Routing Key:** `user.auth.success`

**Mensagem (JSON):**

```json
{
  "event": "USER_AUTH_SUCCESS",
  "userId": "u123",
  "timestamp": "2025-09-19T15:10:00Z",
  "details": {
    "method": "sms",
    "ip": "189.55.23.11"
  }
}
```


## 2. **Depósito realizado (API Wallet → API User + API Chatbot)**

Usuário faz depósito fictício → Wallet dispara evento.

**Exchange:** `wallet.events`
**Routing Key:** `wallet.deposit.success`

**Mensagem (JSON):**

```json
{
  "event": "WALLET_DEPOSIT_SUCCESS",
  "userId": "u123",
  "walletType": "spot",
  "amount": 500.00,
  "currency": "USD",
  "newBalance": 9000.00,
  "timestamp": "2025-09-19T15:12:00Z"
}
```

→ Esse evento pode ser consumido por:

* **API User** (para atualizar histórico do usuário).
* **API Chatbot** (para notificar: “Depósito de \$500 concluído com sucesso”).


## 3. **Atualização de cotação (API Currency → API Wallet + Chatbot)**

API Currency recebe atualização do mercado → publica evento.

**Exchange:** `currency.events`
**Routing Key:** `currency.price.update`

**Mensagem (JSON):**

```json
{
  "event": "CURRENCY_PRICE_UPDATE",
  "symbol": "BTC-USD",
  "newPrice": 27800.75,
  "timestamp": "2025-09-19T15:15:00Z"
}
```

→ Esse evento pode ser consumido por:

* **API Wallet** (para recalcular trades pendentes).
* **API Chatbot** (para alertar usuário: “O preço do BTC subiu para \$27,800”).


## 4. **Interação do Chatbot (API Chatbot → outros serviços)**

Usuário pede via chat: “Depositar 200 USD”.

**Exchange:** `chatbot.commands`
**Routing Key:** `chatbot.wallet.deposit`

**Mensagem (JSON):**

```json
{
  "command": "DEPOSIT",
  "userId": "u123",
  "amount": 200.00,
  "currency": "USD",
  "origin": "chatbot",
  "timestamp": "2025-09-19T15:20:00Z"
}
```

→ Esse evento é consumido pela **API Wallet**, que executa o depósito e depois emite o evento de **deposit.success**.

# **Fluxos de Comunicação**

## **1. Fluxo de Trade de Ativos**

```mermaid
sequenceDiagram
    participant FE as Frontend (Next.js)
    participant GW as API Gateway
    participant U as API User
    participant W as API Wallet
    participant C as API Currency
    participant MQ as RabbitMQ
    participant CB as API Chatbot

    FE->>GW: POST /wallet/trade {from: BTC, to: USDT, amount: 1}
    GW->>U: Valida JWT
    U-->>GW: OK
    GW->>W: Solicita trade (userId, BTC → USDT, 1.0)
    W->>C: Consulta preço BTC-USD
    C-->>W: Retorna preço (27,500.40)
    W-->>GW: Retorna sucesso {tradeId, status: SUCCESS, newBalances}
    GW-->>FE: Exibe saldo atualizado (REST síncrono)

    Note over W, MQ: Publica evento de trade concluído
    W->>MQ: {event: TRADE_SUCCESS, userId, assets, newBalances}
    MQ-->>CB: Notificação: "Trade realizado com sucesso"
    MQ-->>U: Atualiza histórico do usuário
```

## **2. Fluxo de Depósito via Chatbot**

```mermaid
sequenceDiagram
    participant FE as Frontend (Chat UI)
    participant GW as API Gateway
    participant CB as API Chatbot
    participant MQ as RabbitMQ
    participant W as API Wallet
    participant U as API User

    FE->>GW: POST /chatbot/message {"Depositar 200 USD"}
    GW->>CB: Mensagem do usuário
    CB-->>GW: Resposta inicial "Ok, processando depósito..."
    GW-->>FE: Retorna resposta imediata (REST)

    Note over CB, MQ: Publica comando de depósito
    CB->>MQ: {command: DEPOSIT, userId, amount: 200, currency: USD}
    MQ-->>W: Solicitação de depósito recebida
    W-->>W: Atualiza saldo da carteira
    W->>MQ: {event: WALLET_DEPOSIT_SUCCESS, userId, newBalance}
    MQ-->>CB: Confirmação para chatbot
    MQ-->>U: Atualiza histórico do usuário
    CB-->>FE: "Depósito de $200 concluído com sucesso!"
```


# **Resumo**

* **Síncrono (REST/HTTP)** → usado em **requisições diretas do frontend**, como login, consultar saldo, executar trade, ou falar com o chatbot.
* **Assíncrono (RabbitMQ)** → usado em **eventos internos entre microserviços**, garantindo consistência em tempo real e desacoplamento.
* O **fluxo de trade** é majoritariamente **síncrono** (precisa de resposta imediata), mas termina com um evento **assíncrono** notificando saldo atualizado.
* O **fluxo de depósito via chatbot** começa síncrono (mensagem do usuário), mas a execução real do depósito ocorre de forma **assíncrona via RabbitMQ**.
