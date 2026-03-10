# ReactFullStackDemo

Full-stack demo (React + ASP.NET Core + MongoDB). This phase contains the API and the standalone frontend.

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

## Frontend (Local Dev)

```bash
cd src/ReactFullStackDemo.Web
npm install
npm test
npm run dev
```

Vite dev server: `http://localhost:5173`

## Frontend (Docker)

```bash
docker compose -f infra/docker-compose.web.yml up -d --build
```

Web: `http://localhost:8080`

Note: `/api` is proxied to the `api` container. For a full stack run, use the compose file below.

## Full Stack (Docker)

```bash
docker compose -f infra/docker-compose.full.yml up -d --build
```

Web + API via Nginx: `http://localhost:8080`
Health: `http://localhost:8080/api/health`
