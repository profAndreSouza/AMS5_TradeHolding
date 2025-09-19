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

Após login, API User publica evento.

**Exchange:** `user.events`
**Routing Key:** `user.auth.success`

**Mensagem:**

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

**Mensagem:**

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



## 3. **Atualização de cotação (API Currency → API Wallet + Chatbot)**

API Currency recebe atualização do mercado → publica evento.

**Exchange:** `currency.events`
**Routing Key:** `currency.price.update`

**Mensagem:**

```json
{
  "event": "CURRENCY_PRICE_UPDATE",
  "symbol": "BTC-USD",
  "newPrice": 27800.75,
  "timestamp": "2025-09-19T15:15:00Z"
}
```



## 4. **Interação do Chatbot (API Chatbot → outros serviços)**

Usuário pede via chat: “Depositar 200 USD”.

**Exchange:** `chatbot.commands`
**Routing Key:** `chatbot.wallet.deposit`

**Mensagem:**

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



# **Fluxos de Comunicação**

## **1. Fluxo de Trade de Ativos**

```mermaid
sequenceDiagram
    participant FE as Frontend (Next.js)
    participant GW as API Gateway
    participant U as API User
    participant W as API Wallet
    participant C as API Currency
    participant MQ as BrokerAPI
    participant CB as API Chatbot

    FE->>GW: POST /wallet/trade {from: BTC, to: USDT, amount: 1}
    GW->>U: Valida JWT
    U-->>GW: OK
    GW->>W: Solicita trade (userId, BTC → USDT, 1.0)
    W->>C: Consulta preço BTC-USD
    C-->>W: Retorna preço (27,500.40)
    W-->>GW: Retorna sucesso {tradeId, status: SUCCESS, newBalances}
    GW-->>FE: Exibe saldo atualizado (REST)

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
    participant MQ as BrokerAPI
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



# **Tabela Consolidada – Síncrono vs Assíncrono por API**

| **API**          | **Chamadas Síncronas (REST/HTTP)**                                                           | **Chamadas Assíncronas (Eventos BrokerAPI)**                                                                                            |
| ---------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| **API User**     | `POST /user/login` → login e autenticação                                                    | `user.auth.success` → evento disparado após autenticação bem-sucedida                                                                  |
| **API Wallet**   | `GET /wallet/balance` → consultar saldo<br>`POST /wallet/trade` → executar trade             | `wallet.deposit.success` → depósito concluído<br>`wallet.trade.success` → trade concluído                                              |
| **API Currency** | `GET /currency/price` → consulta preço em tempo real                                         | `currency.price.update` → atualização de cotação em tempo real                                                                         |
| **API Chatbot**  | `POST /chatbot/message` → envio de mensagem do usuário, retorno imediato de resposta textual | `chatbot.wallet.deposit` → comando para depósito<br>Eventos de notificação para confirmar ações (ex: depósito concluído, alerta preço) |


# **Resumo Final**

* **Síncrono (REST/HTTP)** → usado em **requisições do frontend**: login, saldo, trade, chatbot.
* **Assíncrono (BrokerAPI)** → usado em **eventos entre microserviços**: depósitos, trades, atualizações de preço.
* Fluxos mistos:

  * **Trade** → começa síncrono, termina com evento assíncrono.
  * **Depósito via Chatbot** → começa síncrono (mensagem), continua assíncrono (execução via BrokerAPI).
