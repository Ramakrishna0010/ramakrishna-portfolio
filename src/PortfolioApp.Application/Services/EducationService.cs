using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Education;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Interfaces;

namespace PortfolioApp.Application.Services;

public class EducationService : IEducationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EducationService> _logger;

    public EducationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EducationService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<EducationDto>>> GetAllEducationsAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Educations.Query().Where(e => !e.IsDeleted);

        if (parameters.IsActive.HasValue)
            query = query.Where(e => e.IsActive == parameters.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(e => e.InstitutionName.Contains(parameters.SearchTerm) || e.Degree.Contains(parameters.SearchTerm));

        query = query.OrderByDescending(e => e.StartDate);
        return ApiResponse<IEnumerable<EducationDto>>.SuccessResponse(_mapper.Map<IEnumerable<EducationDto>>(query.ToList()));
    }

    public async Task<ApiResponse<EducationDto>> GetEducationByIdAsync(int id)
    {
        var edu = await _unitOfWork.Educations.GetByIdAsync(id);
        if (edu == null || edu.IsDeleted)
            return ApiResponse<EducationDto>.FailureResponse("Education not found.", 404);
        return ApiResponse<EducationDto>.SuccessResponse(_mapper.Map<EducationDto>(edu));
    }

    public async Task<ApiResponse<EducationDto>> CreateEducationAsync(CreateEducationDto dto)
    {
        try
        {
            var edu = _mapper.Map<Education>(dto);
            await _unitOfWork.Educations.AddAsync(edu);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<EducationDto>.SuccessResponse(_mapper.Map<EducationDto>(edu), "Education created.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating education");
            return ApiResponse<EducationDto>.FailureResponse("Failed to create education.", 500);
        }
    }

    public async Task<ApiResponse<EducationDto>> UpdateEducationAsync(int id, UpdateEducationDto dto)
    {
        try
        {
            var edu = await _unitOfWork.Educations.GetByIdAsync(id);
            if (edu == null || edu.IsDeleted)
                return ApiResponse<EducationDto>.FailureResponse("Education not found.", 404);

            _mapper.Map(dto, edu);
            edu.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Educations.Update(edu);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<EducationDto>.SuccessResponse(_mapper.Map<EducationDto>(edu), "Education updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating education {Id}", id);
            return ApiResponse<EducationDto>.FailureResponse("Failed to update education.", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteEducationAsync(int id)
    {
        var edu = await _unitOfWork.Educations.GetByIdAsync(id);
        if (edu == null || edu.IsDeleted)
            return ApiResponse<bool>.FailureResponse("Education not found.", 404);

        edu.IsDeleted = true;
        edu.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Educations.Update(edu);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Education deleted.");
    }
}
