import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { BlogListDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-blogs',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  template: `
    <section class="section">
      <div class="container">
        <div class="section-header">
          <span class="section-tag">Blog</span>
          <h2>Latest Articles</h2>
          <p>Thoughts, tutorials, and insights on software development</p>
        </div>
        <div class="blog-filters">
          <input type="text" placeholder="🔍 Search articles..." [(ngModel)]="searchQuery" (input)="onSearch()" class="search-input" />
        </div>
        @if (loading()) {
          <div class="loading-container"><div class="spinner"></div></div>
        } @else {
          <div class="blogs-grid">
            @for (blog of blogs(); track blog.id) {
              <article class="blog-card glass-card">
                @if (blog.thumbnailUrl) {
                  <img [src]="blog.thumbnailUrl" [alt]="blog.title" class="blog-thumb" />
                } @else {
                  <div class="blog-thumb-placeholder">📝</div>
                }
                <div class="blog-body">
                  <div class="blog-meta">
                    <span class="badge badge-primary">{{ blog.category }}</span>
                    <span class="read-time">⏱ {{ blog.readTimeMinutes }} min read</span>
                  </div>
                  <h3>{{ blog.title }}</h3>
                  <p>{{ blog.summary }}</p>
                  <div class="blog-tags">
                    @for (tag of getTags(blog.tags); track tag) {
                      <span class="tag-chip">{{ tag }}</span>
                    }
                  </div>
                  <div class="blog-footer">
                    <span class="date">{{ blog.publishedAt | date:'MMM d, yyyy' }}</span>
                    <a [routerLink]="['/blogs', blog.slug]" class="read-more">Read More →</a>
                  </div>
                </div>
              </article>
            }
          </div>
          @if (blogs().length === 0) {
            <div class="empty-state">
              <span>📭</span>
              <p>No articles found.</p>
            </div>
          }
        }
      </div>
    </section>
  `,
  styleUrl: './blogs.component.scss'
})
export class BlogsComponent implements OnInit {
  blogs = signal<BlogListDto[]>([]);
  loading = signal(false);
  searchQuery = '';

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void { this.loadBlogs(); }

  loadBlogs(): void {
    this.loading.set(true);
    this.portfolioService.getBlogs({ search: this.searchQuery }).subscribe(r => {
      if (r.success) this.blogs.set(r.data.items);
      this.loading.set(false);
    });
  }

  onSearch(): void { this.loadBlogs(); }
  getTags(tags: string): string[] { return tags ? tags.split(',').map(t => t.trim()).slice(0, 3).filter(Boolean) : []; }
}
