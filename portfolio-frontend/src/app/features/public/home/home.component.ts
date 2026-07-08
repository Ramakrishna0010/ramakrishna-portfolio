import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { AboutDto, SkillGroupDto, ProjectDto, TestimonialDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  about = signal<AboutDto | null>(null);
  skillGroups = signal<SkillGroupDto[]>([]);
  featuredProjects = signal<ProjectDto[]>([]);
  testimonials = signal<TestimonialDto[]>([]);
  typedText = signal('');

  private titles = ['Full Stack Developer', 'ASP.NET Core Expert', 'Angular Developer', 'Cloud Architect'];
  private titleIndex = 0;
  private charIndex = 0;
  private isDeleting = false;

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.portfolioService.getAbout().subscribe(r => { if (r.success) this.about.set(r.data); });
    this.portfolioService.getSkills().subscribe(r => { if (r.success) this.skillGroups.set(r.data); });
    this.portfolioService.getProjects({ pageSize: 3 }).subscribe(r => { if (r.success) this.featuredProjects.set(r.data.items); });
    this.portfolioService.getTestimonials().subscribe(r => { if (r.success) this.testimonials.set(r.data.filter(t => t.isFeatured).slice(0, 3)); });
    this.typeEffect();
  }

  private typeEffect(): void {
    const current = this.titles[this.titleIndex];
    if (this.isDeleting) {
      this.typedText.set(current.substring(0, this.charIndex - 1));
      this.charIndex--;
    } else {
      this.typedText.set(current.substring(0, this.charIndex + 1));
      this.charIndex++;
    }
    let delay = this.isDeleting ? 60 : 100;
    if (!this.isDeleting && this.charIndex === current.length) { delay = 2000; this.isDeleting = true; }
    else if (this.isDeleting && this.charIndex === 0) { this.isDeleting = false; this.titleIndex = (this.titleIndex + 1) % this.titles.length; delay = 300; }
    setTimeout(() => this.typeEffect(), delay);
  }

  getTechArray(tech: string): string[] {
    return tech ? tech.split(',').map(t => t.trim()).slice(0, 4) : [];
  }

  getStars(rating: number): number[] {
    return Array(rating).fill(0);
  }
}
