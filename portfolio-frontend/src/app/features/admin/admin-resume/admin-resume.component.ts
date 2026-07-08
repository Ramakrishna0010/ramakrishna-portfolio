import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { ResumeDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-resume',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header">
        <h2>Resume</h2>
        <p>Upload and manage resume versions</p>
      </div>

      <div class="upload-card glass-card">
        <h3>Upload New Resume</h3>
        <form [formGroup]="form" (ngSubmit)="onUpload()">
          <div class="form-row-2">
            <div class="form-group">
              <label>Version</label>
              <input type="text" formControlName="version" placeholder="v2.0" />
            </div>
            <div class="form-group">
              <label>Description</label>
              <input type="text" formControlName="description" placeholder="Updated skills section" />
            </div>
          </div>
          <div class="form-group">
            <label>Resume File (PDF) *</label>
            <input type="file" accept=".pdf,.doc,.docx" (change)="onFileChange($event)" class="file-input" />
          </div>
          @if (uploadError()) { <div class="alert-error">{{ uploadError() }}</div> }
          @if (uploadSuccess()) { <div class="alert-success">✅ Resume uploaded successfully!</div> }
          <button type="submit" class="btn-primary" [disabled]="!selectedFile || uploading()">
            {{ uploading() ? 'Uploading...' : '📤 Upload Resume' }}
          </button>
        </form>
      </div>

      <div class="resumes-list">
        <h3>Version History</h3>
        <div class="resume-items">
          @for (r of resumes(); track r.id) {
            <div class="resume-item glass-card" [class.current]="r.isCurrentVersion">
              <div class="resume-info">
                <div class="resume-icon">📄</div>
                <div>
                  <h4>{{ r.fileName }}</h4>
                  <div class="resume-meta">
                    <span>Version {{ r.version }}</span>
                    <span>{{ r.fileSizeFormatted }}</span>
                    <span>{{ r.downloadCount }} downloads</span>
                    <span>{{ r.uploadedAt | date:'MMM d, yyyy' }}</span>
                  </div>
                  @if (r.description) { <p>{{ r.description }}</p> }
                </div>
              </div>
              <div class="resume-actions">
                @if (r.isCurrentVersion) {
                  <span class="badge badge-success">Current</span>
                }
                <a [href]="r.fileUrl" target="_blank" class="btn-ghost">⬇️ Download</a>
              </div>
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styleUrl: './admin-resume.component.scss'
})
export class AdminResumeComponent implements OnInit {
  resumes = signal<ResumeDto[]>([]);
  form!: FormGroup;
  selectedFile: File | null = null;
  uploading = signal(false);
  uploadError = signal('');
  uploadSuccess = signal(false);

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({ version: [''], description: [''] });
    this.load();
  }

  load(): void { this.portfolioService.getResumes().subscribe(r => { if (r.success) this.resumes.set(r.data); }); }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0] ?? null;
  }

  onUpload(): void {
    if (!this.selectedFile) return;
    this.uploading.set(true);
    this.uploadError.set('');
    this.uploadSuccess.set(false);
    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('version', this.form.value.version || '');
    formData.append('description', this.form.value.description || '');
    this.portfolioService.uploadResume(formData).subscribe({
      next: r => {
        if (r.success) { this.load(); this.uploadSuccess.set(true); this.form.reset(); this.selectedFile = null; }
        else this.uploadError.set(r.message);
        this.uploading.set(false);
      },
      error: () => { this.uploadError.set('Upload failed.'); this.uploading.set(false); }
    });
  }
}
