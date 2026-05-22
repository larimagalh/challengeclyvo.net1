# 🐾 ClyvoVet API

API RESTful desenvolvida em ASP.NET Core 8 para gerenciamento de pets, responsáveis e consultas veterinárias.

---

# 🚀 Tecnologias

- ASP.NET Core 8
- C#
- Entity Framework Core
- Oracle Database
- Swagger/OpenAPI

---

# 📦 Funcionalidades

- CRUD de Pets
- CRUD de Responsáveis
- CRUD de Consultas
- Integração com Oracle
- Documentação Swagger

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

# 👩‍💻 Desenvolvido por

Larissa Juvenal de Magalhães RM566457
Matheus Gianolli RM565258
Júlia Kauane Menezes Farias RM565568
Gustavo Ribeiro Permagnani RM564995
Enzo Xavier Coelho RM563379
