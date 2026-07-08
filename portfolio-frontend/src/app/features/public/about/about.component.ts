import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { AboutDto, SocialLinkDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section class="section about-page">
      <div class="container">
        <div class="section-header">
          <span class="section-tag">About Me</span>
          <h2>Who I Am</h2>
        </div>
        @if (about()) {
          <div class="about-grid">
            <div class="about-image animate-fade-left">
              <div class="image-frame">
                @if (about()!.profileImageUrl) {
                  <img [src]="about()!.profileImageUrl" [alt]="about()!.fullName" />
                } @else {
                  <div class="img-placeholder">👨‍💻</div>
                }
              </div>
              <div class="availability-badge" [class.available]="about()!.isAvailableForWork">
                <span class="dot"></span>
                {{ about()!.availabilityStatus }}
              </div>
            </div>
            <div class="about-content animate-fade-up">
              <h1>{{ about()!.fullName }}</h1>
              <h3 class="title-gradient">{{ about()!.title }}</h3>
              <p class="bio">{{ about()!.bio }}</p>
              <div class="info-grid">
                <div class="info-item"><span class="info-label">📧 Email</span><span>{{ about()!.email }}</span></div>
                <div class="info-item"><span class="info-label">📍 Location</span><span>{{ about()!.location }}</span></div>
                <div class="info-item"><span class="info-label">🌍 Nationality</span><span>{{ about()!.nationality }}</span></div>
                <div class="info-item"><span class="info-label">🗣️ Languages</span><span>{{ about()!.languages }}</span></div>
              </div>
              <div class="about-actions">
                <a routerLink="/contact" class="btn-primary">Hire Me</a>
                <a [href]="about()!.resumeUrl" target="_blank" class="btn-outline" *ngIf="about()!.resumeUrl">Download CV</a>
              </div>
            </div>
          </div>
          <div class="stats-row">
            <div class="stat-card glass-card">
              <div class="stat-number">{{ about()!.yearsOfExperience }}+</div>
              <div class="stat-label">Years of Experience</div>
            </div>
            <div class="stat-card glass-card">
              <div class="stat-number">{{ about()!.projectsCompleted }}+</div>
              <div class="stat-label">Projects Completed</div>
            </div>
            <div class="stat-card glass-card">
              <div class="stat-number">{{ about()!.happyClients }}+</div>
              <div class="stat-label">Happy Clients</div>
            </div>
          </div>
        } @else {
          <div class="loading-container"><div class="spinner"></div></div>
        }
      </div>
    </section>
  `,
  styleUrl: './about.component.scss'
})
export class AboutComponent implements OnInit {
  about = signal<AboutDto | null>(null);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getAbout().subscribe(r => { if (r.success) this.about.set(r.data); });
  }
}
