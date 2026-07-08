using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class ResumeController : BaseApiController
{
    private readonly IResumeService _resumeService;

    public ResumeController(IResumeService resumeService) => _resumeService = resumeService;

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent()
    {
        var result = await _resumeService.GetCurrentResumeAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("versions")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllVersions()
    {
        var result = await _resumeService.GetAllVersionsAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Upload(IFormFile file, [FromForm] string version, [FromForm] string description)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { success = false, message = "No file provided." });

        var result = await _resumeService.UploadResumeAsync(file, version, description);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("{id:int}/set-current")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> SetCurrent(int id)
    {
        var result = await _resumeService.SetCurrentVersionAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _resumeService.DeleteResumeAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var resumeResult = await _resumeService.GetCurrentResumeAsync();
        if (!resumeResult.Success || resumeResult.Data == null)
            return NotFound(new { success = false, message = "Resume not found." });

        await _resumeService.IncrementDownloadCountAsync(id);

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
            resumeResult.Data.FileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (!System.IO.File.Exists(filePath))
            return NotFound(new { success = false, message = "File not found on server." });

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(fileBytes, resumeResult.Data.ContentType, resumeResult.Data.FileName);
    }
}
