import { Component, HostListener, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ThemeService } from '../../../core/services/theme.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  template: `
    <nav class="navbar" [class.scrolled]="isScrolled()" [class.menu-open]="menuOpen()">
      <div class="nav-container">
        <a routerLink="/" class="nav-brand">
          <span class="brand-icon">⚡</span>
          <span class="brand-text">Portfolio</span>
        </a>

        <ul class="nav-links" [class.open]="menuOpen()">
          <li><a routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{exact:true}" (click)="closeMenu()">Home</a></li>
          <li><a routerLink="/about" routerLinkActive="active" (click)="closeMenu()">About</a></li>
          <li><a routerLink="/skills" routerLinkActive="active" (click)="closeMenu()">Skills</a></li>
          <li><a routerLink="/experience" routerLinkActive="active" (click)="closeMenu()">Experience</a></li>
          <li><a routerLink="/projects" routerLinkActive="active" (click)="closeMenu()">Projects</a></li>
          <li><a routerLink="/blogs" routerLinkActive="active" (click)="closeMenu()">Blog</a></li>
          <li><a routerLink="/contact" routerLinkActive="active" (click)="closeMenu()">Contact</a></li>
          @if (auth.isAdmin()) {
            <li><a routerLink="/admin/dashboard" routerLinkActive="active" (click)="closeMenu()" class="admin-link">Admin</a></li>
          }
        </ul>

        <div class="nav-actions">
          <button class="theme-btn" (click)="themeService.toggleTheme()" [title]="themeService.isDarkMode() ? 'Light Mode' : 'Dark Mode'">
            {{ themeService.isDarkMode() ? '☀️' : '🌙' }}
          </button>
          @if (auth.isAuthenticated()) {
            <button class="btn-outline-sm" (click)="auth.logout()">Logout</button>
          } @else {
            <a routerLink="/auth/login" class="btn-outline-sm">Login</a>
          }
          <button class="hamburger" (click)="toggleMenu()" [class.open]="menuOpen()">
            <span></span><span></span><span></span>
          </button>
        </div>
      </div>
    </nav>
  `,
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  isScrolled = signal(false);
  menuOpen = signal(false);

  constructor(public themeService: ThemeService, public auth: AuthService) {}

  @HostListener('window:scroll')
  onScroll(): void { this.isScrolled.set(window.scrollY > 50); }

  toggleMenu(): void { this.menuOpen.update(v => !v); }
  closeMenu(): void { this.menuOpen.set(false); }
}
