using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Skills;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Interfaces;

namespace PortfolioApp.Application.Services;

public class SkillService : ISkillService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SkillService> _logger;

    public SkillService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SkillService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<SkillDto>>> GetAllSkillsAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Skills.Query()
            .Where(s => !s.IsDeleted);

        if (parameters.IsActive.HasValue)
            query = query.Where(s => s.IsActive == parameters.IsActive.Value);

        if (parameters.IsFeatured.HasValue)
            query = query.Where(s => s.IsFeatured == parameters.IsFeatured.Value);

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(s => s.Name.Contains(parameters.SearchTerm));

        query = query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Name);

        var skills = query.ToList();
        return ApiResponse<IEnumerable<SkillDto>>.SuccessResponse(_mapper.Map<IEnumerable<SkillDto>>(skills));
    }

    public async Task<ApiResponse<IEnumerable<SkillGroupDto>>> GetSkillsGroupedByCategoryAsync()
    {
        var skills = await _unitOfWork.Skills.FindAsync(s => s.IsActive && !s.IsDeleted);
        var grouped = skills
            .OrderBy(s => s.DisplayOrder)
            .GroupBy(s => s.Category)
            .Select(g => new SkillGroupDto
            {
                Category = g.Key.ToString(),
                Skills = _mapper.Map<List<SkillDto>>(g.OrderBy(s => s.DisplayOrder).ToList())
            });
        return ApiResponse<IEnumerable<SkillGroupDto>>.SuccessResponse(grouped);
    }

    public async Task<ApiResponse<SkillDto>> GetSkillByIdAsync(int id)
    {
        var skill = await _unitOfWork.Skills.GetByIdAsync(id);
        if (skill == null || skill.IsDeleted)
            return ApiResponse<SkillDto>.FailureResponse("Skill not found.", 404);
        return ApiResponse<SkillDto>.SuccessResponse(_mapper.Map<SkillDto>(skill));
    }

    public async Task<ApiResponse<SkillDto>> CreateSkillAsync(CreateSkillDto dto)
    {
        try
        {
            var skill = _mapper.Map<Skill>(dto);
            await _unitOfWork.Skills.AddAsync(skill);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Skill created: {Name}", dto.Name);
            return ApiResponse<SkillDto>.SuccessResponse(_mapper.Map<SkillDto>(skill), "Skill created.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill");
            return ApiResponse<SkillDto>.FailureResponse("Failed to create skill.", 500);
        }
    }

    public async Task<ApiResponse<SkillDto>> UpdateSkillAsync(int id, UpdateSkillDto dto)
    {
        try
        {
            var skill = await _unitOfWork.Skills.GetByIdAsync(id);
            if (skill == null || skill.IsDeleted)
                return ApiResponse<SkillDto>.FailureResponse("Skill not found.", 404);

            _mapper.Map(dto, skill);
            skill.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Skills.Update(skill);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<SkillDto>.SuccessResponse(_mapper.Map<SkillDto>(skill), "Skill updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating skill {Id}", id);
            return ApiResponse<SkillDto>.FailureResponse("Failed to update skill.", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteSkillAsync(int id)
    {
        var skill = await _unitOfWork.Skills.GetByIdAsync(id);
        if (skill == null || skill.IsDeleted)
            return ApiResponse<bool>.FailureResponse("Skill not found.", 404);

        skill.IsDeleted = true;
        skill.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Skills.Update(skill);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Skill deleted.");
    }

    public async Task<ApiResponse<bool>> ToggleSkillStatusAsync(int id)
    {
        var skill = await _unitOfWork.Skills.GetByIdAsync(id);
        if (skill == null || skill.IsDeleted)
            return ApiResponse<bool>.FailureResponse("Skill not found.", 404);

        skill.IsActive = !skill.IsActive;
        skill.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Skills.Update(skill);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, $"Skill {(skill.IsActive ? "activated" : "deactivated")}.");
    }
}
