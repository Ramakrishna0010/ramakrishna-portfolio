import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { SkillGroupDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-skills',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="section">
      <div class="container">
        <div class="section-header">
          <span class="section-tag">Skills</span>
          <h2>Technical Expertise</h2>
          <p>A comprehensive overview of my technical skills and proficiency levels</p>
        </div>
        @if (skillGroups().length > 0) {
          @for (group of skillGroups(); track group.category) {
            <div class="skill-category-section">
              <h3 class="category-title">{{ group.category }}</h3>
              <div class="skills-grid">
                @for (skill of group.skills; track skill.id) {
                  <div class="skill-card glass-card">
                    <div class="skill-header">
                      @if (skill.iconClass) {
                        <i [class]="skill.iconClass" class="skill-icon"></i>
                      } @else {
                        <span class="skill-icon-text">{{ skill.name[0] }}</span>
                      }
                      <div>
                        <h4>{{ skill.name }}</h4>
                        <span class="years">{{ skill.yearsOfExperience }}+ yrs</span>
                      </div>
                      <span class="pct-badge">{{ skill.percentage }}%</span>
                    </div>
                    <div class="progress-bar">
                      <div class="progress-fill" [style.width]="skill.percentage + '%'" [style.background]="skill.color || 'var(--gradient-primary)'"></div>
                    </div>
                    @if (skill.description) {
                      <p class="skill-desc">{{ skill.description }}</p>
                    }
                  </div>
                }
              </div>
            </div>
          }
        } @else {
          <div class="loading-container"><div class="spinner"></div></div>
        }
      </div>
    </section>
  `,
  styles: [`
    .skill-category-section { margin-bottom: 3rem; }
    .category-title { font-size: 1.5rem; margin-bottom: 1.5rem; background: var(--gradient-primary); -webkit-background-clip: text; -webkit-text-fill-color: transparent; }
    .skills-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 1.5rem; }
    .skill-card { padding: 1.5rem; }
    .skill-header { display: flex; align-items: center; gap: 1rem; margin-bottom: 1rem; }
    .skill-icon { font-size: 2rem; color: var(--accent-primary); }
    .skill-icon-text { width: 40px; height: 40px; background: var(--gradient-primary); border-radius: 10px; display: flex; align-items: center; justify-content: center; color: white; font-weight: 700; font-size: 1.2rem; }
    h4 { font-size: 1rem; margin-bottom: 0.2rem; }
    .years { color: var(--text-muted); font-size: 0.8rem; }
    .pct-badge { margin-left: auto; background: var(--gradient-primary); -webkit-background-clip: text; -webkit-text-fill-color: transparent; font-weight: 700; font-size: 1.1rem; }
    .skill-desc { color: var(--text-muted); font-size: 0.85rem; margin-top: 0.8rem; line-height: 1.5; }
  `]
})
export class SkillsComponent implements OnInit {
  skillGroups = signal<SkillGroupDto[]>([]);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getSkills().subscribe(r => { if (r.success) this.skillGroups.set(r.data); });
  }
}
