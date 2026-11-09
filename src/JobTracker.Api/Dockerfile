# Build from the repository root:
#   docker build -f src/JobTracker.Api/Dockerfile -t jobtracker-api .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY JobTracker.slnx ./
COPY src/JobTracker.Shared/JobTracker.Shared.csproj src/JobTracker.Shared/
COPY src/JobTracker.Api/JobTracker.Api.csproj src/JobTracker.Api/
COPY src/JobTracker.Web/JobTracker.Web.Client/JobTracker.Web.Client.csproj src/JobTracker.Web/JobTracker.Web.Client/
COPY src/JobTracker.Web/JobTracker.Web/JobTracker.Web.csproj src/JobTracker.Web/JobTracker.Web/
RUN dotnet restore JobTracker.slnx

COPY . .
RUN dotnet publish src/JobTracker.Api/JobTracker.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JobTracker.Api.dll"]
