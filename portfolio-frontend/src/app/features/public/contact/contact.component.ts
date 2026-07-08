import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss'
})
export class ContactComponent implements OnInit {
  form!: FormGroup;
  loading = signal(false);
  success = signal(false);
  error = signal('');

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      subject: ['', Validators.required],
      message: ['', [Validators.required, Validators.minLength(10)]],
      company: [''],
      budget: [''],
      projectType: ['']
    });
  }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.loading.set(true);
    this.error.set('');
    this.portfolioService.sendMessage(this.form.value).subscribe({
      next: r => {
        if (r.success) { this.success.set(true); this.form.reset(); }
        else this.error.set(r.message);
        this.loading.set(false);
      },
      error: () => { this.error.set('Failed to send message. Please try again.'); this.loading.set(false); }
    });
  }

  hasError(field: string, error: string): boolean {
    const ctrl = this.form.get(field);
    return !!(ctrl?.hasError(error) && ctrl?.touched);
  }
}
