# 🚀 Portfolio Application

A **production-ready** full-stack personal portfolio built with **ASP.NET Core 8** and **Angular 18**.

---

## 🏗️ Architecture

```
portfolio/
├── src/
│   ├── PortfolioApp.API/           # Presentation Layer (Controllers, Middleware)
│   ├── PortfolioApp.Application/   # Application Layer (Services, DTOs, Validators)
│   ├── PortfolioApp.Domain/        # Domain Layer (Entities, Interfaces, Enums)
│   └── PortfolioApp.Infrastructure/# Infrastructure Layer (EF Core, Repositories)
├── portfolio-frontend/             # Angular 18 SPA
├── database/                       # SQL scripts
├── docs/                           # Documentation
├── .github/workflows/              # CI/CD pipelines
├── docker-compose.yml
└── PortfolioApp.sln
```

---

## ⚙️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core 8, C#, EF Core, SQL Server |
| Frontend | Angular 18, TypeScript, Angular Material, Bootstrap 5 |
| Auth | JWT + Refresh Tokens, Role-Based Authorization |
| Patterns | Clean Architecture, Repository, Unit of Work, CQRS-lite |
| DevOps | Docker, GitHub Actions, Azure App Service |

---

## 🚀 Quick Start

### Option 1 — Docker Compose (Recommended)
```bash
docker-compose up --build
```
- Frontend: http://localhost:4200
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Option 2 — Manual

**Backend**
```bash
cd src/PortfolioApp.API
# Update appsettings.json connection string
dotnet ef database update
dotnet run
```

**Frontend**
```bash
cd portfolio-frontend
npm install
npm start
```

---

## 🔐 Default Admin Credentials
After seeding:
- **Email:** `admin@portfolio.com`
- **Password:** `Admin@123!`

---

## 📋 Portfolio Sections

| Section | Public | Admin CRUD |
|---------|--------|-----------|
| Home | ✅ | — |
| About | ✅ | ✅ |
| Skills | ✅ | ✅ |
| Experience | ✅ | ✅ |
| Education | ✅ | ✅ |
| Projects | ✅ | ✅ |
| Blogs | ✅ | ✅ |
| Certificates | ✅ | ✅ |
| Achievements | ✅ | ✅ |
| Testimonials | ✅ | ✅ |
| Services | ✅ | ✅ |
| Contact | ✅ | ✅ (view messages) |
| Resume | ✅ (download) | ✅ (upload) |

---

## 🌐 API Endpoints

| Module | Endpoint |
|--------|---------|
| Auth | `POST /api/auth/login`, `POST /api/auth/register`, `POST /api/auth/refresh-token` |
| About | `GET /api/about`, `POST /api/about`, `PUT /api/about/{id}` |
| Skills | `GET /api/skills`, `GET /api/skills/all`, `POST`, `PUT`, `DELETE` |
| Experience | `GET /api/experience`, `POST`, `PUT/{id}`, `DELETE/{id}` |
| Projects | `GET /api/projects?search=&category=&pageNumber=&pageSize=` |
| Blogs | `GET /api/blogs?search=&category=`, `GET /api/blogs/{slug}` |
| Contact | `POST /api/contact`, `GET /api/contact` (admin), `PUT /api/contact/{id}/status` |
| Resume | `POST /api/resume/upload`, `GET /api/resume/download` |

Full Swagger docs at `/swagger` when running locally.

---

## 🎨 UI Features
- Glassmorphism design
- Dark / Light mode (persisted)
- Animated hero with typewriter effect
- Responsive (mobile, tablet, desktop)
- Smooth page transitions (Angular View Transitions API)

---

## ☁️ Azure Deployment

### Backend — Azure App Service
```bash
az webapp up --name your-api-name --resource-group your-rg --runtime "DOTNET:8.0"
```

### Frontend — Azure Static Web Apps
```bash
# Via GitHub Actions (auto-deploys on push to main)
# Or manually:
npm run build:prod
az staticwebapp deploy --name your-app --source dist/portfolio-frontend/browser
```

### Database — Azure SQL
```bash
az sql server create --name your-sql-server --resource-group your-rg --admin-user sqladmin --admin-password YourPassword@123
az sql db create --name PortfolioDb --server your-sql-server --resource-group your-rg --tier Basic
```

Update `ConnectionStrings:DefaultConnection` in Azure App Service → Configuration.

---

## 🔒 Security
- JWT Bearer authentication
- Refresh token rotation
- BCrypt password hashing
- CORS policy (Angular origin only)
- Global exception middleware
- Input validation (FluentValidation)
- SQL injection prevention (EF Core parameterized queries)

---

## 📦 CI/CD
GitHub Actions pipeline (`.github/workflows/ci-cd.yml`):
1. Build & test backend (.NET 8)
2. Build frontend (Angular 18)
3. Deploy to Azure on `main` branch push

Required GitHub Secrets:
- `AZURE_WEBAPP_NAME_API`
- `AZURE_WEBAPP_PUBLISH_PROFILE_API`
- `AZURE_STATIC_WEB_APPS_API_TOKEN`
