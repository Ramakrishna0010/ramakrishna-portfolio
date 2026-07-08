import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { AchievementDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-achievements',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="section">
      <div class="container">
        <div class="section-header">
          <span class="section-tag">Achievements</span>
          <h2>Awards & Recognition</h2>
        </div>
        <div class="achievements-grid">
          @for (ach of achievements(); track ach.id) {
            <div class="ach-card glass-card">
              <div class="ach-icon">
                @if (ach.iconClass) { <i [class]="ach.iconClass"></i> }
                @else if (ach.imageUrl) { <img [src]="ach.imageUrl" [alt]="ach.title" /> }
                @else { <span>🏆</span> }
              </div>
              <div class="ach-body">
                <span class="badge badge-warning">{{ ach.category }}</span>
                <h3>{{ ach.title }}</h3>
                <p>{{ ach.description }}</p>
                <div class="ach-meta">
                  <span>📅 {{ ach.achievedDate | date:'MMM yyyy' }}</span>
                  @if (ach.issuedBy) { <span>🏢 {{ ach.issuedBy }}</span> }
                </div>
                @if (ach.awardUrl) {
                  <a [href]="ach.awardUrl" target="_blank" class="btn-ghost" style="font-size:0.8rem;padding:0.3rem 0.8rem">View Award</a>
                }
              </div>
            </div>
          }
        </div>
        @if (achievements().length === 0) {
          <div class="loading-container"><div class="spinner"></div></div>
        }
      </div>
    </section>
  `,
  styles: [`
    .achievements-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(320px, 1fr)); gap: 2rem; }
    .ach-card { padding: 2rem; display: flex; gap: 1.5rem; align-items: flex-start; }
    .ach-icon { font-size: 3rem; flex-shrink: 0; img { width: 60px; height: 60px; object-fit: contain; } }
    .ach-body { flex: 1; }
    h3 { font-size: 1.1rem; margin: 0.5rem 0; }
    p { color: var(--text-muted); font-size: 0.9rem; line-height: 1.5; margin-bottom: 0.75rem; }
    .ach-meta { display: flex; gap: 1rem; flex-wrap: wrap; margin-bottom: 0.75rem; span { color: var(--text-muted); font-size: 0.8rem; } }
  `]
})
export class AchievementsComponent implements OnInit {
  achievements = signal<AchievementDto[]>([]);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getAchievements().subscribe(r => { if (r.success) this.achievements.set(r.data); });
  }
}
