import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { EducationDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-education',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header-row">
        <div><h2>Education</h2><p>Manage academic background</p></div>
        <button class="btn-primary" (click)="openCreate()">+ Add Education</button>
      </div>
      @if (showForm()) {
        <div class="form-panel glass-card">
          <h3>{{ editingId() ? 'Edit' : 'Add' }} Education</h3>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="form-row-2">
              <div class="form-group"><label>Institution *</label><input type="text" formControlName="institutionName" /></div>
              <div class="form-group"><label>Degree *</label><input type="text" formControlName="degree" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Field of Study</label><input type="text" formControlName="fieldOfStudy" /></div>
              <div class="form-group"><label>Grade/GPA</label><input type="text" formControlName="grade" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Start Date *</label><input type="date" formControlName="startDate" /></div>
              <div class="form-group"><label>End Date</label><input type="date" formControlName="endDate" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Location</label><input type="text" formControlName="location" /></div>
              <div class="form-group"><label>Logo URL</label><input type="url" formControlName="institutionLogoUrl" /></div>
            </div>
            <div class="form-group"><label>Description</label><textarea formControlName="description" rows="3"></textarea></div>
            <div class="form-group checkbox-group"><label><input type="checkbox" formControlName="isCurrentlyStudying" /> Currently Studying</label></div>
            <div class="form-actions">
              <button type="submit" class="btn-primary" [disabled]="loading()">{{ loading() ? 'Saving...' : 'Save' }}</button>
              <button type="button" class="btn-ghost" (click)="showForm.set(false)">Cancel</button>
            </div>
          </form>
        </div>
      }
      <div class="table-wrapper glass-card">
        <table class="admin-table">
          <thead><tr><th>Institution</th><th>Degree</th><th>Field</th><th>Period</th><th>Actions</th></tr></thead>
          <tbody>
            @for (edu of educations(); track edu.id) {
              <tr>
                <td>{{ edu.institutionName }}</td>
                <td>{{ edu.degree }}</td>
                <td>{{ edu.fieldOfStudy }}</td>
                <td>{{ edu.startDate | date:'yyyy' }} - {{ edu.isCurrentlyStudying ? 'Present' : (edu.endDate | date:'yyyy') }}</td>
                <td><div class="action-btns">
                  <button class="btn-ghost" (click)="openEdit(edu)">✏️ Edit</button>
                  <button class="btn-ghost danger" (click)="delete(edu.id)">🗑️ Delete</button>
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
export class AdminEducationComponent implements OnInit {
  educations = signal<EducationDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      institutionName: ['', Validators.required], degree: ['', Validators.required],
      fieldOfStudy: [''], grade: [''], description: [''], activities: [''],
      startDate: ['', Validators.required], endDate: [''], isCurrentlyStudying: [false],
      location: [''], institutionLogoUrl: [''], displayOrder: [0], isFeatured: [false]
    });
    this.load();
  }

  load(): void { this.portfolioService.getEducations().subscribe(r => { if (r.success) this.educations.set(r.data); }); }
  openCreate(): void { this.editingId.set(null); this.form.reset({ displayOrder: 0 }); this.showForm.set(true); }
  openEdit(e: EducationDto): void {
    this.editingId.set(e.id);
    this.form.patchValue({ ...e, startDate: e.startDate?.substring(0, 10), endDate: e.endDate?.substring(0, 10) });
    this.showForm.set(true);
  }
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId() ? this.portfolioService.updateEducation(this.editingId()!, this.form.value) : this.portfolioService.createEducation(this.form.value);
    action.subscribe({ next: r => { if (r.success) { this.load(); this.showForm.set(false); } this.loading.set(false); }, error: () => this.loading.set(false) });
  }
  delete(id: number): void { if (!confirm('Delete?')) return; this.portfolioService.deleteEducation(id).subscribe(r => { if (r.success) this.load(); }); }
}
