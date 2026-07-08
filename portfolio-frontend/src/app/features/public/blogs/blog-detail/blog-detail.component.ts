import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PortfolioService } from '../../../../core/services/portfolio.service';
import { BlogDto } from '../../../../core/models/portfolio.model';

@Component({
  selector: 'app-blog-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    @if (blog()) {
      <section class="section">
        <div class="container blog-container">
          <a routerLink="/blogs" class="back-link">← Back to Blog</a>
          <article class="blog-article">
            @if (blog()!.coverImageUrl) {
              <img [src]="blog()!.coverImageUrl" [alt]="blog()!.title" class="cover-img" />
            }
            <div class="article-header">
              <div class="article-meta">
                <span class="badge badge-primary">{{ blog()!.category }}</span>
                <span class="meta-item">⏱ {{ blog()!.readTimeMinutes }} min read</span>
                <span class="meta-item">👁 {{ blog()!.viewCount }} views</span>
                <span class="meta-item">📅 {{ blog()!.publishedAt | date:'MMMM d, yyyy' }}</span>
              </div>
              <h1>{{ blog()!.title }}</h1>
              <p class="summary">{{ blog()!.summary }}</p>
              <div class="tags">
                @for (tag of getTags(blog()!.tags); track tag) {
                  <span class="tag-chip">{{ tag }}</span>
                }
              </div>
            </div>
            <div class="article-content" [innerHTML]="blog()!.content"></div>
          </article>
        </div>
      </section>
    } @else {
      <div class="loading-container"><div class="spinner"></div></div>
    }
  `,
  styles: [`
    .back-link { color: var(--accent-primary); font-size: 0.9rem; display: inline-block; margin-bottom: 2rem; }
    .blog-container { max-width: 800px; }
    .blog-article { }
    .cover-img { width: 100%; height: 400px; object-fit: cover; border-radius: var(--radius-lg); margin-bottom: 2rem; }
    .article-header { margin-bottom: 2rem; }
    .article-meta { display: flex; flex-wrap: wrap; gap: 1rem; align-items: center; margin-bottom: 1rem; }
    .meta-item { color: var(--text-muted); font-size: 0.85rem; }
    h1 { font-size: 2.2rem; margin-bottom: 1rem; line-height: 1.3; }
    .summary { color: var(--text-secondary); font-size: 1.1rem; line-height: 1.7; margin-bottom: 1rem; }
    .article-content { color: var(--text-secondary); line-height: 1.8; font-size: 1rem; }
  `]
})
export class BlogDetailComponent implements OnInit {
  blog = signal<BlogDto | null>(null);

  constructor(private route: ActivatedRoute, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    const slug = this.route.snapshot.paramMap.get('slug')!;
    this.portfolioService.getBlogBySlug(slug).subscribe(r => { if (r.success) this.blog.set(r.data); });
  }

  getTags(tags: string): string[] { return tags ? tags.split(',').map(t => t.trim()).filter(Boolean) : []; }
}
