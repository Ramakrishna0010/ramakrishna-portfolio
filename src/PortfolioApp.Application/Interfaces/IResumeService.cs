using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Resume;
using Microsoft.AspNetCore.Http;

namespace PortfolioApp.Application.Interfaces;

public interface IResumeService
{
    Task<ApiResponse<ResumeDto>> GetCurrentResumeAsync();
    Task<ApiResponse<IEnumerable<ResumeDto>>> GetAllVersionsAsync();
    Task<ApiResponse<ResumeDto>> UploadResumeAsync(IFormFile file, string version, string description);
    Task<ApiResponse<bool>> SetCurrentVersionAsync(int id);
    Task<ApiResponse<bool>> DeleteResumeAsync(int id);
    Task<ApiResponse<bool>> IncrementDownloadCountAsync(int id);
}
