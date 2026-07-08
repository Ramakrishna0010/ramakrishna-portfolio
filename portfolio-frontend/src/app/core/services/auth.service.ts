import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponseDto, LoginDto, UserDto } from '../models/auth.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'portfolio_access_token';
  private readonly REFRESH_KEY = 'portfolio_refresh_token';
  private readonly USER_KEY = 'portfolio_user';

  currentUser = signal<UserDto | null>(this.getStoredUser());
  isAuthenticated = signal<boolean>(!!this.getToken());

  constructor(private http: HttpClient, private router: Router) {}

  login(dto: LoginDto): Observable<ApiResponse<AuthResponseDto>> {
    return this.http.post<ApiResponse<AuthResponseDto>>(`${environment.apiUrl}/auth/login`, dto).pipe(
      tap(res => {
        if (res.success && res.data) {
          this.storeTokens(res.data);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_KEY);
  }

  isAdmin(): boolean {
    const user = this.currentUser();
    return user?.role === 'Admin';
  }

  refreshToken(): Observable<ApiResponse<AuthResponseDto>> {
    const dto = { accessToken: this.getToken() ?? '', refreshToken: this.getRefreshToken() ?? '' };
    return this.http.post<ApiResponse<AuthResponseDto>>(`${environment.apiUrl}/auth/refresh-token`, dto).pipe(
      tap(res => { if (res.success && res.data) this.storeTokens(res.data); }),
      catchError(err => { this.logout(); return throwError(() => err); })
    );
  }

  private storeTokens(data: AuthResponseDto): void {
    localStorage.setItem(this.TOKEN_KEY, data.accessToken);
    localStorage.setItem(this.REFRESH_KEY, data.refreshToken);
    localStorage.setItem(this.USER_KEY, JSON.stringify(data.user));
    this.currentUser.set(data.user);
    this.isAuthenticated.set(true);
  }

  private getStoredUser(): UserDto | null {
    const stored = localStorage.getItem(this.USER_KEY);
    return stored ? JSON.parse(stored) : null;
  }
}
