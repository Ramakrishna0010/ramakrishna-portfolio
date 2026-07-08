using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Blogs;

namespace PortfolioApp.Application.Interfaces;

public interface IBlogService
{
    Task<ApiResponse<PagedResponse<BlogListDto>>> GetAllBlogsAsync(QueryParameters parameters);
    Task<ApiResponse<PagedResponse<BlogListDto>>> GetPublishedBlogsAsync(QueryParameters parameters);
    Task<ApiResponse<BlogDto>> GetBlogByIdAsync(int id);
    Task<ApiResponse<BlogDto>> GetBlogBySlugAsync(string slug);
    Task<ApiResponse<BlogDto>> CreateBlogAsync(CreateBlogDto dto, int authorId);
    Task<ApiResponse<BlogDto>> UpdateBlogAsync(int id, UpdateBlogDto dto);
    Task<ApiResponse<bool>> DeleteBlogAsync(int id);
    Task<ApiResponse<bool>> PublishBlogAsync(int id);
    Task<ApiResponse<bool>> IncrementViewCountAsync(int id);
    Task<ApiResponse<IEnumerable<string>>> GetAllTagsAsync();
    Task<ApiResponse<IEnumerable<string>>> GetAllCategoriesAsync();
}
