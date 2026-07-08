import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { AchievementDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-achievements',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header-row">
        <div><h2>Achievements</h2><p>Manage awards and recognition</p></div>
        <button class="btn-primary" (click)="openCreate()">+ Add Achievement</button>
      </div>
      @if (showForm()) {
        <div class="form-panel glass-card">
          <h3>{{ editingId() ? 'Edit' : 'Add' }} Achievement</h3>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="form-row-2">
              <div class="form-group"><label>Title *</label><input type="text" formControlName="title" /></div>
              <div class="form-group"><label>Category</label><input type="text" formControlName="category" /></div>
            </div>
            <div class="form-group"><label>Description</label><textarea formControlName="description" rows="3"></textarea></div>
            <div class="form-row-2">
              <div class="form-group"><label>Issued By</label><input type="text" formControlName="issuedBy" /></div>
              <div class="form-group"><label>Achieved Date *</label><input type="date" formControlName="achievedDate" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Award URL</label><input type="url" formControlName="awardUrl" /></div>
              <div class="form-group"><label>Image URL</label><input type="url" formControlName="imageUrl" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Icon Class</label><input type="text" formControlName="iconClass" /></div>
              <div class="form-group"><label>Display Order</label><input type="number" formControlName="displayOrder" /></div>
            </div>
            <div class="form-group checkbox-group"><label><input type="checkbox" formControlName="isFeatured" /> Featured</label></div>
            <div class="form-actions">
              <button type="submit" class="btn-primary" [disabled]="loading()">{{ loading() ? 'Saving...' : 'Save' }}</button>
              <button type="button" class="btn-ghost" (click)="showForm.set(false)">Cancel</button>
            </div>
          </form>
        </div>
      }
      <div class="table-wrapper glass-card">
        <table class="admin-table">
          <thead><tr><th>Title</th><th>Category</th><th>Issued By</th><th>Date</th><th>Actions</th></tr></thead>
          <tbody>
            @for (a of achievements(); track a.id) {
              <tr>
                <td>{{ a.title }}</td>
                <td>{{ a.category }}</td>
                <td>{{ a.issuedBy }}</td>
                <td>{{ a.achievedDate | date:'MMM yyyy' }}</td>
                <td><div class="action-btns">
                  <button class="btn-ghost" (click)="openEdit(a)">✏️ Edit</button>
                  <button class="btn-ghost danger" (click)="delete(a.id)">🗑️ Delete</button>
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
export class AdminAchievementsComponent implements OnInit {
  achievements = signal<AchievementDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      title: ['', Validators.required], description: [''], iconClass: [''],
      iconUrl: [''], category: [''], achievedDate: ['', Validators.required],
      issuedBy: [''], awardUrl: [''], imageUrl: [''], displayOrder: [0], isFeatured: [false]
    });
    this.load();
  }

  load(): void { this.portfolioService.getAchievements().subscribe(r => { if (r.success) this.achievements.set(r.data); }); }
  openCreate(): void { this.editingId.set(null); this.form.reset({ displayOrder: 0 }); this.showForm.set(true); }
  openEdit(a: AchievementDto): void {
    this.editingId.set(a.id);
    this.form.patchValue({ ...a, achievedDate: a.achievedDate?.substring(0, 10) });
    this.showForm.set(true);
  }
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId()
      ? this.portfolioService.updateAchievement(this.editingId()!, this.form.value)
      : this.portfolioService.createAchievement(this.form.value);
    action.subscribe({ next: r => { if (r.success) { this.load(); this.showForm.set(false); } this.loading.set(false); }, error: () => this.loading.set(false) });
  }
  delete(id: number): void { if (!confirm('Delete?')) return; this.portfolioService.deleteAchievement(id).subscribe(r => { if (r.success) this.load(); }); }
}
