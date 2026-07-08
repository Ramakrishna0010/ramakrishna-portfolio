import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { ServiceDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-services',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header-row">
        <div><h2>Services</h2><p>Manage offered services</p></div>
        <button class="btn-primary" (click)="openCreate()">+ Add Service</button>
      </div>
      @if (showForm()) {
        <div class="form-panel glass-card">
          <h3>{{ editingId() ? 'Edit' : 'Add' }} Service</h3>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="form-row-2">
              <div class="form-group"><label>Title *</label><input type="text" formControlName="title" /></div>
              <div class="form-group"><label>Service Type</label>
                <select formControlName="serviceType">
                  @for (t of serviceTypes; track t.value) { <option [value]="t.value">{{ t.label }}</option> }
                </select>
              </div>
            </div>
            <div class="form-group"><label>Short Description</label><input type="text" formControlName="shortDescription" /></div>
            <div class="form-group"><label>Full Description</label><textarea formControlName="description" rows="4"></textarea></div>
            <div class="form-row-2">
              <div class="form-group"><label>Icon Class</label><input type="text" formControlName="iconClass" /></div>
              <div class="form-group"><label>Color (hex)</label><input type="text" formControlName="color" placeholder="#667eea" /></div>
            </div>
            <div class="form-group"><label>Features (comma separated)</label><input type="text" formControlName="features" /></div>
            <div class="form-group"><label>Technologies Used</label><input type="text" formControlName="technologiesUsed" /></div>
            <div class="form-row-3">
              <div class="form-group"><label>Starting Price</label><input type="number" formControlName="startingPrice" /></div>
              <div class="form-group"><label>Pricing Unit</label><input type="text" formControlName="pricingUnit" placeholder="project" /></div>
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
          <thead><tr><th>Title</th><th>Type</th><th>Price</th><th>Featured</th><th>Actions</th></tr></thead>
          <tbody>
            @for (s of services(); track s.id) {
              <tr>
                <td>{{ s.title }}</td>
                <td>{{ s.serviceTypeName }}</td>
                <td>{{ s.startingPrice ? '$' + s.startingPrice : '—' }}</td>
                <td>{{ s.isFeatured ? '⭐' : '—' }}</td>
                <td><div class="action-btns">
                  <button class="btn-ghost" (click)="openEdit(s)">✏️ Edit</button>
                  <button class="btn-ghost danger" (click)="delete(s.id)">🗑️ Delete</button>
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
export class AdminServicesComponent implements OnInit {
  services = signal<ServiceDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);

  serviceTypes = [
    { value: 1, label: 'Web Development' }, { value: 2, label: 'Mobile Development' },
    { value: 3, label: 'Cloud Solutions' }, { value: 4, label: 'Consulting' },
    { value: 5, label: 'UI/UX Design' }, { value: 6, label: 'Other' }
  ];

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      title: ['', Validators.required], description: [''], shortDescription: [''],
      iconClass: [''], iconUrl: [''], serviceType: [1], features: [''],
      technologiesUsed: [''], startingPrice: [null], pricingUnit: ['project'],
      displayOrder: [0], isFeatured: [false], color: ['']
    });
    this.load();
  }

  load(): void { this.portfolioService.getServices().subscribe(r => { if (r.success) this.services.set(r.data); }); }
  openCreate(): void { this.editingId.set(null); this.form.reset({ serviceType: 1, displayOrder: 0, pricingUnit: 'project' }); this.showForm.set(true); }
  openEdit(s: ServiceDto): void { this.editingId.set(s.id); this.form.patchValue(s); this.showForm.set(true); }
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId()
      ? this.portfolioService.updateService(this.editingId()!, this.form.value)
      : this.portfolioService.createService(this.form.value);
    action.subscribe({ next: r => { if (r.success) { this.load(); this.showForm.set(false); } this.loading.set(false); }, error: () => this.loading.set(false) });
  }
  delete(id: number): void { if (!confirm('Delete?')) return; this.portfolioService.deleteService(id).subscribe(r => { if (r.success) this.load(); }); }
}
