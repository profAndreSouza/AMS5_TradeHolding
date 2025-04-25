
# AMS TRADE HOLDING

## 1. **Arquitetura da Aplicação**

- **Frontend:**
  - Utiliza **React** com **Next.js** e **TypeScript**, garantindo uma aplicação web dinâmica e escalável.
  - **Tailwind CSS** é adotado para uma UI responsiva, de fácil manutenção e com design adaptável.
  - **Principais Funcionalidades**: Exibição de dados dinâmicos (ex: saldo, ativos), interatividade com gráficos e execução de transações de trade.

- **Backend:**
  - Estruturado como **microserviços**, com um **API Gateway** que gerencia a comunicação entre o frontend e os diversos serviços.
  - **API User**: Gerencia o cadastro de usuários, autenticação via JWT e outros recursos relacionados.
  - **API Wallet**: Responsável pela gestão de carteiras de usuários, permitindo depósitos, saques e transferências.
  - **API Currency**: Fornece cotações de moedas e histórico de ativos.
  - **API Chatbot**: Gerencia interações automatizadas com o usuário através de chatbot.
  - **RabbitMQ**: Utilizado para comunicação assíncrona entre microserviços, garantindo alta disponibilidade e integração entre eles.

## 2. **Requisitos Funcionais**

### **Login**
- **Autenticação multifatorial**: Implementação de SMS para desktop e biometria para mobile.
- **Autenticação via JWT**: Garantia de sessões seguras e persistentes, com gerenciamento eficiente de tokens.

### **Home**
- Exibição do **saldo total** do usuário, com atualização em tempo real.
- **Botão de depósitos fictícios** para simulação de transações.
- Lista dinâmica de **ativos populares**, atualizada em tempo real via API.

### **Trade**
- Funcionalidade de **seleção de ativos** para troca, com validação de saldo antes de realizar qualquer transação.
- **Gráficos históricos** dos valores dos ativos, permitindo análise de tendências.
- Execução de **trocas de ativos** com base nos saldos disponíveis.

### **Wallets**
- Visão geral das carteiras, com categorias filtráveis (ex: overview, spot, funding).
- Funcionalidade de **depósitos** e **saques** fictícios para simulação de transações.
- **Transferências** entre carteiras do mesmo usuário, com saldo e ativos atualizados em tempo real.

## 3. **Requisitos Não Funcionais**

### **Desempenho e Tempo de Resposta**
- O sistema deve garantir que o tempo de resposta para exibição de dados e execução de transações não ultrapasse 1 segundo.

### **Escalabilidade**
- A aplicação deve ser escalável horizontalmente, podendo adicionar servidores conforme o aumento da demanda.

### **Segurança**
- Implementação de criptografia robusta para dados sensíveis.
- Proteção contra falhas e **acessos não autorizados** por meio de autenticação multifatorial e controle rigoroso de permissões.

### **Usabilidade**
- A interface será **responsiva**, adaptando-se adequadamente a dispositivos móveis, desktop e tablets, garantindo uma experiência de uso consistente.

### **Manutenibilidade**
- Código modular, bem documentado e estruturado para facilitar a expansão e manutenção a longo prazo.

### **Tolerância a Falhas**
- Implementação de uma estratégia de **failover**, assegurando a continuidade dos serviços mesmo em caso de falhas em componentes críticos.

### **Compatibilidade**
- O sistema será compatível com as versões mais recentes dos principais navegadores, como **Chrome**, **Firefox**, **Safari** e **Edge**.

## 4. **Padronização de Commits**

A padronização de commits proposta ajuda a manter o histórico de desenvolvimento organizado e facilita o entendimento de mudanças no código. Exemplos:

- **`feat(auth): adicionar autenticação com JWT`**: Implementação da autenticação JWT no sistema de login.
- **`fix(wallet): corrigir erro no cálculo do saldo após depósito`**: Correção de bug que afetava o cálculo de saldo após um depósito.
- **`docs: atualizar documentação sobre a integração com a API de moeda`**: Atualização da documentação para refletir mudanças na integração com a API de cotações.
- **`refactor(api): refatorar código da API de wallet para melhorar legibilidade`**: Refatoração para melhorar a estrutura e legibilidade do código da API de wallet.

