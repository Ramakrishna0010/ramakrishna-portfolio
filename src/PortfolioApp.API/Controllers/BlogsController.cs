using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Blogs;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class BlogsController : BaseApiController
{
    private readonly IBlogService _blogService;

    public BlogsController(IBlogService blogService) => _blogService = blogService;

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _blogService.GetAllBlogsAsync(parameters);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("published")]
    public async Task<IActionResult> GetPublished([FromQuery] QueryParameters parameters)
    {
        var result = await _blogService.GetPublishedBlogsAsync(parameters);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _blogService.GetBlogByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await _blogService.GetBlogBySlugAsync(slug);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateBlogDto dto)
    {
        var result = await _blogService.CreateBlogAsync(dto, GetCurrentUserId());
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBlogDto dto)
    {
        var result = await _blogService.UpdateBlogAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _blogService.DeleteBlogAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{id:int}/publish")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Publish(int id)
    {
        var result = await _blogService.PublishBlogAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{id:int}/view")]
    public async Task<IActionResult> IncrementView(int id)
    {
        var result = await _blogService.IncrementViewCountAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("tags")]
    public async Task<IActionResult> GetTags()
    {
        var result = await _blogService.GetAllTagsAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _blogService.GetAllCategoriesAsync();
        return StatusCode(result.StatusCode, result);
    }
}
