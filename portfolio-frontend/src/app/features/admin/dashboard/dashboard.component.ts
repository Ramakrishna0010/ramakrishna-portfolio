import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PortfolioService } from '../../../core/services/portfolio.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="dashboard">
      <div class="page-header">
        <h1>Dashboard</h1>
        <p>Welcome back! Here's an overview of your portfolio.</p>
      </div>

      <div class="stats-grid">
        @for (stat of stats(); track stat.label) {
          <div class="stat-widget glass-card">
            <div class="stat-icon">{{ stat.icon }}</div>
            <div class="stat-info">
              <div class="stat-value">{{ stat.value }}</div>
              <div class="stat-label">{{ stat.label }}</div>
            </div>
            <a [routerLink]="stat.route" class="stat-link">Manage →</a>
          </div>
        }
      </div>

      <div class="quick-actions">
        <h3>Quick Actions</h3>
        <div class="actions-grid">
          <a routerLink="/admin/projects" class="action-card glass-card">
            <span>🚀</span>
            <span>Add Project</span>
          </a>
          <a routerLink="/admin/blogs" class="action-card glass-card">
            <span>📝</span>
            <span>Write Blog</span>
          </a>
          <a routerLink="/admin/contact" class="action-card glass-card">
            <span>📧</span>
            <span>View Messages</span>
          </a>
          <a routerLink="/admin/resume" class="action-card glass-card">
            <span>📄</span>
            <span>Update Resume</span>
          </a>
        </div>
      </div>
    </div>
  `,
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  stats = signal<any[]>([]);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    const statDefs = [
      { icon: '🚀', label: 'Projects', route: '/admin/projects', fetch: () => this.portfolioService.getProjects({ pageSize: 1 }) },
      { icon: '📝', label: 'Blog Posts', route: '/admin/blogs', fetch: () => this.portfolioService.getBlogs({ pageSize: 1 }) },
      { icon: '📧', label: 'Messages', route: '/admin/contact', fetch: () => this.portfolioService.getMessages() },
    ];

    this.stats.set([
      { icon: '🚀', label: 'Projects', value: '—', route: '/admin/projects' },
      { icon: '📝', label: 'Blog Posts', value: '—', route: '/admin/blogs' },
      { icon: '📧', label: 'Messages', value: '—', route: '/admin/contact' },
      { icon: '⚡', label: 'Skills', value: '—', route: '/admin/skills' },
      { icon: '💼', label: 'Experience', value: '—', route: '/admin/experience' },
      { icon: '🏆', label: 'Certificates', value: '—', route: '/admin/certificates' },
    ]);

    this.portfolioService.getProjects({ pageSize: 1 }).subscribe(r => {
      if (r.success) this.updateStat('Projects', r.data.totalCount);
    });
    this.portfolioService.getBlogs({ pageSize: 1 }).subscribe(r => {
      if (r.success) this.updateStat('Blog Posts', r.data.totalCount);
    });
    this.portfolioService.getMessages().subscribe(r => {
      if (r.success) this.updateStat('Messages', r.data.totalCount);
    });
    this.portfolioService.getAllSkills().subscribe(r => {
      if (r.success) this.updateStat('Skills', r.data.totalCount);
    });
    this.portfolioService.getExperiences().subscribe(r => {
      if (r.success) this.updateStat('Experience', r.data.length);
    });
    this.portfolioService.getCertificates().subscribe(r => {
      if (r.success) this.updateStat('Certificates', r.data.length);
    });
  }

  private updateStat(label: string, value: number): void {
    this.stats.update(stats => stats.map(s => s.label === label ? { ...s, value } : s));
  }
}