## 5. **Associação de Requisitos Funcionais com as APIs de Backend**

### 1. **Login**
- **API User**
  - **Função**: Gerencia o cadastro, login e autenticação do usuário.
  - **Requisitos Funcionais**:
    - **Autenticação via JWT** para sessões seguras e persistentes.
    - **Autenticação multifatorial**:
      - **Desktop**: Autenticação via SMS.
      - **Mobile**: Autenticação biométrica (facial ou digital).

### 2. **Home**
- **API Wallet**
  - **Função**: Exibe o saldo total do usuário e possibilita depósitos fictícios.
  - **Requisitos Funcionais**:
    - Exibição do saldo total das carteiras do usuário.
    - **Botão de depósito** para adicionar valores fictícios à conta.
  
- **API Currency**
  - **Função**: Fornece dados sobre ativos populares, como cotações de moedas e histórico.
  - **Requisitos Funcionais**:
    - Exibição em tempo real de **ativos populares**, com dados atualizados de cotações e histórico.

### 3. **Trade**
- **API Wallet**
  - **Função**: Gerencia as trocas de ativos e verifica o saldo dos ativos disponíveis.
  - **Requisitos Funcionais**:
    - **Seleção de ativos**: Permite que o usuário escolha dois ativos para troca.
    - **Troca de ativos**: A troca é validada com base no saldo do usuário e realizada com a conversão automática de valores.
  
- **API Currency**
  - **Função**: Fornece os valores de mercado dos ativos para realizar o cálculo das trocas.
  - **Requisitos Funcionais**:
    - Exibição de **gráficos históricos** dos valores dos ativos nas últimas 24 horas para facilitar a análise de mercado.

### 4. **Wallets**
- **API Wallet**
  - **Função**: Gerencia as carteiras de usuários, incluindo depósitos, saques e transferências entre carteiras.
  - **Requisitos Funcionais**:
    - Exibição de **visão geral** das carteiras com filtros de categorias (overview, spot, funding).
    - **Depósitos e saques** fictícios, com atualização dinâmica do saldo.
    - **Transferências de valores** entre carteiras, com atualização em tempo real do saldo e ativos.

### 5. **Mensageria e Comunicação**
- **RabbitMQ**
  - **Função**: Facilita a comunicação assíncrona entre os microserviços.
  - **Requisitos Funcionais**:
    - Notificações em tempo real sobre alterações de saldo, trocas de ativos, depósitos/saques, etc.
    - **Integração entre as APIs** de Wallet, Currency e User para garantir que as ações em uma API sejam refletidas de forma eficiente nas outras.

### 6. **Chatbot**
- **API Chatbot**
  - **Função**: Gerencia as interações com o usuário via chatbot.
  - **Requisitos Funcionais**:
    - Oferece suporte para responder dúvidas sobre transações, valores de mercado e status da conta.
    - Serve como interface adicional para **consultas de saldo** e até mesmo para realizar **depósitos** e **transferências** simuladas por comandos de texto.


## Resumo das APIs e seus Requisitos Funcionais

| **Requisito Funcional**               | **API Backend**          | **Função Principal**                                                       |
|---------------------------------------|--------------------------|----------------------------------------------------------------------------|
| **Login (Autenticação)**              | **API User**             | Gerenciamento de login e autenticação via JWT, SMS (desktop), biometria (mobile) |
| **Home (Saldo total, Depósitos)**    | **API Wallet**           | Exibição de saldo total e simulação de depósitos fictícios                 |
| **Home (Ativos populares em tempo real)** | **API Currency**         | Exibição de ativos populares com cotações em tempo real                    |
| **Trade (Troca de ativos)**          | **API Wallet, API Currency** | Validação e execução de trocas entre ativos com dados de mercado            |
| **Wallets (Visão geral da carteira)** | **API Wallet**           | Gerenciamento de carteiras, depósitos, saques, transferências e ativos     |
| **Mensageria**                        | **RabbitMQ**             | Comunicação assíncrona entre microserviços para atualização em tempo real   |
| **Chatbot**                           | **API Chatbot**          | Interação com o usuário via chatbot para consultas e transações           |
