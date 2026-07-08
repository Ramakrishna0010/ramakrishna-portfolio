using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Contact;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class ContactController : BaseApiController
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService) => _contactService = contactService;

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] CreateContactMessageDto dto)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var ua = Request.Headers.UserAgent.ToString();
        var result = await _contactService.SendMessageAsync(dto, ip, ua);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _contactService.GetAllMessagesAsync(parameters);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _contactService.GetMessageByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("status")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateContactStatusDto dto)
    {
        var result = await _contactService.UpdateMessageStatusAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _contactService.DeleteMessageAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("unread-count")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var result = await _contactService.GetUnreadCountAsync();
        return StatusCode(result.StatusCode, result);
    }
}
