# Task Manager API

A backend REST API built with ASP.NET Core, Entity Framework Core, SQL Server, and Swagger to manage tasks using a clean and scalable project structure. This project demonstrates backend development fundamentals and professional API design practices, including DTO usage, validation, service layer abstraction, filtering, search, and pagination.

---

## Overview

Task Manager API is a backend application designed to manage tasks through a RESTful API. It was built as a portfolio project to showcase practical software engineering skills, including API development, database integration, layered architecture, DTO-based design, input validation, and containerization readiness.

This project is intended to reflect real-world backend practices instead of only basic CRUD operations.

---

## Features

- Task CRUD operations
- DTO-based request and response models
- Input validation with Data Annotations
- Service layer abstraction
- SQL Server integration with Entity Framework Core
- Swagger/OpenAPI documentation
- Filtering by completion status
- Search by title or description
- Pagination support
- Clean project organization for maintainability and scalability

---

## Tech Stack

- **Language:** C#
- **Framework:** ASP.NET Core 8
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **API Documentation:** Swagger / Swashbuckle
- **Containerization:** Docker, Docker Compose

---

## Architecture and Design

This project follows a clean and organized structure with separation of concerns:

- **Controllers** handle HTTP requests and responses
- **Services** contain business logic
- **DTOs** define request and response contracts
- **Models** represent domain entities
- **Data** contains the database context and EF Core configuration
- **Interfaces** define service contracts for abstraction and maintainability

This approach improves readability, testability, and scalability.

---

## Project Structure

```text
TaskManagerApi/
├── Controllers/
│   └── TasksController.cs
├── Data/
│   ├── AppDbContext.cs
│   └── AppDbContextFactory.cs
├── DTOs/
│   ├── CreateTaskDto.cs
│   ├── UpdateTaskDto.cs
│   ├── TaskResponseDto.cs
│   ├── TaskQueryParametersDto.cs
│   └── PagedResultDto.cs
├── Interfaces/
│   └── ITaskService.cs
├── Migrations/
├── Models/
│   └── TaskItem.cs
├── Services/
│   └── TaskService.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── TaskManagerApi.csproj
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
└── README.md
```

---

## API Endpoints

### Tasks

- `GET /api/Task`
- `GET /api/Task/{id}`
- `POST /api/Task`
- `PUT /api/Task/{id}`
- `DELETE /api/Task/{id}`

> Note: The current route is `/api/Task` because it follows the current controller naming. If the controller is renamed to `TasksController`, the route can be updated to `/api/tasks` for a more RESTful style.

---

## Query Parameters

### `GET /api/Task`

Supports the following optional query parameters:

- `isCompleted=true|false`
- `search=keyword`
- `page=1`
- `pageSize=10`

### Example Requests

```http
GET /api/Task?isCompleted=false&search=api&page=1&pageSize=5
```

```http
GET /api/Task?isCompleted=true
```

```http
GET /api/Task?page=1&pageSize=10
```

---

## Sample Request Bodies

### Create Task

```json
{
  "title": "Learn ASP.NET Core",
  "description": "Build a clean Web API project"
}
```

### Update Task

```json
{
  "title": "Learn ASP.NET Core and Docker",
  "description": "Improve the API structure and containerize the application",
  "isCompleted": true
}
```

---

## Validation Rules

### CreateTaskDto

- `Title` is required
- `Title` maximum length: 100 characters
- `Description` maximum length: 500 characters

### UpdateTaskDto

- `Title` is required
- `Title` maximum length: 100 characters
- `Description` maximum length: 500 characters

These validations are handled automatically by ASP.NET Core due to the use of `[ApiController]` and Data Annotation attributes.

---

## Response Examples

### Successful GET Response

```json
[
  {
    "id": 1,
    "title": "Learn ASP.NET Core",
    "description": "Build a clean Web API project",
    "isCompleted": false,
    "createdAt": "2026-03-16T22:10:00Z"
  }
]
```

### Successful Paginated GET Response

```json
{
  "items": [
    {
      "id": 1,
      "title": "Learn ASP.NET Core",
      "description": "Build a clean Web API project",
      "isCompleted": false,
      "createdAt": "2026-03-16T22:10:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

### Validation Error Example

```json
{
  "errors": {
    "Title": [
      "The Title field is required."
    ]
  },
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400
}
```

---

## Running the Project Locally

### 1. Clone the repository

```bash
git clone https://github.com/alessandro-ruiz/task-manager-api.git
cd task-manager-api
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure the database connection

Update the connection string inside `appsettings.json`.

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

If you are using SQL Server LocalDB, you can use:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 4. Apply migrations

```bash
dotnet ef database update
```

### 5. Run the project

```bash
dotnet run
```

### 6. Open Swagger

```text
http://localhost:5106/swagger
```

---

## Running with Docker

This project includes Docker support to run both the API and SQL Server in containers.

### Start containers

```bash
docker compose up --build
```

### Default exposed ports

- API: `http://localhost:5106`
- Swagger: `http://localhost:5106/swagger`
- SQL Server: `localhost:1433`

### Database connection inside Docker

When running in Docker, the API uses the SQL Server container name as the server host:

```text
Server=sqlserver;Database=${DB_NAME};User Id=${DB_USER};Password=${DB_PASSWORD};TrustServerCertificate=True;
```

This value is configured in `docker-compose.yml` using environment variables.

---

## Docker Files Included

### Dockerfile

The Dockerfile builds and publishes the ASP.NET Core application and runs it in an ASP.NET runtime container.

### docker-compose.yml

The compose file runs:

- the API container
- the SQL Server container
- the required environment configuration and port mappings

### .dockerignore

The `.dockerignore` file excludes unnecessary local files and folders such as:

- `bin/`
- `obj/`
- `.vs/`
- `.git/`

---

## Development Notes

### Why DTOs are used

DTOs help separate internal entities from external API contracts. This avoids exposing domain models directly and makes the API easier to maintain and evolve.

### Why a service layer is used

The service layer moves business logic out of controllers, resulting in cleaner controllers and a more scalable architecture.

### Why filtering, search, and pagination matter

These features make the API more realistic and closer to production-style systems, where clients often need efficient querying and result control.

---

## Example Test Flow in Swagger

A good manual test flow is:

1. `POST /api/Task` → create a task
2. `GET /api/Task` → verify it appears in the list
3. `GET /api/Task/{id}` → verify the specific item
4. `PUT /api/Task/{id}` → update the task
5. `GET /api/Task/{id}` → confirm the update
6. `DELETE /api/Task/{id}` → remove the task
7. `GET /api/Task/{id}` → confirm it returns `404 Not Found`

---

## Future Improvements

This project can be extended with:

- Global exception handling middleware
- Structured logging
- Unit tests
- Integration tests
- Repository pattern
- JWT authentication and authorization
- Soft delete support
- Role-based access control
- API versioning
- CI/CD pipeline integration

---

## Why This Project Matters

This project showcases backend development fundamentals and professional engineering practices such as:

- REST API design
- layered project structure
- DTO usage
- validation
- service layer separation
- Entity Framework Core integration
- SQL Server database usage
- pagination, filtering, and search
- Swagger documentation
- Docker-based environment setup

It is designed to be a strong portfolio project for backend, full-stack, and software engineering opportunities.

---

## Author

**Alessandro Adrian Ruiz Nina**  
Software Engineer | Full-Stack Developer

- GitHub: https://github.com/alessandro-ruiz
- LinkedIn: add-your-linkedin-url-here
- Email: a.adrianruiznina@gmail.com

---

## License

This project is open for learning and portfolio purposes.
