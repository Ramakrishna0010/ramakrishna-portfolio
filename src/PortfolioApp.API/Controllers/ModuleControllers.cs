using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Certificates;
using PortfolioApp.Application.DTOs.Achievements;
using PortfolioApp.Application.DTOs.Services;
using PortfolioApp.Application.DTOs.Testimonials;
using PortfolioApp.Application.DTOs.SocialLinks;
using PortfolioApp.Application.DTOs.Home;
using PortfolioApp.Application.DTOs.Media;
using PortfolioApp.Application.Interfaces;

namespace PortfolioApp.API.Controllers;

public class CertificatesController : BaseApiController
{
    private readonly ICertificateService _service;
    public CertificatesController(ICertificateService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters p) => StatusCode((await _service.GetAllAsync(p)).StatusCode, await _service.GetAllAsync(p));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) => StatusCode((await _service.GetByIdAsync(id)).StatusCode, await _service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateCertificateDto dto) => StatusCode((await _service.CreateAsync(dto)).StatusCode, await _service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCertificateDto dto) => StatusCode((await _service.UpdateAsync(id, dto)).StatusCode, await _service.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id) => StatusCode((await _service.DeleteAsync(id)).StatusCode, await _service.DeleteAsync(id));
}

public class AchievementsController : BaseApiController
{
    private readonly IAchievementService _service;
    public AchievementsController(IAchievementService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters p) { var r = await _service.GetAllAsync(p); return StatusCode(r.StatusCode, r); }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) { var r = await _service.GetByIdAsync(id); return StatusCode(r.StatusCode, r); }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateAchievementDto dto) { var r = await _service.CreateAsync(dto); return StatusCode(r.StatusCode, r); }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAchievementDto dto) { var r = await _service.UpdateAsync(id, dto); return StatusCode(r.StatusCode, r); }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id) { var r = await _service.DeleteAsync(id); return StatusCode(r.StatusCode, r); }
}

public class ServicesController : BaseApiController
{
    private readonly IServiceService _service;
    public ServicesController(IServiceService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters p) { var r = await _service.GetAllAsync(p); return StatusCode(r.StatusCode, r); }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) { var r = await _service.GetByIdAsync(id); return StatusCode(r.StatusCode, r); }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateServiceDto dto) { var r = await _service.CreateAsync(dto); return StatusCode(r.StatusCode, r); }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceDto dto) { var r = await _service.UpdateAsync(id, dto); return StatusCode(r.StatusCode, r); }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id) { var r = await _service.DeleteAsync(id); return StatusCode(r.StatusCode, r); }
}

public class TestimonialsController : BaseApiController
{
    private readonly ITestimonialService _service;
    public TestimonialsController(ITestimonialService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters p) { var r = await _service.GetAllAsync(p); return StatusCode(r.StatusCode, r); }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) { var r = await _service.GetByIdAsync(id); return StatusCode(r.StatusCode, r); }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateTestimonialDto dto) { var r = await _service.CreateAsync(dto); return StatusCode(r.StatusCode, r); }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTestimonialDto dto) { var r = await _service.UpdateAsync(id, dto); return StatusCode(r.StatusCode, r); }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id) { var r = await _service.DeleteAsync(id); return StatusCode(r.StatusCode, r); }

    [HttpPatch("{id:int}/approve")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Approve(int id) { var r = await _service.ApproveAsync(id); return StatusCode(r.StatusCode, r); }
}

public class SocialLinksController : BaseApiController
{
    private readonly ISocialLinkService _service;
    public SocialLinksController(ISocialLinkService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() { var r = await _service.GetAllAsync(); return StatusCode(r.StatusCode, r); }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) { var r = await _service.GetByIdAsync(id); return StatusCode(r.StatusCode, r); }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateSocialLinkDto dto) { var r = await _service.CreateAsync(dto); return StatusCode(r.StatusCode, r); }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSocialLinkDto dto) { var r = await _service.UpdateAsync(id, dto); return StatusCode(r.StatusCode, r); }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id) { var r = await _service.DeleteAsync(id); return StatusCode(r.StatusCode, r); }
}

public class HomeSectionController : BaseApiController
{
    private readonly IHomeSectionService _service;
    public HomeSectionController(IHomeSectionService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get() { var r = await _service.GetHomeSectionAsync(); return StatusCode(r.StatusCode, r); }

    [HttpPut]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update([FromBody] UpdateHomeSectionDto dto) { var r = await _service.UpdateHomeSectionAsync(dto); return StatusCode(r.StatusCode, r); }
}
