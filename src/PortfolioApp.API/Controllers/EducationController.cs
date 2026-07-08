using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Education;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class EducationController : BaseApiController
{
    private readonly IEducationService _educationService;

    public EducationController(IEducationService educationService) => _educationService = educationService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _educationService.GetAllEducationsAsync(parameters);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _educationService.GetEducationByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateEducationDto dto)
    {
        var result = await _educationService.CreateEducationAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEducationDto dto)
    {
        var result = await _educationService.UpdateEducationAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _educationService.DeleteEducationAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
