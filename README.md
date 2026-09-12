# 🐾 ClyvoVet API

API RESTful desenvolvida em ASP.NET Core 8 para gerenciamento de pets, responsáveis e consultas veterinárias.

---

# 🚀 Tecnologias

- ASP.NET Core 10
- C#
- Entity Framework Core
- Oracle Database
- Swagger/OpenAPI
- Serilog (logging estruturado)
- OpenTelemetry (tracing e métricas)
- xUnit, FluentAssertions e Moq (testes unitários)

---

# 📦 Funcionalidades

- CRUD de Pets
- CRUD de Responsáveis
- CRUD de Consultas
- Integração com Oracle
- Documentação Swagger
- Logging estruturado com Serilog e correlação de requisições
- Health Check do banco de dados
- Rastreamento distribuído e métricas com OpenTelemetry
- Cobertura de testes unitários (xUnit + FluentAssertions)

---

# 🔗 Endpoints

## Pets

- GET /api/Pets
- GET /api/Pets/{id}
- GET /api/Pets/especie/{especie}
- POST /api/Pets
- PUT /api/Pets/{id}
- DELETE /api/Pets/{id}

## Responsáveis

- GET /api/Responsaveis
- GET /api/Responsaveis/{id}
- POST /api/Responsaveis
- PUT /api/Responsaveis/{id}
- DELETE /api/Responsaveis/{id}

## Consultas

- GET /api/Consultas
- GET /api/Consultas/{id}
- GET /api/Consultas/status/{status}
- POST /api/Consultas
- PUT /api/Consultas/{id}
- DELETE /api/Consultas/{id}

## Observabilidade

- GET /health

---

# ⚙️ Como executar

```bash
dotnet restore
dotnet ef database update
dotnet run
```

---

# 📘 Swagger

Acesse:

```bash
http://localhost:5043/swagger
```
<img width="1919" height="1021" alt="image" src="https://github.com/user-attachments/assets/939e370f-9729-4963-b8ca-8b76f26c8a98" />

---

# 📝 Logging estruturado (Serilog + ILogger)

A API utiliza o **Serilog** como provedor de logging, integrado ao `ILogger<T>` nativo do ASP.NET Core. Os logs são enviados simultaneamente para:

- **Console**, com um template customizado que exibe o `CorrelationId` de cada requisição.
- **Arquivo**, em `logs/app-{data}.log`, com rotação diária (`RollingInterval.Day`).

Cada Controller (`PetsController`, `ResponsaveisController`, `ConsultasController`) recebe um `ILogger<T>` via injeção de dependência e registra eventos relevantes do fluxo, como criação, busca e ausência de registros (ex: `_logger.LogInformation("Procurando por Pet: {petID}", id)`).

Além disso, o `UseSerilogRequestLogging()` é habilitado no `Program.cs`, gerando automaticamente um log estruturado para cada requisição HTTP recebida pela API (método, rota, status code e tempo de resposta).

---

# 🧩 Middlewares

## CorrelationIdMiddleware

Middleware responsável por garantir a rastreabilidade das requisições ponta a ponta. Seu funcionamento:

1. Verifica se a requisição já possui o header `X-Correlation-ID`.
2. Caso não exista, gera um novo `Guid` para identificar a requisição.
3. Adiciona o valor ao header de resposta (`X-Correlation-ID`), para que o cliente também possa rastreá-lo.
4. Insere o `CorrelationId` no contexto de log do Serilog (`LogContext.PushProperty`), fazendo com que **todos os logs gerados durante aquela requisição** — inclusive dentro dos Controllers — carreguem o mesmo identificador.

Isso facilita a correlação de logs distribuídos entre diferentes camadas e chamadas, especialmente em cenários de depuração e observabilidade.

O middleware é registrado no pipeline em `Program.cs` com `app.UseMiddleware<CorrelationIdMiddleware>();`.

---

# 📊 Observabilidade (Health Check, Tracing e Métricas)

- **Health Check**: exposto em `GET /health`, valida a conectividade com o banco de dados através do `BancoDadosHealthCheck`.
- **Tracing (OpenTelemetry)**: cada endpoint cria uma `Activity` própria (ex: `ListarPetsEndpoint`, `BuscarPetPorIdEndpoint`), permitindo rastrear o fluxo da requisição e marcar erros (`ActivityStatusCode.Error`) quando um recurso não é encontrado.
- **Métricas (OpenTelemetry Metrics)**: contadores customizados (`Pets_criadas_total`, `Consultas_criadas_total`, `responsaveis_criados_total`) registram a quantidade de recursos criados com sucesso na API.
- Tanto os traces quanto as métricas são exportados via `ConsoleExporter`, facilitando a inspeção local durante o desenvolvimento.

---

# ✅ Testes Unitários

O projeto conta com uma suíte de testes unitários localizada em `ClyvoVet.API/tests/ClyvoVet.API.Tests.Unit`, construída com:

- **xUnit** — framework de execução dos testes (`[Fact]` e `[Theory]`).
- **FluentAssertions** — asserções mais legíveis e expressivas (`Should().Be(...)`, `Should().Throw<ArgumentException>()`).
- **Moq** — disponível para criação de mocks em testes de camadas com dependências (ex: `AppDbContext`, `ILogger`).

## Cobertura atual

- `PetTests` — valida a criação de um `Pet` com dados válidos e o disparo de exceção para nomes inválidos (vazio, espaço em branco ou nulo).
- `ResponsavelTests` — valida a criação de um `Responsavel` com dados válidos e o disparo de exceção para nomes inválidos.
- `ConsultaTests` — valida a criação de uma `Consulta` garantindo que todos os campos (pet, responsável, sintomas, status e data) sejam corretamente atribuídos.

## Como executar os testes

```bash
cd ClyvoVet.API/tests/ClyvoVet.API.Tests.Unit
dotnet test
```

Também é possível rodar a partir da raiz da solução, apontando diretamente para o projeto de testes:

```bash
dotnet test ClyvoVet.API/tests/ClyvoVet.API.Tests.Unit/ClyvoVet.API.Tests.Unit.csproj
```

---

# 👩‍💻 Desenvolvido por

Larissa Juvenal de Magalhães RM566457
Matheus Gianolli RM565258
Júlia Kauane Menezes Farias RM565568
Gustavo Ribeiro Permagnani RM564995
Enzo Xavier Coelho RM563379