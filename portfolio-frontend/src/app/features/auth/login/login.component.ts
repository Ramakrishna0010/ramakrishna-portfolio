import { Component, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="login-page">
      <div class="login-card glass-card">
        <div class="login-header">
          <span class="brand-icon">⚡</span>
          <h2>Admin Login</h2>
          <p>Sign in to manage your portfolio</p>
        </div>
        <form [formGroup]="form" (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label>Email</label>
            <input type="email" formControlName="email" placeholder="admin@portfolio.com" />
            @if (hasError('email', 'required')) { <span class="form-error">Email required</span> }
            @if (hasError('email', 'email')) { <span class="form-error">Valid email required</span> }
          </div>
          <div class="form-group">
            <label>Password</label>
            <div class="password-wrapper">
              <input [type]="showPassword() ? 'text' : 'password'" formControlName="password" placeholder="••••••••" />
              <button type="button" class="toggle-pw" (click)="togglePassword()">
                {{ showPassword() ? '🙈' : '👁️' }}
              </button>
            </div>
            @if (hasError('password', 'required')) { <span class="form-error">Password required</span> }
          </div>
          @if (error()) { <div class="alert-error">{{ error() }}</div> }
          <button type="submit" class="btn-primary login-btn" [disabled]="loading()">
            {{ loading() ? 'Signing in...' : '🔐 Sign In' }}
          </button>
        </form>
      </div>
    </div>
  `,
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  loading = signal(false);
  error = signal('');
  showPassword = signal(false);

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.loading.set(true);
    this.error.set('');
    this.auth.login(this.form.value).subscribe({
      next: r => {
        if (r.success) this.router.navigate(['/admin/dashboard']);
        else this.error.set(r.message || 'Login failed');
        this.loading.set(false);
      },
      error: () => { this.error.set('Invalid credentials. Please try again.'); this.loading.set(false); }
    });
  }

  togglePassword(): void { this.showPassword.set(!this.showPassword()); }

  hasError(field: string, error: string): boolean {
    const ctrl = this.form.get(field);
    return !!(ctrl?.hasError(error) && ctrl?.touched);
  }
}
