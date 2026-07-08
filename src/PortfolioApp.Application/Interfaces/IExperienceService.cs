using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Experience;

namespace PortfolioApp.Application.Interfaces;

public interface IExperienceService
{
    Task<ApiResponse<IEnumerable<ExperienceDto>>> GetAllExperiencesAsync(QueryParameters parameters);
    Task<ApiResponse<ExperienceDto>> GetExperienceByIdAsync(int id);
    Task<ApiResponse<ExperienceDto>> CreateExperienceAsync(CreateExperienceDto dto);
    Task<ApiResponse<ExperienceDto>> UpdateExperienceAsync(int id, UpdateExperienceDto dto);
    Task<ApiResponse<bool>> DeleteExperienceAsync(int id);
}
