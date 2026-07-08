using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Blogs;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Enums;
using PortfolioApp.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace PortfolioApp.Application.Services;

public class BlogService : IBlogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BlogService> _logger;

    public BlogService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BlogService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResponse<BlogListDto>>> GetAllBlogsAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Blogs.Query()
            .Include(b => b.Author)
            .Where(b => !b.IsDeleted);

        query = ApplyFiltersAndSort(query, parameters);

        var totalCount = query.Count();
        var items = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();

        return ApiResponse<PagedResponse<BlogListDto>>.SuccessResponse(new PagedResponse<BlogListDto>
        {
            Data = _mapper.Map<IEnumerable<BlogListDto>>(items),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<ApiResponse<PagedResponse<BlogListDto>>> GetPublishedBlogsAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Blogs.Query()
            .Include(b => b.Author)
            .Where(b => !b.IsDeleted && b.IsActive && b.Status == BlogStatus.Published);

        query = ApplyFiltersAndSort(query, parameters);

        var totalCount = query.Count();
        var items = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();

        return ApiResponse<PagedResponse<BlogListDto>>.SuccessResponse(new PagedResponse<BlogListDto>
        {
            Data = _mapper.Map<IEnumerable<BlogListDto>>(items),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<ApiResponse<BlogDto>> GetBlogByIdAsync(int id)
    {
        var blog = _unitOfWork.Blogs.Query()
            .Include(b => b.Author)
            .FirstOrDefault(b => b.Id == id && !b.IsDeleted);

        if (blog == null) return ApiResponse<BlogDto>.FailureResponse("Blog not found.", 404);
        return ApiResponse<BlogDto>.SuccessResponse(_mapper.Map<BlogDto>(blog));
    }

    public async Task<ApiResponse<BlogDto>> GetBlogBySlugAsync(string slug)
    {
        var blog = _unitOfWork.Blogs.Query()
            .Include(b => b.Author)
            .FirstOrDefault(b => b.Slug == slug && !b.IsDeleted);

        if (blog == null) return ApiResponse<BlogDto>.FailureResponse("Blog not found.", 404);
        return ApiResponse<BlogDto>.SuccessResponse(_mapper.Map<BlogDto>(blog));
    }

    public async Task<ApiResponse<BlogDto>> CreateBlogAsync(CreateBlogDto dto, int authorId)
    {
        try
        {
            var blog = _mapper.Map<Blog>(dto);
            blog.AuthorId = authorId;
            blog.Slug = await GenerateUniqueSlugAsync(dto.Title);
            if (dto.Status == BlogStatus.Published)
                blog.PublishedAt = DateTime.UtcNow;

            await _unitOfWork.Blogs.AddAsync(blog);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Blog created: {Title}", dto.Title);

            var created = _unitOfWork.Blogs.Query().Include(b => b.Author).FirstOrDefault(b => b.Id == blog.Id);
            return ApiResponse<BlogDto>.SuccessResponse(_mapper.Map<BlogDto>(created), "Blog created.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating blog");
            return ApiResponse<BlogDto>.FailureResponse("Failed to create blog.", 500);
        }
    }

    public async Task<ApiResponse<BlogDto>> UpdateBlogAsync(int id, UpdateBlogDto dto)
    {
        try
        {
            var blog = _unitOfWork.Blogs.Query().Include(b => b.Author).FirstOrDefault(b => b.Id == id && !b.IsDeleted);
            if (blog == null) return ApiResponse<BlogDto>.FailureResponse("Blog not found.", 404);

            _mapper.Map(dto, blog);
            blog.UpdatedAt = DateTime.UtcNow;
            if (dto.Status == BlogStatus.Published && !blog.PublishedAt.HasValue)
                blog.PublishedAt = DateTime.UtcNow;

            _unitOfWork.Blogs.Update(blog);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<BlogDto>.SuccessResponse(_mapper.Map<BlogDto>(blog), "Blog updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating blog {Id}", id);
            return ApiResponse<BlogDto>.FailureResponse("Failed to update blog.", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteBlogAsync(int id)
    {
        var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
        if (blog == null || blog.IsDeleted) return ApiResponse<bool>.FailureResponse("Blog not found.", 404);

        blog.IsDeleted = true;
        blog.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Blogs.Update(blog);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Blog deleted.");
    }

    public async Task<ApiResponse<bool>> PublishBlogAsync(int id)
    {
        var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
        if (blog == null || blog.IsDeleted) return ApiResponse<bool>.FailureResponse("Blog not found.", 404);

        blog.Status = BlogStatus.Published;
        blog.PublishedAt = DateTime.UtcNow;
        blog.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Blogs.Update(blog);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Blog published.");
    }

    public async Task<ApiResponse<bool>> IncrementViewCountAsync(int id)
    {
        var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
        if (blog == null) return ApiResponse<bool>.FailureResponse("Blog not found.", 404);
        blog.ViewCount++;
        _unitOfWork.Blogs.Update(blog);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true);
    }

    public async Task<ApiResponse<IEnumerable<string>>> GetAllTagsAsync()
    {
        var blogs = await _unitOfWork.Blogs.FindAsync(b => !b.IsDeleted && !string.IsNullOrEmpty(b.Tags));
        var tags = blogs.SelectMany(b => b.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries))
                        .Select(t => t.Trim())
                        .Distinct()
                        .OrderBy(t => t);
        return ApiResponse<IEnumerable<string>>.SuccessResponse(tags);
    }

    public async Task<ApiResponse<IEnumerable<string>>> GetAllCategoriesAsync()
    {
        var blogs = await _unitOfWork.Blogs.FindAsync(b => !b.IsDeleted && !string.IsNullOrEmpty(b.Category));
        var categories = blogs.Select(b => b.Category.Trim()).Distinct().OrderBy(c => c);
        return ApiResponse<IEnumerable<string>>.SuccessResponse(categories);
    }

    private IQueryable<Blog> ApplyFiltersAndSort(IQueryable<Blog> query, QueryParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(b => b.Title.Contains(parameters.SearchTerm) ||
                                     b.Summary.Contains(parameters.SearchTerm) ||
                                     b.Tags.Contains(parameters.SearchTerm));

        if (parameters.IsFeatured.HasValue)
            query = query.Where(b => b.IsFeatured == parameters.IsFeatured.Value);

        return query.OrderByDescending(b => b.PublishedAt ?? b.CreatedAt);
    }

    private async Task<string> GenerateUniqueSlugAsync(string title)
    {
        var slug = Regex.Replace(title.ToLower().Trim(), @"[^a-z0-9\s-]", "").Replace(" ", "-");
        slug = Regex.Replace(slug, @"-+", "-").Trim('-');

        var exists = await _unitOfWork.Blogs.ExistsAsync(b => b.Slug == slug);
        if (!exists) return slug;

        var counter = 1;
        while (await _unitOfWork.Blogs.ExistsAsync(b => b.Slug == $"{slug}-{counter}"))
            counter++;

        return $"{slug}-{counter}";
    }
}
