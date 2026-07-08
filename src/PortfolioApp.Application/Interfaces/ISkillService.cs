using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Skills;

namespace PortfolioApp.Application.Interfaces;

public interface ISkillService
{
    Task<ApiResponse<IEnumerable<SkillDto>>> GetAllSkillsAsync(QueryParameters parameters);
    Task<ApiResponse<IEnumerable<SkillGroupDto>>> GetSkillsGroupedByCategoryAsync();
    Task<ApiResponse<SkillDto>> GetSkillByIdAsync(int id);
    Task<ApiResponse<SkillDto>> CreateSkillAsync(CreateSkillDto dto);
    Task<ApiResponse<SkillDto>> UpdateSkillAsync(int id, UpdateSkillDto dto);
    Task<ApiResponse<bool>> DeleteSkillAsync(int id);
    Task<ApiResponse<bool>> ToggleSkillStatusAsync(int id);
}
