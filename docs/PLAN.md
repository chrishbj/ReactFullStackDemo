# ReactFullStackDemo Plan

## 1. Goals and Scope
- Use the ShareGateDemo reference to preserve cloud, infrastructure, and CI/CD patterns.
- Build a full-stack app with React + ASP.NET Core + MongoDB.
- Provide an Angular frontend as an alternative implementation.
- Phase 1: Local Docker app (web + api + mongo).
- Phase 2: Evolve to Azure Container Apps with Terraform.
- Emphasize clean code, scalable API design, and demo readability.

## 2. Proposed Tech Stack
- Frontend: React + Vite + TypeScript + Nginx (static hosting + reverse proxy).
- Frontend (alt): Angular + TypeScript + Nginx (static hosting + reverse proxy).
- Backend: ASP.NET Core 10 Web API (controller-based) + MongoDB driver.
- Database: MongoDB 7.
- Infrastructure: Terraform + Azure Container Apps + ACR + Log Analytics.
- CI/CD: GitHub Actions (CI build + Azure deploy).

## 3. Architecture (Local and Cloud Parity)
- Local Docker Compose:
  - `web`: Nginx container serving React and proxying `/api` to `api`.
  - `api`: ASP.NET Core API connecting to `mongo`.
  - `mongo`: MongoDB container with a named volume.
- Local Docker Compose (Angular):
  - `angular-web`: Nginx container serving Angular and proxying `/api` to `api`.
  - `api`: ASP.NET Core API connecting to `mongo`.
  - `mongo`: MongoDB container with a named volume.
- Local Docker Compose (Angular reuse backend):
  - `angular-web` proxies `/api` to `host.docker.internal:5069`.
  - Uses the React stack API container without creating new `api`/`mongo`.
- Azure Container Apps:
  - One Container App with two containers: `web` and `api`.
  - `mongo` as a sidecar for demo use (EmptyDir, non-persistent).
  - A separate Container App for `angular-web` that proxies to the React app ingress.

## 4. Repository Structure (Target)
- `src/ReactFullStackDemo.Api`
- `src/ReactFullStackDemo.Web`
- `src/AngularFullStackDemo.Web`
- `infra/` (Docker Compose + Terraform)
- `.github/workflows/`
- `README.md`
- `DEMO_SCRIPT.md`

## 5. Local Run Experience (Target)
1. `docker compose -f infra/docker-compose.full.yml up -d`
2. Open React `http://localhost:8080`
3. Health check: `http://localhost:8080/api/health`
4. (Angular) `docker compose -f infra/docker-compose.angular.full.yml up -d`
5. Open Angular `http://localhost:8081`
6. Health check: `http://localhost:8081/api/health`

## 6. Azure Evolution (Target)
1. `terraform init / apply` to create RG, ACA, ACR, Log Analytics.
2. CI builds and pushes `web` + `api` images.
3. `terraform apply -var=web_image_tag=... -var=api_image_tag=...` to deploy.
4. Access frontend via ACA ingress.

## 7. Testing Strategy (Target)
- API unit tests: service layer + validation + mapping.
- API integration tests: minimal happy-path for core endpoints.
- Frontend unit tests: React component logic + API client behavior.
- Frontend e2e smoke test: basic flow against local compose (optional).

## 8. CI/CD Plan (Target)
- `ci.yml`: Build .NET + Node, run unit tests and fast integration tests.
- `deploy-azure-dispatch.yml`: Build and push web + api + angular images, then Terraform apply.
- Deploy runs automatically after `ci` succeeds on `main`, plus manual dispatch.
- Support manual dispatch for demo updates.

## 9. Milestones (Suggested Order)
1. Create solution skeleton and baseline structure.
2. Implement API with Mongo CRUD + health.
3. Implement React UI (list/create/edit/delete) with API integration.
4. Dockerize web and api + compose.
5. Add unit tests and a minimal integration test suite.
6. Update Terraform for multi-container ACA + ACR.
7. Update CI/CD and validate.
