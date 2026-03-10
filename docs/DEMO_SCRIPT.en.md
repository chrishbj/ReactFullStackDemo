# Demo Script (EN)

**Goal**  
Show a full-stack knowledge publishing platform with scalable API, MongoDB, Docker, Azure Container Apps, and CI/CD.

**Flow (10–15 minutes)**

1. **Overview**
- React + ASP.NET Core + MongoDB
- Local Docker and Azure Container Apps
- CI/CD with GitHub Actions

2. **Architecture Snapshot**
- Frontend: React + Nginx (reverse proxy `/api`)
- Backend: ASP.NET Core Web API
- Data: MongoDB
- Cloud: Azure Container Apps + ACR + Terraform

3. **Local Run (optional)**
```bash
docker compose -f infra/docker-compose.full.yml up -d --build
```
- Open: `http://localhost:8080`

4. **Live Site**
- URL: `https://react-fullstack-demo-app.orangepond-3f12b8c2.canadacentral.azurecontainerapps.io`
- Health: `/api/health`

5. **Core Feature Demo**
- Create a Draft (title, markdown, tags)
- Save and verify list refresh
- Edit and change to Published
- Observe status and updated timestamp

6. **API Demo (optional)**
```bash
curl https://react-fullstack-demo-app.orangepond-3f12b8c2.canadacentral.azurecontainerapps.io/api/posts
```

7. **CI/CD**
- Workflows: `ci`, `deploy-azure-dispatch`
- Build and push web + api images
- Terraform applies infrastructure and deployment

8. **IaC**
- `infra/terraform`: ACA + ACR + Log Analytics
- `infra/bootstrap`: TF state storage
