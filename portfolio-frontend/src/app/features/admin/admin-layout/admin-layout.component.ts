import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { ThemeService } from '../../../core/services/theme.service';

interface NavItem { label: string; icon: string; route: string; }

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  sidebarOpen = signal(true);

  navItems: NavItem[] = [
    { label: 'Dashboard', icon: '📊', route: '/admin/dashboard' },
    { label: 'About', icon: '👤', route: '/admin/about' },
    { label: 'Skills', icon: '⚡', route: '/admin/skills' },
    { label: 'Experience', icon: '💼', route: '/admin/experience' },
    { label: 'Education', icon: '🎓', route: '/admin/education' },
    { label: 'Projects', icon: '🚀', route: '/admin/projects' },
    { label: 'Blogs', icon: '📝', route: '/admin/blogs' },
    { label: 'Certificates', icon: '🏆', route: '/admin/certificates' },
    { label: 'Achievements', icon: '🥇', route: '/admin/achievements' },
    { label: 'Testimonials', icon: '💬', route: '/admin/testimonials' },
    { label: 'Services', icon: '⚙️', route: '/admin/services' },
    { label: 'Contact', icon: '📧', route: '/admin/contact' },
    { label: 'Resume', icon: '📄', route: '/admin/resume' },
  ];

  toggleSidebar(): void { this.sidebarOpen.set(!this.sidebarOpen()); }

  constructor(public auth: AuthService, public themeService: ThemeService) {}
}
