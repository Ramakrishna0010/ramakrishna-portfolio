using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Skills;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class SkillsController : BaseApiController
{
    private readonly ISkillService _skillService;

    public SkillsController(ISkillService skillService) => _skillService = skillService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _skillService.GetAllSkillsAsync(parameters);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("grouped")]
    public async Task<IActionResult> GetGrouped()
    {
        var result = await _skillService.GetSkillsGroupedByCategoryAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _skillService.GetSkillByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateSkillDto dto)
    {
        var result = await _skillService.CreateSkillAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSkillDto dto)
    {
        var result = await _skillService.UpdateSkillAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _skillService.DeleteSkillAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("{id:int}/toggle")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Toggle(int id)
    {
        var result = await _skillService.ToggleSkillStatusAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
