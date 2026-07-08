import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { ProjectDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.scss'
})
export class ProjectsComponent implements OnInit {
  projects = signal<ProjectDto[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = 9;
  searchQuery = signal('');
  selectedCategory = signal('');
  categories = signal<string[]>([]);
  loading = signal(false);

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void { this.loadProjects(); }

  loadProjects(): void {
    this.loading.set(true);
    this.portfolioService.getProjects({
      search: this.searchQuery(),
      category: this.selectedCategory(),
      page: this.currentPage(),
      pageSize: this.pageSize
    }).subscribe(r => {
      if (r.success) {
        this.projects.set(r.data.items);
        this.totalCount.set(r.data.totalCount);
        const cats = [...new Set(r.data.items.map(p => p.category).filter(Boolean))];
        if (this.categories().length === 0) this.categories.set(cats);
      }
      this.loading.set(false);
    });
  }

  onSearch(): void { this.currentPage.set(1); this.loadProjects(); }
  onCategoryChange(cat: string): void { this.selectedCategory.set(cat); this.currentPage.set(1); this.loadProjects(); }
  getTechs(tech: string): string[] { return tech.split(',').map(t => t.trim()).slice(0, 4).filter(Boolean); }
  get totalPages(): number { return Math.ceil(this.totalCount() / this.pageSize); }
}
