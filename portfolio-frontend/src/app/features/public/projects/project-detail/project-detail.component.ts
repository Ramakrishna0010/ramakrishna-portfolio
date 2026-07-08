import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../../core/services/portfolio.service';
import { ProjectDto } from '../../../../core/models/portfolio.model';

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    @if (project()) {
      <section class="section">
        <div class="container">
          <a routerLink="/projects" class="back-link">← Back to Projects</a>
          <div class="project-hero glass-card">
            @if (project()!.thumbnailUrl) {
              <img [src]="project()!.thumbnailUrl" [alt]="project()!.title" class="hero-img" />
            }
            <div class="hero-content">
              <div class="badges">
                <span class="badge badge-primary">{{ project()!.category }}</span>
                <span class="badge badge-success">{{ project()!.statusName }}</span>
              </div>
              <h1>{{ project()!.title }}</h1>
              <p>{{ project()!.shortDescription }}</p>
              <div class="project-links">
                @if (project()!.gitHubUrl) {
                  <a [href]="project()!.gitHubUrl" target="_blank" class="btn-outline">🔗 GitHub</a>
                }
                @if (project()!.liveDemoUrl) {
                  <a [href]="project()!.liveDemoUrl" target="_blank" class="btn-primary">🚀 Live Demo</a>
                }
              </div>
            </div>
          </div>

          <div class="project-details">
            <div class="detail-main">
              <div class="detail-section glass-card">
                <h3>📋 Description</h3>
                <p>{{ project()!.description }}</p>
              </div>
              @if (project()!.features) {
                <div class="detail-section glass-card">
                  <h3>✨ Features</h3>
                  <ul>
                    @for (f of getList(project()!.features); track f) {
                      <li>{{ f }}</li>
                    }
                  </ul>
                </div>
              }
              @if (project()!.responsibilities) {
                <div class="detail-section glass-card">
                  <h3>👤 My Responsibilities</h3>
                  <ul>
                    @for (r of getList(project()!.responsibilities); track r) {
                      <li>{{ r }}</li>
                    }
                  </ul>
                </div>
              }
            </div>
            <div class="detail-sidebar">
              <div class="sidebar-card glass-card">
                <h4>Tech Stack</h4>
                <div class="tech-tags">
                  @for (tech of getTechs(project()!.technologyStack); track tech) {
                    <span class="tag-chip">{{ tech }}</span>
                  }
                </div>
              </div>
              <div class="sidebar-card glass-card">
                <h4>Project Info</h4>
                <div class="info-list">
                  <div><span>Architecture</span><span>{{ project()!.architecture }}</span></div>
                  <div><span>Duration</span><span>{{ project()!.duration }}</span></div>
                  <div><span>Views</span><span>{{ project()!.viewCount }}</span></div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    } @else {
      <div class="loading-container"><div class="spinner"></div></div>
    }
  `,
  styleUrl: './project-detail.component.scss'
})
export class ProjectDetailComponent implements OnInit {
  project = signal<ProjectDto | null>(null);

  constructor(private route: ActivatedRoute, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    const slug = this.route.snapshot.paramMap.get('slug')!;
    this.portfolioService.getProjectBySlug(slug).subscribe(r => { if (r.success) this.project.set(r.data); });
  }

  getTechs(tech: string): string[] { return tech.split(',').map(t => t.trim()).filter(Boolean); }
  getList(text: string): string[] { return text.split('\n').map(t => t.trim()).filter(Boolean); }
}
