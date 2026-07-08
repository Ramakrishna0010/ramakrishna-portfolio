using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Experience;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class ExperienceController : BaseApiController
{
    private readonly IExperienceService _experienceService;

    public ExperienceController(IExperienceService experienceService) => _experienceService = experienceService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _experienceService.GetAllExperiencesAsync(parameters);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _experienceService.GetExperienceByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateExperienceDto dto)
    {
        var result = await _experienceService.CreateExperienceAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateExperienceDto dto)
    {
        var result = await _experienceService.UpdateExperienceAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _experienceService.DeleteExperienceAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
