import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { ServiceDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-services-section',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './services-section.component.html',
  styles: [`
    .services-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 2rem; }
    .service-card { padding: 2rem; border-top: 3px solid var(--accent-primary); }
    .svc-icon { width: 60px; height: 60px; border-radius: 16px; display: flex; align-items: center; justify-content: center; font-size: 1.8rem; margin-bottom: 1.5rem; }
    h3 { font-size: 1.2rem; margin-bottom: 0.75rem; }
    p { color: var(--text-muted); font-size: 0.9rem; line-height: 1.6; margin-bottom: 1.5rem; }
    .price { font-size: 1.3rem; font-weight: 700; color: var(--accent-primary); margin-bottom: 1.5rem; }
    .price span { font-size: 0.85rem; color: var(--text-muted); font-weight: 400; }
    .contact-btn { width: 100%; justify-content: center; }
  `]
})
export class ServicesSectionComponent implements OnInit {
  services = signal<ServiceDto[]>([]);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getServices().subscribe(r => { if (r.success) this.services.set(r.data); });
  }

  getPriceLabel(svc: ServiceDto): string {
    return svc.startingPrice ? `From $${svc.startingPrice} / ${svc.pricingUnit}` : '';
  }
}
