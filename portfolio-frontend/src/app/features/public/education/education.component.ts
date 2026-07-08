import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { EducationDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-education',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="section">
      <div class="container">
        <div class="section-header">
          <span class="section-tag">Education</span>
          <h2>Academic Background</h2>
        </div>
        <div class="education-grid">
          @for (edu of educations(); track edu.id) {
            <div class="edu-card glass-card">
              <div class="edu-header">
                @if (edu.institutionLogoUrl) {
                  <img [src]="edu.institutionLogoUrl" [alt]="edu.institutionName" class="inst-logo" />
                } @else {
                  <div class="inst-icon">🎓</div>
                }
                <div class="edu-meta">
                  <h3>{{ edu.degree }}</h3>
                  <h4>{{ edu.fieldOfStudy }}</h4>
                  <span class="institution">{{ edu.institutionName }}</span>
                </div>
                @if (edu.isCurrentlyStudying) {
                  <span class="badge badge-success">Current</span>
                }
              </div>
              <div class="edu-details">
                <span>📅 {{ edu.startDate | date:'MMM yyyy' }} - {{ edu.isCurrentlyStudying ? 'Present' : (edu.endDate | date:'MMM yyyy') }}</span>
                <span>📍 {{ edu.location }}</span>
                @if (edu.grade) { <span>🏆 {{ edu.grade }}</span> }
              </div>
              @if (edu.description) { <p class="edu-desc">{{ edu.description }}</p> }
            </div>
          }
        </div>
        @if (educations().length === 0) {
          <div class="loading-container"><div class="spinner"></div></div>
        }
      </div>
    </section>
  `,
  styles: [`
    .education-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(350px, 1fr)); gap: 2rem; }
    .edu-card { padding: 2rem; }
    .edu-header { display: flex; align-items: flex-start; gap: 1rem; margin-bottom: 1rem; }
    .inst-logo { width: 56px; height: 56px; object-fit: contain; border-radius: 12px; }
    .inst-icon { width: 56px; height: 56px; background: var(--gradient-primary); border-radius: 12px; display: flex; align-items: center; justify-content: center; font-size: 2rem; }
    .edu-meta h3 { font-size: 1.1rem; margin-bottom: 0.2rem; }
    .edu-meta h4 { color: var(--accent-primary); font-size: 0.95rem; font-weight: 500; margin-bottom: 0.2rem; }
    .institution { color: var(--text-muted); font-size: 0.85rem; }
    .edu-details { display: flex; flex-wrap: wrap; gap: 1rem; margin-bottom: 1rem; span { color: var(--text-muted); font-size: 0.85rem; } }
    .edu-desc { color: var(--text-secondary); font-size: 0.9rem; line-height: 1.6; }
  `]
})
export class EducationComponent implements OnInit {
  educations = signal<EducationDto[]>([]);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getEducations().subscribe(r => { if (r.success) this.educations.set(r.data); });
  }
}
