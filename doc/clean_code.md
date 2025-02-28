
## **1. API (Interface de Entrada)**
Esta camada é responsável por expor os endpoints da Web API e interagir com os casos de uso da aplicação.

- **`/Controllers`**: Contém os controladores que lidam com as requisições HTTP.
  - `UserController.cs`: Controlador responsável pelos endpoints relacionados a usuários. Ele chama os casos de uso ou serviços para processar as requisições.

- **`/DTOs`**: Contém os Data Transfer Objects (DTOs), usados para transferência de dados entre camadas.
  - `UserDTO.cs`: Representa a estrutura dos dados que serão expostos ou recebidos pela API.

- **`/Configurations`**: Contém configurações relacionadas à API.
  - `DependencyInjection.cs`: Classe onde são configuradas as injeções de dependência da aplicação.

- **`Program.cs`**: Arquivo principal que configura e inicia a aplicação ASP.NET Core.

---

## **2. Domain (Camada de Domínio)**
Aqui ficam os conceitos centrais do sistema, seguindo o **princípio da separação de interesses**.

- **`/Entities`**: Contém as entidades de domínio que representam os objetos do negócio.
  - `User.cs`: Classe que define as propriedades do usuário, como `Id`, `Nome`, `Email`, etc.

- **`/Interfaces`**: Define contratos para os repositórios.
  - `IUserRepository.cs`: Interface que define os métodos necessários para manipulação de dados do usuário (CRUD).

---

## **3. Infrastructure (Infraestrutura)**
Esta camada contém implementações de acesso a dados, configurações e migrations.

- **`/Data`**: Contém o contexto do Entity Framework Core.
  - `UserDbContext.cs`: Classe que representa o banco de dados e define os DbSets.

- **`/Repositories`**: Implementação concreta dos repositórios.
  - `UserRepository.cs`: Implementa `IUserRepository`, responsável por interagir com o banco de dados.

- **`/Migrations`**: Contém os arquivos de migração gerados pelo Entity Framework Core.
  - `MigrationsFiles.cs`: Representa os arquivos de versão do banco de dados.

- **`/Configurations`**: Contém configurações relacionadas ao armazenamento de arquivos ou outras configurações específicas.
  - `UserFiles.cs`: Pode ser responsável por manipulação de arquivos do usuário, como uploads de avatar.

---

## **4. Application (Regras de Negócio e Casos de Uso)**
Esta camada contém a lógica de aplicação e os serviços.

- **`/Services`**: Implementa as regras de negócio.
  - `UserService.cs`: Classe que gerencia a lógica de usuários, chamando os repositórios e validando regras.

- **`/Interfaces`**: Define contratos para os serviços da aplicação.
  - `IUserService.cs`: Interface que define os métodos da lógica de negócio do usuário.

- **`/UseCases`**: Implementa os casos de uso específicos do sistema.
  - `RegisterUserUseCase.cs`: Lida com a lógica para registrar um usuário.
  - `GetUserDetailsUseCase.cs`: Responsável por obter os detalhes de um usuário.

