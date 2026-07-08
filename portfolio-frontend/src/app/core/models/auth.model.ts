export interface LoginDto {
  email: string;
  password: string;
  rememberMe?: boolean;
}

export interface RegisterDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponseDto {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: UserDto;
}

export interface UserDto {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  role: string;
  profileImageUrl?: string;
  lastLoginAt?: string;
}

export interface RefreshTokenDto {
  accessToken: string;
  refreshToken: string;
}
