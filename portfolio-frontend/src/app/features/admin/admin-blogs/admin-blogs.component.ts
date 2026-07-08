import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { BlogDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-blogs',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header-row">
        <div><h2>Blog Posts</h2><p>Create and manage blog articles</p></div>
        <button class="btn-primary" (click)="openCreate()">+ New Post</button>
      </div>
      @if (showForm()) {
        <div class="form-panel glass-card">
          <h3>{{ editingId() ? 'Edit' : 'Create' }} Blog Post</h3>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="form-group"><label>Title *</label><input type="text" formControlName="title" /></div>
            <div class="form-group"><label>Summary *</label><textarea formControlName="summary" rows="2"></textarea></div>
            <div class="form-group"><label>Content *</label><textarea formControlName="content" rows="10"></textarea></div>
            <div class="form-row-2">
              <div class="form-group"><label>Category</label><input type="text" formControlName="category" /></div>
              <div class="form-group"><label>Tags (comma separated)</label><input type="text" formControlName="tags" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Thumbnail URL</label><input type="url" formControlName="thumbnailUrl" /></div>
              <div class="form-group"><label>Cover Image URL</label><input type="url" formControlName="coverImageUrl" /></div>
            </div>
            <div class="form-row-3">
              <div class="form-group"><label>Status</label>
                <select formControlName="status">
                  <option [value]="1">Draft</option><option [value]="2">Published</option><option [value]="3">Archived</option>
                </select>
              </div>
              <div class="form-group"><label>Read Time (min)</label><input type="number" formControlName="readTimeMinutes" min="1" /></div>
              <div class="form-group checkbox-group"><label><input type="checkbox" formControlName="isFeatured" /> Featured</label></div>
            </div>
            <div class="form-actions">
              <button type="submit" class="btn-primary" [disabled]="loading()">{{ loading() ? 'Saving...' : 'Save' }}</button>
              <button type="button" class="btn-ghost" (click)="showForm.set(false)">Cancel</button>
            </div>
          </form>
        </div>
      }
      <div class="table-wrapper glass-card">
        <table class="admin-table">
          <thead><tr><th>Title</th><th>Category</th><th>Status</th><th>Views</th><th>Featured</th><th>Actions</th></tr></thead>
          <tbody>
            @for (b of blogs(); track b.id) {
              <tr>
                <td>{{ b.title }}</td>
                <td>{{ b.category }}</td>
                <td><span class="badge" [class.badge-success]="b.status===2" [class.badge-warning]="b.status===1">{{ b.statusName }}</span></td>
                <td>{{ b.viewCount }}</td>
                <td>{{ b.isFeatured ? '⭐' : '—' }}</td>
                <td><div class="action-btns">
                  <button class="btn-ghost" (click)="openEdit(b)">✏️ Edit</button>
                  <button class="btn-ghost danger" (click)="delete(b.id)">🗑️ Delete</button>
                </div></td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    </div>
  `,
  styleUrl: '../admin-skills/admin-skills.component.scss'
})
export class AdminBlogsComponent implements OnInit {
  blogs = signal<BlogDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      title: ['', Validators.required], summary: ['', Validators.required],
      content: ['', Validators.required], thumbnailUrl: [''], coverImageUrl: [''],
      tags: [''], category: [''], status: [1], readTimeMinutes: [5],
      metaTitle: [''], metaDescription: [''], metaKeywords: [''], isFeatured: [false]
    });
    this.load();
  }

  load(): void {
    this.portfolioService.getBlogs({ pageSize: 100 }).subscribe(r => {
      if (r.success) this.blogs.set(r.data.items as any);
    });
  }
  openCreate(): void { this.editingId.set(null); this.form.reset({ status: 1, readTimeMinutes: 5 }); this.showForm.set(true); }
  openEdit(b: BlogDto): void { this.editingId.set(b.id); this.form.patchValue(b); this.showForm.set(true); }
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId()
      ? this.portfolioService.updateBlog(this.editingId()!, this.form.value)
      : this.portfolioService.createBlog(this.form.value);
    action.subscribe({ next: r => { if (r.success) { this.load(); this.showForm.set(false); } this.loading.set(false); }, error: () => this.loading.set(false) });
  }
  delete(id: number): void { if (!confirm('Delete?')) return; this.portfolioService.deleteBlog(id).subscribe(r => { if (r.success) this.load(); }); }
}
