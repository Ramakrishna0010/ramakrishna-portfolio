import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { AboutDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-about',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-about.component.html',
  styleUrl: './admin-about.component.scss'
})
export class AdminAboutComponent implements OnInit {
  form!: FormGroup;
  about = signal<AboutDto | null>(null);
  loading = signal(false);
  saved = signal(false);
  error = signal('');

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      title: ['', Validators.required],
      subtitle: [''],
      bio: ['', Validators.required],
      shortBio: [''],
      profileImageUrl: [''],
      heroImageUrl: [''],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      location: [''],
      nationality: [''],
      languages: [''],
      yearsOfExperience: [0],
      projectsCompleted: [0],
      happyClients: [0],
      isAvailableForWork: [true],
      availabilityStatus: ['Open to opportunities'],
      metaTitle: [''],
      metaDescription: ['']
    });

    this.portfolioService.getAbout().subscribe(r => {
      if (r.success && r.data) {
        this.about.set(r.data);
        this.form.patchValue(r.data);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.loading.set(true);
    const data = this.form.value;
    const action = this.about()
      ? this.portfolioService.updateAbout(this.about()!.id, data)
      : this.portfolioService.createAbout(data);

    action.subscribe({
      next: r => {
        if (r.success) { this.about.set(r.data); this.saved.set(true); setTimeout(() => this.saved.set(false), 3000); }
        else this.error.set(r.message);
        this.loading.set(false);
      },
      error: () => { this.error.set('Failed to save.'); this.loading.set(false); }
    });
  }
}
