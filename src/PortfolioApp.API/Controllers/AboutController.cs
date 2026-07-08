using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.DTOs.About;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class AboutController : BaseApiController
{
    private readonly IAboutService _aboutService;

    public AboutController(IAboutService aboutService) => _aboutService = aboutService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _aboutService.GetAboutAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateAboutDto dto)
    {
        var result = await _aboutService.CreateAboutAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAboutDto dto)
    {
        var result = await _aboutService.UpdateAboutAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }
}
