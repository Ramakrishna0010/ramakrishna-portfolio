using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Auth;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PortfolioApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        try
        {
            var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
                return ApiResponse<AuthResponseDto>.FailureResponse("Email already registered.", 409);

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Visitor"
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var response = await GenerateAuthResponseAsync(user);
            _logger.LogInformation("New user registered: {Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.SuccessResponse(response, "Registration successful.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for {Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.FailureResponse("Registration failed.", 500);
        }
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        try
        {
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && !u.IsDeleted);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return ApiResponse<AuthResponseDto>.FailureResponse("Invalid email or password.", 401);

            if (!user.IsActive)
                return ApiResponse<AuthResponseDto>.FailureResponse("Account is deactivated.", 403);

            user.LastLoginAt = DateTime.UtcNow;
            var response = await GenerateAuthResponseAsync(user);
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User logged in: {Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.SuccessResponse(response, "Login successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.FailureResponse("Login failed.", 500);
        }
    }

    public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(dto.AccessToken);
            if (principal == null)
                return ApiResponse<AuthResponseDto>.FailureResponse("Invalid access token.", 401);

            var email = principal.FindFirstValue(ClaimTypes.Email);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
                return ApiResponse<AuthResponseDto>.FailureResponse("Invalid or expired refresh token.", 401);

            var response = await GenerateAuthResponseAsync(user);
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<AuthResponseDto>.SuccessResponse(response, "Token refreshed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return ApiResponse<AuthResponseDto>.FailureResponse("Token refresh failed.", 500);
        }
    }

    public async Task<ApiResponse<bool>> RevokeTokenAsync(string email)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return ApiResponse<bool>.FailureResponse("User not found.", 404);

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Token revoked.");
    }

    public async Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return ApiResponse<bool>.FailureResponse("User not found.", 404);

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            return ApiResponse<bool>.FailureResponse("Current password is incorrect.", 400);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Password changed successfully.");
    }

    public async Task<ApiResponse<UserDto>> GetCurrentUserAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return ApiResponse<UserDto>.FailureResponse("User not found.", 404);
        return ApiResponse<UserDto>.SuccessResponse(_mapper.Map<UserDto>(user));
    }

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(ApplicationUser user)
    {
        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"] ?? "60"));

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = _mapper.Map<UserDto>(user)
        };
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"] ?? "60")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;
            return principal;
        }
        catch { return null; }
    }
}
