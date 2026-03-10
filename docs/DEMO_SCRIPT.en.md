# Demo Script (EN)

**Goal**  
Show a full-stack knowledge publishing platform with scalable API, MongoDB, Docker, Azure Container Apps, and CI/CD.

**Flow (10–15 minutes)**

1. **Overview**
- React + ASP.NET Core + MongoDB
- Angular alternative frontend
- Local Docker and Azure Container Apps
- CI/CD with GitHub Actions

2. **Architecture Snapshot**
- Frontend: React + Nginx and Angular + Nginx (reverse proxy `/api`)
- Backend: ASP.NET Core Web API
- Data: MongoDB
- Cloud: Azure Container Apps + ACR + Terraform (two Container Apps)

3. **Local Run (optional)**
```bash
docker compose -f infra/docker-compose.full.yml up -d --build
```
- React: `http://localhost:8080`

4. **Local Run (Angular, optional)**
```bash
docker compose -f infra/docker-compose.angular.full.yml up -d --build
```
- Angular: `http://localhost:8081`

5. **Live Site**
- React App: `https://react-fullstack-demo-app.orangepond-3f12b8c2.canadacentral.azurecontainerapps.io`
- Angular App: `https://react-fullstack-demo-angular.orangepond-3f12b8c2.canadacentral.azurecontainerapps.io`
- Health: `/api/health`

6. **Core Feature Demo**
- Create a Draft (title, markdown, tags)
- Save and verify list refresh
- Edit and change to Published
- Observe status and updated timestamp

7. **API Demo (optional)**
```bash
curl https://react-fullstack-demo-app.orangepond-3f12b8c2.canadacentral.azurecontainerapps.io/api/posts
```

8. **CI/CD**
- Workflows: `ci`, `deploy-azure-dispatch`
- Build and push web + api + angular images
- Terraform applies infrastructure and deployment

9. **IaC**
- `infra/terraform`: ACA + ACR + Log Analytics (two Container Apps)
- `infra/bootstrap`: TF state storage
