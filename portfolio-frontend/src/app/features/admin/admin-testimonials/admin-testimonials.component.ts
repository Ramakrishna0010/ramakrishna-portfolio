import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { TestimonialDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-testimonials',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header-row">
        <div><h2>Testimonials</h2><p>Manage client reviews</p></div>
        <button class="btn-primary" (click)="openCreate()">+ Add Testimonial</button>
      </div>
      @if (showForm()) {
        <div class="form-panel glass-card">
          <h3>{{ editingId() ? 'Edit' : 'Add' }} Testimonial</h3>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="form-row-2">
              <div class="form-group"><label>Client Name *</label><input type="text" formControlName="clientName" /></div>
              <div class="form-group"><label>Client Title</label><input type="text" formControlName="clientTitle" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Company</label><input type="text" formControlName="clientCompany" /></div>
              <div class="form-group"><label>LinkedIn URL</label><input type="url" formControlName="clientLinkedIn" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Client Image URL</label><input type="url" formControlName="clientImageUrl" /></div>
              <div class="form-group"><label>Project Worked On</label><input type="text" formControlName="projectWorkedOn" /></div>
            </div>
            <div class="form-group"><label>Testimonial Content *</label><textarea formControlName="content" rows="4"></textarea></div>
            <div class="form-row-3">
              <div class="form-group"><label>Rating (1-5)</label><input type="number" formControlName="rating" min="1" max="5" /></div>
              <div class="form-group"><label>Date</label><input type="date" formControlName="date" /></div>
              <div class="form-group"><label>Display Order</label><input type="number" formControlName="displayOrder" /></div>
            </div>
            <div class="form-row-2">
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
          <thead><tr><th>Client</th><th>Company</th><th>Rating</th><th>Featured</th><th>Actions</th></tr></thead>
          <tbody>
            @for (t of testimonials(); track t.id) {
              <tr>
                <td>{{ t.clientName }}</td>
                <td>{{ t.clientCompany }}</td>
                <td>{{ '⭐'.repeat(t.rating) }}</td>
                <td>{{ t.isFeatured ? '⭐' : '—' }}</td>
                <td><div class="action-btns">
                  <button class="btn-ghost" (click)="openEdit(t)">✏️ Edit</button>
                  <button class="btn-ghost danger" (click)="delete(t.id)">🗑️ Delete</button>
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
export class AdminTestimonialsComponent implements OnInit {
  testimonials = signal<TestimonialDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      clientName: ['', Validators.required], clientTitle: [''], clientCompany: [''],
      clientImageUrl: [''], clientLinkedIn: [''], content: ['', Validators.required],
      rating: [5, [Validators.min(1), Validators.max(5)]], projectWorkedOn: [''],
      date: [''], displayOrder: [0], isFeatured: [false]
    });
    this.load();
  }

  load(): void { this.portfolioService.getTestimonials().subscribe(r => { if (r.success) this.testimonials.set(r.data); }); }
  openCreate(): void { this.editingId.set(null); this.form.reset({ rating: 5, displayOrder: 0 }); this.showForm.set(true); }
  openEdit(t: TestimonialDto): void {
    this.editingId.set(t.id);
    this.form.patchValue({ ...t, date: t.date?.substring(0, 10) });
    this.showForm.set(true);
  }
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId()
      ? this.portfolioService.updateTestimonial(this.editingId()!, this.form.value)
      : this.portfolioService.createTestimonial(this.form.value);
    action.subscribe({ next: r => { if (r.success) { this.load(); this.showForm.set(false); } this.loading.set(false); }, error: () => this.loading.set(false) });
  }
  delete(id: number): void { if (!confirm('Delete?')) return; this.portfolioService.deleteTestimonial(id).subscribe(r => { if (r.success) this.load(); }); }
}
