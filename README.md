# Job Tracker System

End-to-end job application tracker built with:

- **Frontend:** Blazor Web App (`JobTracker.Web`, .NET 10, interactive server rendering)
- **Backend:** ASP.NET Core Web API (`JobTracker.Api`, .NET 10)
- **Shared contracts:** `JobTracker.Shared`
- **Data:** EF Core + SQLite (`jobtracker.db`, created automatically in development)

## Solution structure

```text
JobTracker.slnx
src/
  JobTracker.Api/             # Web API, EF Core, SQLite, CRUD endpoints
  JobTracker.Shared/          # DTOs and shared enums
  JobTracker.Web/
    JobTracker.Web/           # Blazor Web App host
    JobTracker.Web.Client/    # Blazor WebAssembly client project (template)
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Run locally

Start the API first, then the Blazor app (in separate terminals):

```bash
# Terminal 1 - API (http://localhost:5062)
dotnet run --project src/JobTracker.Api/JobTracker.Api.csproj
```

```bash
# Terminal 2 - Blazor Web App (http://localhost:5257)
dotnet run --project src/JobTracker.Web/JobTracker.Web/JobTracker.Web.csproj
```

Open the Blazor app at <http://localhost:5257> and navigate to **Jobs**.

## API endpoints

Base URL: `http://localhost:5062`

- `GET /api/job-applications?search=&status=` - list/search job applications
- `GET /api/job-applications/{id}` - get a job application
- `POST /api/job-applications` - create
- `PUT /api/job-applications/{id}` - update
- `DELETE /api/job-applications/{id}` - delete
- `GET /openapi/v1.json` - OpenAPI document (development)

## Configuration

- API base URL used by Blazor: `src/JobTracker.Web/JobTracker.Web/appsettings.json` (`Api:BaseUrl`)
- API SQLite connection/CORS origin: `src/JobTracker.Api/appsettings.json`
- The API seeds a couple of sample applications on first run when the database is empty.

## Build

```bash
dotnet build JobTracker.slnx
```
