# Portfolio Frontend - Angular 18

## 🚀 Tech Stack
- Angular 18 (Standalone Components)
- TypeScript
- Angular Material 18
- Bootstrap 5
- RxJS
- SCSS with CSS Variables (Dark/Light Mode)

## 📁 Project Structure

```
src/app/
├── core/
│   ├── guards/          # Auth & Guest guards
│   ├── interceptors/    # JWT auth interceptor
│   ├── models/          # TypeScript interfaces
│   └── services/        # Auth, Portfolio, Theme services
├── features/
│   ├── auth/login/      # Admin login page
│   ├── public/          # Public portfolio pages
│   │   ├── home/
│   │   ├── about/
│   │   ├── skills/
│   │   ├── experience/
│   │   ├── education/
│   │   ├── projects/
│   │   ├── blogs/
│   │   ├── certificates/
│   │   ├── achievements/
│   │   ├── services-section/
│   │   ├── testimonials/
│   │   └── contact/
│   └── admin/           # Protected admin dashboard
│       ├── admin-layout/
│       ├── dashboard/
│       ├── admin-about/
│       ├── admin-skills/
│       ├── admin-experience/
│       ├── admin-education/
│       ├── admin-projects/
│       ├── admin-blogs/
│       ├── admin-certificates/
│       ├── admin-achievements/
│       ├── admin-testimonials/
│       ├── admin-services/
│       ├── admin-contact/
│       └── admin-resume/
└── shared/
    └── components/
        ├── layout/      # Public layout wrapper
        ├── navbar/
        └── footer/
```

## ⚙️ Setup & Run

### Prerequisites
- Node.js 18+
- Angular CLI 18

### Install
```bash
npm install
```

### Development
```bash
npm start
# Runs on http://localhost:4200
# API proxied to http://localhost:5000
```

### Production Build
```bash
npm run build:prod
```

## 🔐 Authentication
- Admin login at `/auth/login`
- JWT stored in localStorage
- Auto refresh token on 401
- Route guards protect `/admin/**`

## 🎨 Design Features
- Glassmorphism cards
- Gradient backgrounds
- Dark / Light mode toggle (persisted)
- Smooth CSS animations
- Fully responsive (mobile, tablet, desktop)
- Typewriter hero effect

## 🌐 Environment Config
Edit `src/environments/environment.ts` for local API URL.
Edit `src/environments/environment.prod.ts` for production API URL.

## 🚢 Azure Deployment
```bash
npm run build:prod
# Deploy dist/portfolio-frontend to Azure Static Web Apps
# or Azure App Service (serve with nginx/node)
```
