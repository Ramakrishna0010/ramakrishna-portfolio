using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Education;

namespace PortfolioApp.Application.Interfaces;

public interface IEducationService
{
    Task<ApiResponse<IEnumerable<EducationDto>>> GetAllEducationsAsync(QueryParameters parameters);
    Task<ApiResponse<EducationDto>> GetEducationByIdAsync(int id);
    Task<ApiResponse<EducationDto>> CreateEducationAsync(CreateEducationDto dto);
    Task<ApiResponse<EducationDto>> UpdateEducationAsync(int id, UpdateEducationDto dto);
    Task<ApiResponse<bool>> DeleteEducationAsync(int id);
}
