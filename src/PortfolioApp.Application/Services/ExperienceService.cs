using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Experience;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Interfaces;

namespace PortfolioApp.Application.Services;

public class ExperienceService : IExperienceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ExperienceService> _logger;

    public ExperienceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ExperienceService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<ExperienceDto>>> GetAllExperiencesAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Experiences.Query().Where(e => !e.IsDeleted);

        if (parameters.IsActive.HasValue)
            query = query.Where(e => e.IsActive == parameters.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(e => e.CompanyName.Contains(parameters.SearchTerm) || e.Designation.Contains(parameters.SearchTerm));

        query = query.OrderByDescending(e => e.StartDate);
        return ApiResponse<IEnumerable<ExperienceDto>>.SuccessResponse(_mapper.Map<IEnumerable<ExperienceDto>>(query.ToList()));
    }

    public async Task<ApiResponse<ExperienceDto>> GetExperienceByIdAsync(int id)
    {
        var exp = await _unitOfWork.Experiences.GetByIdAsync(id);
        if (exp == null || exp.IsDeleted)
            return ApiResponse<ExperienceDto>.FailureResponse("Experience not found.", 404);
        return ApiResponse<ExperienceDto>.SuccessResponse(_mapper.Map<ExperienceDto>(exp));
    }

    public async Task<ApiResponse<ExperienceDto>> CreateExperienceAsync(CreateExperienceDto dto)
    {
        try
        {
            var exp = _mapper.Map<Experience>(dto);
            await _unitOfWork.Experiences.AddAsync(exp);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Experience created: {Company}", dto.CompanyName);
            return ApiResponse<ExperienceDto>.SuccessResponse(_mapper.Map<ExperienceDto>(exp), "Experience created.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating experience");
            return ApiResponse<ExperienceDto>.FailureResponse("Failed to create experience.", 500);
        }
    }

    public async Task<ApiResponse<ExperienceDto>> UpdateExperienceAsync(int id, UpdateExperienceDto dto)
    {
        try
        {
            var exp = await _unitOfWork.Experiences.GetByIdAsync(id);
            if (exp == null || exp.IsDeleted)
                return ApiResponse<ExperienceDto>.FailureResponse("Experience not found.", 404);

            _mapper.Map(dto, exp);
            exp.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Experiences.Update(exp);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<ExperienceDto>.SuccessResponse(_mapper.Map<ExperienceDto>(exp), "Experience updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating experience {Id}", id);
            return ApiResponse<ExperienceDto>.FailureResponse("Failed to update experience.", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteExperienceAsync(int id)
    {
        var exp = await _unitOfWork.Experiences.GetByIdAsync(id);
        if (exp == null || exp.IsDeleted)
            return ApiResponse<bool>.FailureResponse("Experience not found.", 404);

        exp.IsDeleted = true;
        exp.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Experiences.Update(exp);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Experience deleted.");
    }
}
