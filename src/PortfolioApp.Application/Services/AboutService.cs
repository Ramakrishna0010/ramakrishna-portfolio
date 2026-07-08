using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.About;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Interfaces;

namespace PortfolioApp.Application.Services;

public class AboutService : IAboutService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AboutService> _logger;

    public AboutService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AboutService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<AboutDto>> GetAboutAsync()
    {
        var about = await _unitOfWork.Abouts.FirstOrDefaultAsync(a => !a.IsDeleted);
        if (about == null) return ApiResponse<AboutDto>.FailureResponse("About section not found.", 404);
        return ApiResponse<AboutDto>.SuccessResponse(_mapper.Map<AboutDto>(about));
    }

    public async Task<ApiResponse<AboutDto>> CreateAboutAsync(CreateAboutDto dto)
    {
        try
        {
            var about = _mapper.Map<About>(dto);
            await _unitOfWork.Abouts.AddAsync(about);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("About section created.");
            return ApiResponse<AboutDto>.SuccessResponse(_mapper.Map<AboutDto>(about), "About section created.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating about section");
            return ApiResponse<AboutDto>.FailureResponse("Failed to create about section.", 500);
        }
    }

    public async Task<ApiResponse<AboutDto>> UpdateAboutAsync(int id, UpdateAboutDto dto)
    {
        try
        {
            var about = await _unitOfWork.Abouts.GetByIdAsync(id);
            if (about == null) return ApiResponse<AboutDto>.FailureResponse("About section not found.", 404);

            _mapper.Map(dto, about);
            about.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Abouts.Update(about);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<AboutDto>.SuccessResponse(_mapper.Map<AboutDto>(about), "About section updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating about section {Id}", id);
            return ApiResponse<AboutDto>.FailureResponse("Failed to update about section.", 500);
        }
    }
}
