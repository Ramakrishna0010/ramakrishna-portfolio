using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Projects;

namespace PortfolioApp.Application.Interfaces;

public interface IProjectService
{
    Task<ApiResponse<PagedResponse<ProjectDto>>> GetAllProjectsAsync(QueryParameters parameters);
    Task<ApiResponse<IEnumerable<ProjectDto>>> GetFeaturedProjectsAsync();
    Task<ApiResponse<ProjectDto>> GetProjectByIdAsync(int id);
    Task<ApiResponse<ProjectDto>> GetProjectBySlugAsync(string slug);
    Task<ApiResponse<ProjectDto>> CreateProjectAsync(CreateProjectDto dto);
    Task<ApiResponse<ProjectDto>> UpdateProjectAsync(int id, UpdateProjectDto dto);
    Task<ApiResponse<bool>> DeleteProjectAsync(int id);
    Task<ApiResponse<bool>> IncrementViewCountAsync(int id);
    Task<ApiResponse<bool>> IncrementLikeCountAsync(int id);
    Task<ApiResponse<ProjectImageDto>> AddProjectImageAsync(int projectId, ProjectImageDto imageDto);
    Task<ApiResponse<bool>> DeleteProjectImageAsync(int imageId);
}
