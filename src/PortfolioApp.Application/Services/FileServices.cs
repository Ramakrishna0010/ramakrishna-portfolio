using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Resume;
using PortfolioApp.Application.DTOs.Media;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Interfaces;

namespace PortfolioApp.Application.Services;

public class ResumeService : IResumeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ResumeService> _logger;
    private readonly IConfiguration _configuration;

    public ResumeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ResumeService> logger, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<ApiResponse<ResumeDto>> GetCurrentResumeAsync()
    {
        var resume = await _unitOfWork.Resumes.FirstOrDefaultAsync(r => r.IsCurrentVersion && !r.IsDeleted);
        if (resume == null) return ApiResponse<ResumeDto>.FailureResponse("No resume found.", 404);
        return ApiResponse<ResumeDto>.SuccessResponse(_mapper.Map<ResumeDto>(resume));
    }

    public async Task<ApiResponse<IEnumerable<ResumeDto>>> GetAllVersionsAsync()
    {
        var resumes = await _unitOfWork.Resumes.FindAsync(r => !r.IsDeleted);
        return ApiResponse<IEnumerable<ResumeDto>>.SuccessResponse(
            _mapper.Map<IEnumerable<ResumeDto>>(resumes.OrderByDescending(r => r.UploadedAt)));
    }

    public async Task<ApiResponse<ResumeDto>> UploadResumeAsync(IFormFile file, string version, string description)
    {
        try
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"resume_{version}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var resume = new Resume
            {
                FileName = fileName,
                OriginalFileName = file.FileName,
                FileUrl = $"/resumes/{fileName}",
                Version = version,
                Description = description,
                FileSizeBytes = file.Length,
                ContentType = file.ContentType,
                IsCurrentVersion = false,
                UploadedAt = DateTime.UtcNow
            };

            await _unitOfWork.Resumes.AddAsync(resume);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Resume uploaded: {Version}", version);
            return ApiResponse<ResumeDto>.SuccessResponse(_mapper.Map<ResumeDto>(resume), "Resume uploaded.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading resume");
            return ApiResponse<ResumeDto>.FailureResponse("Failed to upload resume.", 500);
        }
    }

    public async Task<ApiResponse<bool>> SetCurrentVersionAsync(int id)
    {
        var resume = await _unitOfWork.Resumes.GetByIdAsync(id);
        if (resume == null || resume.IsDeleted) return ApiResponse<bool>.FailureResponse("Resume not found.", 404);

        var allResumes = await _unitOfWork.Resumes.FindAsync(r => r.IsCurrentVersion);
        foreach (var r in allResumes)
        {
            r.IsCurrentVersion = false;
            _unitOfWork.Resumes.Update(r);
        }

        resume.IsCurrentVersion = true;
        _unitOfWork.Resumes.Update(resume);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Current version set.");
    }

    public async Task<ApiResponse<bool>> DeleteResumeAsync(int id)
    {
        var resume = await _unitOfWork.Resumes.GetByIdAsync(id);
        if (resume == null || resume.IsDeleted) return ApiResponse<bool>.FailureResponse("Resume not found.", 404);
        resume.IsDeleted = true;
        resume.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Resumes.Update(resume);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Resume deleted.");
    }

    public async Task<ApiResponse<bool>> IncrementDownloadCountAsync(int id)
    {
        var resume = await _unitOfWork.Resumes.GetByIdAsync(id);
        if (resume == null) return ApiResponse<bool>.FailureResponse("Resume not found.", 404);
        resume.DownloadCount++;
        _unitOfWork.Resumes.Update(resume);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true);
    }
}

public class MediaService : IMediaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MediaService> _logger;

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg", ".pdf" };
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    public MediaService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MediaService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<MediaFileDto>> UploadFileAsync(IFormFile file, UploadMediaDto dto, int uploadedByUserId)
    {
        try
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                return ApiResponse<MediaFileDto>.FailureResponse($"File type '{ext}' is not allowed.", 400);

            if (file.Length > MaxFileSizeBytes)
                return ApiResponse<MediaFileDto>.FailureResponse("File size exceeds 10 MB limit.", 400);

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", dto.Category);
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var mediaFile = new MediaFile
            {
                FileName = fileName,
                OriginalFileName = file.FileName,
                FileUrl = $"/uploads/{dto.Category}/{fileName}",
                ContentType = file.ContentType,
                FileSizeBytes = file.Length,
                Category = dto.Category,
                AltText = dto.AltText,
                Description = dto.Description,
                StorageProvider = "Local",
                UploadedByUserId = uploadedByUserId
            };

            await _unitOfWork.MediaFiles.AddAsync(mediaFile);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Media file uploaded: {FileName}", fileName);
            return ApiResponse<MediaFileDto>.SuccessResponse(_mapper.Map<MediaFileDto>(mediaFile), "File uploaded.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading media file");
            return ApiResponse<MediaFileDto>.FailureResponse("Failed to upload file.", 500);
        }
    }

    public async Task<ApiResponse<IEnumerable<MediaFileDto>>> GetAllFilesAsync(string? category = null)
    {
        var files = string.IsNullOrEmpty(category)
            ? await _unitOfWork.MediaFiles.FindAsync(f => !f.IsDeleted)
            : await _unitOfWork.MediaFiles.FindAsync(f => !f.IsDeleted && f.Category == category);

        return ApiResponse<IEnumerable<MediaFileDto>>.SuccessResponse(
            _mapper.Map<IEnumerable<MediaFileDto>>(files.OrderByDescending(f => f.CreatedAt)));
    }

    public async Task<ApiResponse<MediaFileDto>> GetFileByIdAsync(int id)
    {
        var file = await _unitOfWork.MediaFiles.GetByIdAsync(id);
        if (file == null || file.IsDeleted) return ApiResponse<MediaFileDto>.FailureResponse("File not found.", 404);
        return ApiResponse<MediaFileDto>.SuccessResponse(_mapper.Map<MediaFileDto>(file));
    }

    public async Task<ApiResponse<bool>> DeleteFileAsync(int id)
    {
        var file = await _unitOfWork.MediaFiles.GetByIdAsync(id);
        if (file == null || file.IsDeleted) return ApiResponse<bool>.FailureResponse("File not found.", 404);

        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileUrl.TrimStart('/'));
        if (File.Exists(physicalPath)) File.Delete(physicalPath);

        file.IsDeleted = true;
        file.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.MediaFiles.Update(file);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "File deleted.");
    }

    public async Task<ApiResponse<IEnumerable<string>>> GetCategoriesAsync()
    {
        var files = await _unitOfWork.MediaFiles.FindAsync(f => !f.IsDeleted);
        var categories = files.Select(f => f.Category).Distinct().OrderBy(c => c);
        return ApiResponse<IEnumerable<string>>.SuccessResponse(categories);
    }
}
