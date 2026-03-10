# ReactFullStackDemo

Full-stack demo (React + ASP.NET Core + MongoDB). This phase contains the API only.

## Local API (Docker)

```bash
docker compose -f infra/docker-compose.api.yml up -d --build
```

API: `http://localhost:5069`
Health: `http://localhost:5069/api/health`

## Local API (CLI)

```bash
dotnet run --project src/ReactFullStackDemo.Api
```

## Tests

```bash
dotnet test ReactFullStackDemo.slnx -c Release
```
