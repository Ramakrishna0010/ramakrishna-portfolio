using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.About;

namespace PortfolioApp.Application.Interfaces;

public interface IAboutService
{
    Task<ApiResponse<AboutDto>> GetAboutAsync();
    Task<ApiResponse<AboutDto>> CreateAboutAsync(CreateAboutDto dto);
    Task<ApiResponse<AboutDto>> UpdateAboutAsync(int id, UpdateAboutDto dto);
}
