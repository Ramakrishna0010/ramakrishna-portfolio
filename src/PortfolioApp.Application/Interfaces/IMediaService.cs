using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Media;
using Microsoft.AspNetCore.Http;

namespace PortfolioApp.Application.Interfaces;

public interface IMediaService
{
    Task<ApiResponse<MediaFileDto>> UploadFileAsync(IFormFile file, UploadMediaDto dto, int uploadedByUserId);
    Task<ApiResponse<IEnumerable<MediaFileDto>>> GetAllFilesAsync(string? category = null);
    Task<ApiResponse<MediaFileDto>> GetFileByIdAsync(int id);
    Task<ApiResponse<bool>> DeleteFileAsync(int id);
    Task<ApiResponse<IEnumerable<string>>> GetCategoriesAsync();
}
