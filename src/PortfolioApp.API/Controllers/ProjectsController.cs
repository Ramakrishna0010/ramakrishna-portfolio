using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Projects;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class ProjectsController : BaseApiController
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService) => _projectService = projectService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _projectService.GetAllProjectsAsync(parameters);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured()
    {
        var result = await _projectService.GetFeaturedProjectsAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _projectService.GetProjectByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await _projectService.GetProjectBySlugAsync(slug);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var result = await _projectService.CreateProjectAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var result = await _projectService.UpdateProjectAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{id:int}/view")]
    public async Task<IActionResult> IncrementView(int id)
    {
        var result = await _projectService.IncrementViewCountAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{id:int}/like")]
    public async Task<IActionResult> IncrementLike(int id)
    {
        var result = await _projectService.IncrementLikeCountAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{id:int}/images")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddImage(int id, [FromBody] ProjectImageDto imageDto)
    {
        var result = await _projectService.AddProjectImageAsync(id, imageDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("images/{imageId:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var result = await _projectService.DeleteProjectImageAsync(imageId);
        return StatusCode(result.StatusCode, result);
    }
}
