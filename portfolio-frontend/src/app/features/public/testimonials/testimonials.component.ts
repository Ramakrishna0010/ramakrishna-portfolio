import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { TestimonialDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-testimonials',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="section">
      <div class="container">
        <div class="section-header">
          <span class="section-tag">Testimonials</span>
          <h2>Client Reviews</h2>
        </div>
        <div class="testimonials-grid">
          @for (t of testimonials(); track t.id) {
            <div class="testimonial-card glass-card">
              <div class="stars">{{ '⭐'.repeat(t.rating) }}</div>
              <p class="content">"{{ t.content }}"</p>
              @if (t.projectWorkedOn) { <p class="project">Project: {{ t.projectWorkedOn }}</p> }
              <div class="author">
                @if (t.clientImageUrl) {
                  <img [src]="t.clientImageUrl" [alt]="t.clientName" class="avatar" />
                } @else {
                  <div class="avatar-placeholder">{{ t.clientName[0] }}</div>
                }
                <div>
                  <strong>{{ t.clientName }}</strong>
                  <span>{{ t.clientTitle }}@if (t.clientCompany) {, {{ t.clientCompany }}}</span>
                </div>
              </div>
            </div>
          }
        </div>
        @if (testimonials().length === 0) {
          <div class="loading-container"><div class="spinner"></div></div>
        }
      </div>
    </section>
  `,
  styles: [`
    .testimonials-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(320px, 1fr)); gap: 2rem; }
    .testimonial-card { padding: 2rem; }
    .stars { font-size: 1.1rem; margin-bottom: 1rem; }
    .content { color: var(--text-secondary); font-style: italic; line-height: 1.7; margin-bottom: 0.75rem; }
    .project { color: var(--accent-primary); font-size: 0.85rem; margin-bottom: 1.5rem; }
    .author { display: flex; align-items: center; gap: 1rem; }
    .avatar { width: 48px; height: 48px; border-radius: 50%; object-fit: cover; }
    .avatar-placeholder { width: 48px; height: 48px; border-radius: 50%; background: var(--gradient-primary); display: flex; align-items: center; justify-content: center; color: white; font-weight: 700; }
    strong { display: block; font-size: 0.95rem; }
    span { color: var(--text-muted); font-size: 0.8rem; }
  `]
})
export class TestimonialsComponent implements OnInit {
  testimonials = signal<TestimonialDto[]>([]);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getTestimonials().subscribe(r => { if (r.success) this.testimonials.set(r.data); });
  }
}
