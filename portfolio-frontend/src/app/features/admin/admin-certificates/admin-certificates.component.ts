import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { CertificateDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-certificates',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header-row">
        <div><h2>Certificates</h2><p>Manage certifications</p></div>
        <button class="btn-primary" (click)="openCreate()">+ Add Certificate</button>
      </div>
      @if (showForm()) {
        <div class="form-panel glass-card">
          <h3>{{ editingId() ? 'Edit' : 'Add' }} Certificate</h3>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="form-row-2">
              <div class="form-group"><label>Certificate Name *</label><input type="text" formControlName="name" /></div>
              <div class="form-group"><label>Issued By *</label><input type="text" formControlName="issuedBy" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Issue Date *</label><input type="date" formControlName="issueDate" /></div>
              <div class="form-group"><label>Expiry Date</label><input type="date" formControlName="expiryDate" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Credential ID</label><input type="text" formControlName="credentialId" /></div>
              <div class="form-group"><label>Credential URL</label><input type="url" formControlName="credentialUrl" /></div>
            </div>
            <div class="form-row-2">
              <div class="form-group"><label>Issuer Logo URL</label><input type="url" formControlName="issuedByLogoUrl" /></div>
              <div class="form-group"><label>Certificate Image URL</label><input type="url" formControlName="certificateImageUrl" /></div>
            </div>
            <div class="form-group"><label>Skills (comma separated)</label><input type="text" formControlName="skills" /></div>
            <div class="form-group"><label>Description</label><textarea formControlName="description" rows="2"></textarea></div>
            <div class="form-row-2">
              <div class="form-group checkbox-group"><label><input type="checkbox" formControlName="doesNotExpire" /> Does Not Expire</label></div>
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
          <thead><tr><th>Name</th><th>Issued By</th><th>Issue Date</th><th>Featured</th><th>Actions</th></tr></thead>
          <tbody>
            @for (c of certificates(); track c.id) {
              <tr>
                <td>{{ c.name }}</td>
                <td>{{ c.issuedBy }}</td>
                <td>{{ c.issueDate | date:'MMM yyyy' }}</td>
                <td>{{ c.isFeatured ? '⭐' : '—' }}</td>
                <td><div class="action-btns">
                  <button class="btn-ghost" (click)="openEdit(c)">✏️ Edit</button>
                  <button class="btn-ghost danger" (click)="delete(c.id)">🗑️ Delete</button>
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
export class AdminCertificatesComponent implements OnInit {
  certificates = signal<CertificateDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.required], issuedBy: ['', Validators.required],
      issuedByLogoUrl: [''], issueDate: ['', Validators.required],
      expiryDate: [''], doesNotExpire: [false], credentialId: [''],
      credentialUrl: [''], certificateImageUrl: [''], description: [''],
      skills: [''], displayOrder: [0], isFeatured: [false]
    });
    this.load();
  }

  load(): void { this.portfolioService.getCertificates().subscribe(r => { if (r.success) this.certificates.set(r.data); }); }
  openCreate(): void { this.editingId.set(null); this.form.reset({ doesNotExpire: false, displayOrder: 0 }); this.showForm.set(true); }
  openEdit(c: CertificateDto): void {
    this.editingId.set(c.id);
    this.form.patchValue({ ...c, issueDate: c.issueDate?.substring(0, 10), expiryDate: c.expiryDate?.substring(0, 10) });
    this.showForm.set(true);
  }
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId()
      ? this.portfolioService.updateCertificate(this.editingId()!, this.form.value)
      : this.portfolioService.createCertificate(this.form.value);
    action.subscribe({ next: r => { if (r.success) { this.load(); this.showForm.set(false); } this.loading.set(false); }, error: () => this.loading.set(false) });
  }
  delete(id: number): void { if (!confirm('Delete?')) return; this.portfolioService.deleteCertificate(id).subscribe(r => { if (r.success) this.load(); }); }
}
