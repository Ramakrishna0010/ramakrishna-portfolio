import { Routes } from '@angular/router';
import { authGuard, guestGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./shared/components/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      { path: '', loadComponent: () => import('./features/public/home/home.component').then(m => m.HomeComponent) },
      { path: 'about', loadComponent: () => import('./features/public/about/about.component').then(m => m.AboutComponent) },
      { path: 'skills', loadComponent: () => import('./features/public/skills/skills.component').then(m => m.SkillsComponent) },
      { path: 'experience', loadComponent: () => import('./features/public/experience/experience.component').then(m => m.ExperienceComponent) },
      { path: 'education', loadComponent: () => import('./features/public/education/education.component').then(m => m.EducationComponent) },
      { path: 'projects', loadComponent: () => import('./features/public/projects/projects.component').then(m => m.ProjectsComponent) },
      { path: 'projects/:slug', loadComponent: () => import('./features/public/projects/project-detail/project-detail.component').then(m => m.ProjectDetailComponent) },
      { path: 'blogs', loadComponent: () => import('./features/public/blogs/blogs.component').then(m => m.BlogsComponent) },
      { path: 'blogs/:slug', loadComponent: () => import('./features/public/blogs/blog-detail/blog-detail.component').then(m => m.BlogDetailComponent) },
      { path: 'certificates', loadComponent: () => import('./features/public/certificates/certificates.component').then(m => m.CertificatesComponent) },
      { path: 'achievements', loadComponent: () => import('./features/public/achievements/achievements.component').then(m => m.AchievementsComponent) },
      { path: 'services', loadComponent: () => import('./features/public/services-section/services-section.component').then(m => m.ServicesSectionComponent) },
      { path: 'testimonials', loadComponent: () => import('./features/public/testimonials/testimonials.component').then(m => m.TestimonialsComponent) },
      { path: 'contact', loadComponent: () => import('./features/public/contact/contact.component').then(m => m.ContactComponent) },
    ]
  },
  {
    path: 'auth',
    children: [
      { path: 'login', canActivate: [guestGuard], loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
    ]
  },
  {
    path: 'admin',
    canActivate: [authGuard],
    loadComponent: () => import('./features/admin/admin-layout/admin-layout.component').then(m => m.AdminLayoutComponent),
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./features/admin/dashboard/dashboard.component').then(m => m.DashboardComponent) },
      { path: 'about', loadComponent: () => import('./features/admin/admin-about/admin-about.component').then(m => m.AdminAboutComponent) },
      { path: 'skills', loadComponent: () => import('./features/admin/admin-skills/admin-skills.component').then(m => m.AdminSkillsComponent) },
      { path: 'experience', loadComponent: () => import('./features/admin/admin-experience/admin-experience.component').then(m => m.AdminExperienceComponent) },
      { path: 'education', loadComponent: () => import('./features/admin/admin-education/admin-education.component').then(m => m.AdminEducationComponent) },
      { path: 'projects', loadComponent: () => import('./features/admin/admin-projects/admin-projects.component').then(m => m.AdminProjectsComponent) },
      { path: 'blogs', loadComponent: () => import('./features/admin/admin-blogs/admin-blogs.component').then(m => m.AdminBlogsComponent) },
      { path: 'contact', loadComponent: () => import('./features/admin/admin-contact/admin-contact.component').then(m => m.AdminContactComponent) },
      { path: 'certificates', loadComponent: () => import('./features/admin/admin-certificates/admin-certificates.component').then(m => m.AdminCertificatesComponent) },
      { path: 'achievements', loadComponent: () => import('./features/admin/admin-achievements/admin-achievements.component').then(m => m.AdminAchievementsComponent) },
      { path: 'testimonials', loadComponent: () => import('./features/admin/admin-testimonials/admin-testimonials.component').then(m => m.AdminTestimonialsComponent) },
      { path: 'resume', loadComponent: () => import('./features/admin/admin-resume/admin-resume.component').then(m => m.AdminResumeComponent) },
      { path: 'services', loadComponent: () => import('./features/admin/admin-services/admin-services.component').then(m => m.AdminServicesComponent) },
    ]
  },
  { path: '**', redirectTo: '' }
];
