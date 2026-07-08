import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  isDarkMode = signal<boolean>(this.getStoredTheme());

  toggleTheme(): void {
    const newValue = !this.isDarkMode();
    this.isDarkMode.set(newValue);
    localStorage.setItem('portfolio_theme', newValue ? 'dark' : 'light');
    document.documentElement.setAttribute('data-theme', newValue ? 'dark' : 'light');
  }

  initTheme(): void {
    const isDark = this.isDarkMode();
    document.documentElement.setAttribute('data-theme', isDark ? 'dark' : 'light');
  }

  private getStoredTheme(): boolean {
    const stored = localStorage.getItem('portfolio_theme');
    return stored ? stored === 'dark' : window.matchMedia('(prefers-color-scheme: dark)').matches;
  }
}
