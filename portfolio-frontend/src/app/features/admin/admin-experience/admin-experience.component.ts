import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { ExperienceDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-experience',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header-row">
        <div><h2>Experience</h2><p>Manage work experience</p></div>
        <button class="btn-primary" (click)="openCreate()">+ Add Experience</button>
      </div>
      @if (showForm()) {
        <div class="form-panel glass-card">
          <h3>{{ editingId() ? 'Edit' : 'Add' }} Experience</h3>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="form-row-2">
              <div class="form-group"><label>Company Name *</label><input type="text" formControlName="companyName" /></div>
              <div class="form-group"><label>Designation *</label><input type="text" formControlName="designation" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Employment Type</label><input type="text" formControlName="employmentType" placeholder="Full-time" /></div>
              <div class="form-group"><label>Location</label><input type="text" formControlName="location" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Start Date *</label><input type="date" formControlName="startDate" /></div>
              <div class="form-group"><label>End Date</label><input type="date" formControlName="endDate" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Company Website</label><input type="url" formControlName="companyWebsite" /></div>
              <div class="form-group"><label>Company Logo URL</label><input type="url" formControlName="companyLogoUrl" /></div>
            </div>
            <div class="form-group"><label>Description</label><textarea formControlName="description" rows="3"></textarea></div>
            <div class="form-group"><label>Responsibilities (one per line)</label><textarea formControlName="responsibilities" rows="4"></textarea></div>
            <div class="form-group"><label>Technologies Used (comma separated)</label><input type="text" formControlName="technologiesUsed" /></div>
            <div class="form-row-2">
              <div class="form-group checkbox-group"><label><input type="checkbox" formControlName="isCurrentJob" /> Current Job</label></div>
              <div class="form-group checkbox-group"><label><input type="checkbox" formControlName="isRemote" /> Remote</label></div>
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
          <thead><tr><th>Company</th><th>Designation</th><th>Duration</th><th>Current</th><th>Actions</th></tr></thead>
          <tbody>
            @for (exp of experiences(); track exp.id) {
              <tr>
                <td>{{ exp.companyName }}</td>
                <td>{{ exp.designation }}</td>
                <td>{{ exp.duration }}</td>
                <td>{{ exp.isCurrentJob ? '✅' : '—' }}</td>
                <td><div class="action-btns">
                  <button class="btn-ghost" (click)="openEdit(exp)">✏️ Edit</button>
                  <button class="btn-ghost danger" (click)="delete(exp.id)">🗑️ Delete</button>
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
export class AdminExperienceComponent implements OnInit {
  experiences = signal<ExperienceDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      companyName: ['', Validators.required], designation: ['', Validators.required],
      companyLogoUrl: [''], companyWebsite: [''], employmentType: ['Full-time'],
      location: [''], isRemote: [false], startDate: ['', Validators.required],
      endDate: [''], isCurrentJob: [false], description: [''],
      responsibilities: [''], technologiesUsed: [''], achievements: [''],
      displayOrder: [0], isFeatured: [false]
    });
    this.load();
  }

  load(): void { this.portfolioService.getExperiences().subscribe(r => { if (r.success) this.experiences.set(r.data); }); }
  openCreate(): void { this.editingId.set(null); this.form.reset({ employmentType: 'Full-time', displayOrder: 0 }); this.showForm.set(true); }
  openEdit(e: ExperienceDto): void {
    this.editingId.set(e.id);
    this.form.patchValue({ ...e, startDate: e.startDate?.substring(0, 10), endDate: e.endDate?.substring(0, 10) });
    this.showForm.set(true);
  }
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId() ? this.portfolioService.updateExperience(this.editingId()!, this.form.value) : this.portfolioService.createExperience(this.form.value);
    action.subscribe({ next: r => { if (r.success) { this.load(); this.showForm.set(false); } this.loading.set(false); }, error: () => this.loading.set(false) });
  }
  delete(id: number): void { if (!confirm('Delete?')) return; this.portfolioService.deleteExperience(id).subscribe(r => { if (r.success) this.load(); }); }
}
