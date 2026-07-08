using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Projects;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace PortfolioApp.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProjectService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResponse<ProjectDto>>> GetAllProjectsAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Projects.Query()
            .Include(p => p.Images)
            .Where(p => !p.IsDeleted);

        if (parameters.IsActive.HasValue)
            query = query.Where(p => p.IsActive == parameters.IsActive.Value);

        if (parameters.IsFeatured.HasValue)
            query = query.Where(p => p.IsFeatured == parameters.IsFeatured.Value);

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(p => p.Title.Contains(parameters.SearchTerm) ||
                                     p.ShortDescription.Contains(parameters.SearchTerm) ||
                                     p.TechnologyStack.Contains(parameters.SearchTerm));

        query = parameters.SortBy?.ToLower() switch
        {
            "title" => parameters.SortDirection == "desc" ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
            "date" => parameters.SortDirection == "desc" ? query.OrderByDescending(p => p.StartDate) : query.OrderBy(p => p.StartDate),
            _ => query.OrderBy(p => p.DisplayOrder).ThenByDescending(p => p.CreatedAt)
        };

        var totalCount = query.Count();
        var items = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();

        var paged = new PagedResponse<ProjectDto>
        {
            Data = _mapper.Map<IEnumerable<ProjectDto>>(items),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };

        return ApiResponse<PagedResponse<ProjectDto>>.SuccessResponse(paged);
    }

    public async Task<ApiResponse<IEnumerable<ProjectDto>>> GetFeaturedProjectsAsync()
    {
        var projects = _unitOfWork.Projects.Query()
            .Include(p => p.Images)
            .Where(p => p.IsFeatured && p.IsActive && !p.IsDeleted)
            .OrderBy(p => p.DisplayOrder)
            .ToList();
        return ApiResponse<IEnumerable<ProjectDto>>.SuccessResponse(_mapper.Map<IEnumerable<ProjectDto>>(projects));
    }

    public async Task<ApiResponse<ProjectDto>> GetProjectByIdAsync(int id)
    {
        var project = _unitOfWork.Projects.Query()
            .Include(p => p.Images)
            .FirstOrDefault(p => p.Id == id && !p.IsDeleted);

        if (project == null)
            return ApiResponse<ProjectDto>.FailureResponse("Project not found.", 404);

        return ApiResponse<ProjectDto>.SuccessResponse(_mapper.Map<ProjectDto>(project));
    }

    public async Task<ApiResponse<ProjectDto>> GetProjectBySlugAsync(string slug)
    {
        var project = _unitOfWork.Projects.Query()
            .Include(p => p.Images)
            .FirstOrDefault(p => p.Slug == slug && !p.IsDeleted);

        if (project == null)
            return ApiResponse<ProjectDto>.FailureResponse("Project not found.", 404);

        return ApiResponse<ProjectDto>.SuccessResponse(_mapper.Map<ProjectDto>(project));
    }

    public async Task<ApiResponse<ProjectDto>> CreateProjectAsync(CreateProjectDto dto)
    {
        try
        {
            var project = _mapper.Map<Project>(dto);
            project.Slug = await GenerateUniqueSlugAsync(dto.Title);
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Project created: {Title}", dto.Title);
            return ApiResponse<ProjectDto>.SuccessResponse(_mapper.Map<ProjectDto>(project), "Project created.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return ApiResponse<ProjectDto>.FailureResponse("Failed to create project.", 500);
        }
    }

    public async Task<ApiResponse<ProjectDto>> UpdateProjectAsync(int id, UpdateProjectDto dto)
    {
        try
        {
            var project = _unitOfWork.Projects.Query()
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == id && !p.IsDeleted);

            if (project == null)
                return ApiResponse<ProjectDto>.FailureResponse("Project not found.", 404);

            _mapper.Map(dto, project);
            project.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<ProjectDto>.SuccessResponse(_mapper.Map<ProjectDto>(project), "Project updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {Id}", id);
            return ApiResponse<ProjectDto>.FailureResponse("Failed to update project.", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteProjectAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null || project.IsDeleted)
            return ApiResponse<bool>.FailureResponse("Project not found.", 404);

        project.IsDeleted = true;
        project.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Project deleted.");
    }

    public async Task<ApiResponse<bool>> IncrementViewCountAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null) return ApiResponse<bool>.FailureResponse("Project not found.", 404);
        project.ViewCount++;
        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true);
    }

    public async Task<ApiResponse<bool>> IncrementLikeCountAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null) return ApiResponse<bool>.FailureResponse("Project not found.", 404);
        project.LikeCount++;
        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true);
    }

    public async Task<ApiResponse<ProjectImageDto>> AddProjectImageAsync(int projectId, ProjectImageDto imageDto)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (project == null) return ApiResponse<ProjectImageDto>.FailureResponse("Project not found.", 404);

        var image = new ProjectImage
        {
            ProjectId = projectId,
            ImageUrl = imageDto.ImageUrl,
            Caption = imageDto.Caption,
            DisplayOrder = imageDto.DisplayOrder,
            IsThumbnail = imageDto.IsThumbnail
        };
        await _unitOfWork.ProjectImages.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<ProjectImageDto>.SuccessResponse(_mapper.Map<ProjectImageDto>(image), "Image added.", 201);
    }

    public async Task<ApiResponse<bool>> DeleteProjectImageAsync(int imageId)
    {
        var image = await _unitOfWork.ProjectImages.GetByIdAsync(imageId);
        if (image == null) return ApiResponse<bool>.FailureResponse("Image not found.", 404);
        _unitOfWork.ProjectImages.Remove(image);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Image deleted.");
    }

    private async Task<string> GenerateUniqueSlugAsync(string title)
    {
        var slug = Regex.Replace(title.ToLower().Trim(), @"[^a-z0-9\s-]", "")
                        .Replace(" ", "-");
        slug = Regex.Replace(slug, @"-+", "-").Trim('-');

        var exists = await _unitOfWork.Projects.ExistsAsync(p => p.Slug == slug);
        if (!exists) return slug;

        var counter = 1;
        while (await _unitOfWork.Projects.ExistsAsync(p => p.Slug == $"{slug}-{counter}"))
            counter++;

        return $"{slug}-{counter}";
    }
}
