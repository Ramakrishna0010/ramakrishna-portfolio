using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.DTOs.Media;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class MediaController : BaseApiController
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService) => _mediaService = mediaService;

    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> Upload(IFormFile file, [FromForm] UploadMediaDto dto)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { success = false, message = "No file provided." });

        var result = await _mediaService.UploadFileAsync(file, dto, GetCurrentUserId());
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAll([FromQuery] string? category)
    {
        var result = await _mediaService.GetAllFilesAsync(category);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediaService.GetFileByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediaService.DeleteFileAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("categories")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _mediaService.GetCategoriesAsync();
        return StatusCode(result.StatusCode, result);
    }
}
