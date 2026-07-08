using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PortfolioApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    protected int GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claim, out var id) ? id : 0;
    }

    protected string GetCurrentUserEmail()
        => User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    protected string GetCurrentUserRole()
        => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
}
