import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { CertificateDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-certificates',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="section">
      <div class="container">
        <div class="section-header">
          <span class="section-tag">Certifications</span>
          <h2>My Certificates</h2>
        </div>
        <div class="certs-grid">
          @for (cert of certificates(); track cert.id) {
            <div class="cert-card glass-card">
              @if (cert.issuedByLogoUrl) {
                <img [src]="cert.issuedByLogoUrl" [alt]="cert.issuedBy" class="issuer-logo" />
              }
              <h3>{{ cert.name }}</h3>
              <p class="issuer">Issued by {{ cert.issuedBy }}</p>
              <div class="cert-meta">
                <span>📅 {{ cert.issueDate | date:'MMM yyyy' }}</span>
                @if (!cert.doesNotExpire && cert.expiryDate) {
                  <span>Expires: {{ cert.expiryDate | date:'MMM yyyy' }}</span>
                } @else if (cert.doesNotExpire) {
                  <span class="badge badge-success">No Expiry</span>
                }
              </div>
              @if (cert.credentialUrl) {
                <a [href]="cert.credentialUrl" target="_blank" class="btn-ghost verify-btn">🔗 Verify</a>
              }
            </div>
          }
        </div>
        @if (certificates().length === 0) {
          <div class="loading-container"><div class="spinner"></div></div>
        }
      </div>
    </section>
  `,
  styles: [`
    .certs-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 2rem; }
    .cert-card { padding: 2rem; text-align: center; }
    .issuer-logo { width: 60px; height: 60px; object-fit: contain; margin: 0 auto 1rem; display: block; }
    h3 { font-size: 1rem; margin-bottom: 0.5rem; }
    .issuer { color: var(--accent-primary); font-size: 0.9rem; margin-bottom: 1rem; }
    .cert-meta { display: flex; justify-content: center; gap: 1rem; flex-wrap: wrap; margin-bottom: 1rem; span { color: var(--text-muted); font-size: 0.8rem; } }
    .verify-btn { font-size: 0.85rem; padding: 0.4rem 1rem; }
  `]
})
export class CertificatesComponent implements OnInit {
  certificates = signal<CertificateDto[]>([]);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getCertificates().subscribe(r => { if (r.success) this.certificates.set(r.data); });
  }
}
