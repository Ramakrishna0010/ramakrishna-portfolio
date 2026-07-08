import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { ExperienceDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-experience',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="section">
      <div class="container">
        <div class="section-header">
          <span class="section-tag">Career</span>
          <h2>Work Experience</h2>
          <p>My professional journey and the companies I've worked with</p>
        </div>
        @if (experiences().length > 0) {
          <div class="timeline">
            @for (exp of experiences(); track exp.id; let i = $index) {
              <div class="timeline-item" [class.right]="i % 2 !== 0">
                <div class="timeline-dot"></div>
                <div class="timeline-card glass-card">
                  <div class="card-header">
                    @if (exp.companyLogoUrl) {
                      <img [src]="exp.companyLogoUrl" [alt]="exp.companyName" class="company-logo" />
                    }
                    <div>
                      <h3>{{ exp.designation }}</h3>
                      <a [href]="exp.companyWebsite" target="_blank" class="company-name">{{ exp.companyName }}</a>
                    </div>
                    @if (exp.isCurrentJob) {
                      <span class="badge badge-success current-badge">Current</span>
                    }
                  </div>
                  <div class="meta-row">
                    <span>📅 {{ exp.duration }}</span>
                    <span>📍 {{ exp.location }}{{ exp.isRemote ? ' (Remote)' : '' }}</span>
                    <span>💼 {{ exp.employmentType }}</span>
                  </div>
                  <p class="description">{{ exp.description }}</p>
                  @if (exp.technologiesUsed) {
                    <div class="tech-tags">
                      @for (tech of getTechs(exp.technologiesUsed); track tech) {
                        <span class="tag-chip">{{ tech }}</span>
                      }
                    </div>
                  }
                </div>
              </div>
            }
          </div>
        } @else {
          <div class="loading-container"><div class="spinner"></div></div>
        }
      </div>
    </section>
  `,
  styleUrl: './experience.component.scss'
})
export class ExperienceComponent implements OnInit {
  experiences = signal<ExperienceDto[]>([]);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getExperiences().subscribe(r => { if (r.success) this.experiences.set(r.data); });
  }

  getTechs(tech: string): string[] {
    return tech.split(',').map(t => t.trim()).filter(Boolean);
  }
}
