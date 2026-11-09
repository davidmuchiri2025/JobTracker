# Deploying JobTracker with GitHub Actions (Azure App Service)

This solution has two deployable apps:

- **API**: `src/JobTracker.Api/JobTracker.Api.csproj` (ASP.NET Core Web API, SQLite)
- **Web**: `src/JobTracker.Web/JobTracker.Web/JobTracker.Web.csproj` (Blazor Web App host)

The GitHub Actions workflows deploy each app to its own **Azure App Service** using publish profiles.

## Prerequisites

1. Two Azure App Services (recommended: Linux). Make sure each app is configured for the .NET runtime/stack you intend to use (this project targets **.NET 10**; verify .NET 10 availability in your Azure region/plan).
2. The GitHub repository (for example, `davidmuchiri2025/JobTracker`) with Actions enabled.

## 1. Configure workflow placeholders

Edit these files and replace the placeholder app names with your real Azure App Service names:

- `.github/workflows/deploy-api.yml` → `AZURE_WEBAPP_NAME`
- `.github/workflows/deploy-web.yml` → `AZURE_WEBAPP_NAME`

## 2. Add GitHub secrets (publish profiles)

In Azure Portal, for each App Service: **Deployment Center / Overview → Get publish profile** (or **Download publish profile**).

In GitHub: **Repository → Settings → Secrets and variables → Actions → New repository secret**:

| Secret | Value |
|---|---|
| `AZUREAPPSERVICE_PUBLISHPROFILE_API` | Full XML contents of the **API** App Service publish profile |
| `AZUREAPPSERVICE_PUBLISHPROFILE_WEB` | Full XML contents of the **Web** App Service publish profile |

## 3. Configure Azure App Service application settings

App settings override `appsettings.json` keys (use `__` for nesting on Linux).

### API App Service

| Name | Example value | Purpose |
|---|---|---|
| `Cors__WebOrigin` | `https://<your-web-app>.azurewebsites.net` | Allows the deployed Blazor app to call the API |
| `ConnectionStrings__Default` | `Data Source=/home/data/jobtracker.db` | Stores SQLite on the persistent `/home` path (Linux). The API creates the directory if needed. |

### Web App Service

| Name | Example value | Purpose |
|---|---|---|
| `Api__BaseUrl` | `https://<your-api-app>.azurewebsites.net` | Tells the Blazor app where the API is hosted |

## 4. Deploy

Push to `main`, or run the workflows manually from the **Actions** tab (`workflow_dispatch` is enabled).

## Notes

- **SQLite** is file-based. A persistent path (such as `/home/data/jobtracker.db` on App Service Linux) is important; local app content may not behave as you expect across deployments/scaling. For multi-instance or production-grade persistence, consider migrating to Azure SQL or another managed database.
- **.NET 10**: confirm your App Service plan supports the target runtime. If needed, adjust the workflows (runtime identifiers/self-contained publish) to match your hosting setup.
- The workflows build the full solution for simplicity, then publish/deploy the relevant project.
