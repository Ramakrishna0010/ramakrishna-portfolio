using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Auth;

namespace PortfolioApp.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
    Task<ApiResponse<bool>> RevokeTokenAsync(string email);
    Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    Task<ApiResponse<UserDto>> GetCurrentUserAsync(int userId);
}
