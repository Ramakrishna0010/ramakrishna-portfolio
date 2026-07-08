using AutoMapper;
using Microsoft.Extensions.Logging;
using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Contact;
using PortfolioApp.Application.DTOs.Certificates;
using PortfolioApp.Application.DTOs.Achievements;
using PortfolioApp.Application.DTOs.Services;
using PortfolioApp.Application.DTOs.Testimonials;
using PortfolioApp.Application.DTOs.SocialLinks;
using PortfolioApp.Application.DTOs.Home;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Enums;
using PortfolioApp.Domain.Interfaces;

namespace PortfolioApp.Application.Services;

public class ContactService : IContactService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactService> _logger;

    public ContactService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ContactService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResponse<ContactMessageDto>>> GetAllMessagesAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.ContactMessages.Query().Where(c => !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(c => c.Name.Contains(parameters.SearchTerm) ||
                                     c.Email.Contains(parameters.SearchTerm) ||
                                     c.Subject.Contains(parameters.SearchTerm));

        query = query.OrderByDescending(c => c.CreatedAt);
        var totalCount = query.Count();
        var items = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();

        return ApiResponse<PagedResponse<ContactMessageDto>>.SuccessResponse(new PagedResponse<ContactMessageDto>
        {
            Data = _mapper.Map<IEnumerable<ContactMessageDto>>(items),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<ApiResponse<ContactMessageDto>> GetMessageByIdAsync(int id)
    {
        var msg = await _unitOfWork.ContactMessages.GetByIdAsync(id);
        if (msg == null || msg.IsDeleted) return ApiResponse<ContactMessageDto>.FailureResponse("Message not found.", 404);
        return ApiResponse<ContactMessageDto>.SuccessResponse(_mapper.Map<ContactMessageDto>(msg));
    }

    public async Task<ApiResponse<ContactMessageDto>> SendMessageAsync(CreateContactMessageDto dto, string ipAddress, string userAgent)
    {
        try
        {
            var message = _mapper.Map<ContactMessage>(dto);
            message.IpAddress = ipAddress;
            message.UserAgent = userAgent;
            message.Status = ContactStatus.New;
            await _unitOfWork.ContactMessages.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Contact message received from {Email}", dto.Email);
            return ApiResponse<ContactMessageDto>.SuccessResponse(_mapper.Map<ContactMessageDto>(message), "Message sent successfully.", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving contact message");
            return ApiResponse<ContactMessageDto>.FailureResponse("Failed to send message.", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateMessageStatusAsync(UpdateContactStatusDto dto)
    {
        var msg = await _unitOfWork.ContactMessages.GetByIdAsync(dto.Id);
        if (msg == null || msg.IsDeleted) return ApiResponse<bool>.FailureResponse("Message not found.", 404);

        msg.Status = dto.Status;
        msg.AdminNotes = dto.AdminNotes;
        msg.UpdatedAt = DateTime.UtcNow;
        if (dto.Status == ContactStatus.Read && !msg.ReadAt.HasValue) msg.ReadAt = DateTime.UtcNow;
        if (dto.Status == ContactStatus.Replied && !msg.RepliedAt.HasValue) msg.RepliedAt = DateTime.UtcNow;

        _unitOfWork.ContactMessages.Update(msg);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Status updated.");
    }

    public async Task<ApiResponse<bool>> DeleteMessageAsync(int id)
    {
        var msg = await _unitOfWork.ContactMessages.GetByIdAsync(id);
        if (msg == null || msg.IsDeleted) return ApiResponse<bool>.FailureResponse("Message not found.", 404);
        msg.IsDeleted = true;
        msg.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.ContactMessages.Update(msg);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Message deleted.");
    }

    public async Task<ApiResponse<int>> GetUnreadCountAsync()
    {
        var count = await _unitOfWork.ContactMessages.CountAsync(c => c.Status == ContactStatus.New && !c.IsDeleted);
        return ApiResponse<int>.SuccessResponse(count);
    }
}

public class CertificateService : ICertificateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CertificateService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<CertificateDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Certificates.Query().Where(c => !c.IsDeleted);
        if (parameters.IsActive.HasValue) query = query.Where(c => c.IsActive == parameters.IsActive.Value);
        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(c => c.Name.Contains(parameters.SearchTerm) || c.IssuedBy.Contains(parameters.SearchTerm));
        query = query.OrderByDescending(c => c.IssueDate);
        return ApiResponse<IEnumerable<CertificateDto>>.SuccessResponse(_mapper.Map<IEnumerable<CertificateDto>>(query.ToList()));
    }

    public async Task<ApiResponse<CertificateDto>> GetByIdAsync(int id)
    {
        var cert = await _unitOfWork.Certificates.GetByIdAsync(id);
        if (cert == null || cert.IsDeleted) return ApiResponse<CertificateDto>.FailureResponse("Certificate not found.", 404);
        return ApiResponse<CertificateDto>.SuccessResponse(_mapper.Map<CertificateDto>(cert));
    }

    public async Task<ApiResponse<CertificateDto>> CreateAsync(CreateCertificateDto dto)
    {
        var cert = _mapper.Map<Certificate>(dto);
        await _unitOfWork.Certificates.AddAsync(cert);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<CertificateDto>.SuccessResponse(_mapper.Map<CertificateDto>(cert), "Certificate created.", 201);
    }

    public async Task<ApiResponse<CertificateDto>> UpdateAsync(int id, UpdateCertificateDto dto)
    {
        var cert = await _unitOfWork.Certificates.GetByIdAsync(id);
        if (cert == null || cert.IsDeleted) return ApiResponse<CertificateDto>.FailureResponse("Certificate not found.", 404);
        _mapper.Map(dto, cert);
        cert.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Certificates.Update(cert);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<CertificateDto>.SuccessResponse(_mapper.Map<CertificateDto>(cert), "Certificate updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var cert = await _unitOfWork.Certificates.GetByIdAsync(id);
        if (cert == null || cert.IsDeleted) return ApiResponse<bool>.FailureResponse("Certificate not found.", 404);
        cert.IsDeleted = true;
        cert.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Certificates.Update(cert);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Certificate deleted.");
    }
}

public class AchievementService : IAchievementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AchievementService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<AchievementDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Achievements.Query().Where(a => !a.IsDeleted);
        if (parameters.IsActive.HasValue) query = query.Where(a => a.IsActive == parameters.IsActive.Value);
        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(a => a.Title.Contains(parameters.SearchTerm));
        query = query.OrderBy(a => a.DisplayOrder).ThenByDescending(a => a.AchievedDate);
        return ApiResponse<IEnumerable<AchievementDto>>.SuccessResponse(_mapper.Map<IEnumerable<AchievementDto>>(query.ToList()));
    }

    public async Task<ApiResponse<AchievementDto>> GetByIdAsync(int id)
    {
        var ach = await _unitOfWork.Achievements.GetByIdAsync(id);
        if (ach == null || ach.IsDeleted) return ApiResponse<AchievementDto>.FailureResponse("Achievement not found.", 404);
        return ApiResponse<AchievementDto>.SuccessResponse(_mapper.Map<AchievementDto>(ach));
    }

    public async Task<ApiResponse<AchievementDto>> CreateAsync(CreateAchievementDto dto)
    {
        var ach = _mapper.Map<Achievement>(dto);
        await _unitOfWork.Achievements.AddAsync(ach);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<AchievementDto>.SuccessResponse(_mapper.Map<AchievementDto>(ach), "Achievement created.", 201);
    }

    public async Task<ApiResponse<AchievementDto>> UpdateAsync(int id, UpdateAchievementDto dto)
    {
        var ach = await _unitOfWork.Achievements.GetByIdAsync(id);
        if (ach == null || ach.IsDeleted) return ApiResponse<AchievementDto>.FailureResponse("Achievement not found.", 404);
        _mapper.Map(dto, ach);
        ach.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Achievements.Update(ach);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<AchievementDto>.SuccessResponse(_mapper.Map<AchievementDto>(ach), "Achievement updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var ach = await _unitOfWork.Achievements.GetByIdAsync(id);
        if (ach == null || ach.IsDeleted) return ApiResponse<bool>.FailureResponse("Achievement not found.", 404);
        ach.IsDeleted = true;
        ach.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Achievements.Update(ach);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Achievement deleted.");
    }
}

public class ServiceService : IServiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<ServiceDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Services.Query().Where(s => !s.IsDeleted);
        if (parameters.IsActive.HasValue) query = query.Where(s => s.IsActive == parameters.IsActive.Value);
        query = query.OrderBy(s => s.DisplayOrder);
        return ApiResponse<IEnumerable<ServiceDto>>.SuccessResponse(_mapper.Map<IEnumerable<ServiceDto>>(query.ToList()));
    }

    public async Task<ApiResponse<ServiceDto>> GetByIdAsync(int id)
    {
        var svc = await _unitOfWork.Services.GetByIdAsync(id);
        if (svc == null || svc.IsDeleted) return ApiResponse<ServiceDto>.FailureResponse("Service not found.", 404);
        return ApiResponse<ServiceDto>.SuccessResponse(_mapper.Map<ServiceDto>(svc));
    }

    public async Task<ApiResponse<ServiceDto>> CreateAsync(CreateServiceDto dto)
    {
        var svc = _mapper.Map<Domain.Entities.Service>(dto);
        await _unitOfWork.Services.AddAsync(svc);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<ServiceDto>.SuccessResponse(_mapper.Map<ServiceDto>(svc), "Service created.", 201);
    }

    public async Task<ApiResponse<ServiceDto>> UpdateAsync(int id, UpdateServiceDto dto)
    {
        var svc = await _unitOfWork.Services.GetByIdAsync(id);
        if (svc == null || svc.IsDeleted) return ApiResponse<ServiceDto>.FailureResponse("Service not found.", 404);
        _mapper.Map(dto, svc);
        svc.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Services.Update(svc);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<ServiceDto>.SuccessResponse(_mapper.Map<ServiceDto>(svc), "Service updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var svc = await _unitOfWork.Services.GetByIdAsync(id);
        if (svc == null || svc.IsDeleted) return ApiResponse<bool>.FailureResponse("Service not found.", 404);
        svc.IsDeleted = true;
        svc.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Services.Update(svc);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Service deleted.");
    }
}

public class TestimonialService : ITestimonialService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TestimonialService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<TestimonialDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Testimonials.Query().Where(t => !t.IsDeleted);
        if (parameters.IsActive.HasValue) query = query.Where(t => t.IsActive == parameters.IsActive.Value);
        query = query.OrderBy(t => t.DisplayOrder).ThenByDescending(t => t.Date);
        return ApiResponse<IEnumerable<TestimonialDto>>.SuccessResponse(_mapper.Map<IEnumerable<TestimonialDto>>(query.ToList()));
    }

    public async Task<ApiResponse<TestimonialDto>> GetByIdAsync(int id)
    {
        var t = await _unitOfWork.Testimonials.GetByIdAsync(id);
        if (t == null || t.IsDeleted) return ApiResponse<TestimonialDto>.FailureResponse("Testimonial not found.", 404);
        return ApiResponse<TestimonialDto>.SuccessResponse(_mapper.Map<TestimonialDto>(t));
    }

    public async Task<ApiResponse<TestimonialDto>> CreateAsync(CreateTestimonialDto dto)
    {
        var t = _mapper.Map<Testimonial>(dto);
        await _unitOfWork.Testimonials.AddAsync(t);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<TestimonialDto>.SuccessResponse(_mapper.Map<TestimonialDto>(t), "Testimonial created.", 201);
    }

    public async Task<ApiResponse<TestimonialDto>> UpdateAsync(int id, UpdateTestimonialDto dto)
    {
        var t = await _unitOfWork.Testimonials.GetByIdAsync(id);
        if (t == null || t.IsDeleted) return ApiResponse<TestimonialDto>.FailureResponse("Testimonial not found.", 404);
        _mapper.Map(dto, t);
        t.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Testimonials.Update(t);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<TestimonialDto>.SuccessResponse(_mapper.Map<TestimonialDto>(t), "Testimonial updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var t = await _unitOfWork.Testimonials.GetByIdAsync(id);
        if (t == null || t.IsDeleted) return ApiResponse<bool>.FailureResponse("Testimonial not found.", 404);
        t.IsDeleted = true;
        t.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Testimonials.Update(t);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Testimonial deleted.");
    }

    public async Task<ApiResponse<bool>> ApproveAsync(int id)
    {
        var t = await _unitOfWork.Testimonials.GetByIdAsync(id);
        if (t == null || t.IsDeleted) return ApiResponse<bool>.FailureResponse("Testimonial not found.", 404);
        t.IsApproved = true;
        t.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Testimonials.Update(t);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Testimonial approved.");
    }
}

public class SocialLinkService : ISocialLinkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SocialLinkService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SocialLinkDto>>> GetAllAsync()
    {
        var links = await _unitOfWork.SocialLinks.FindAsync(s => s.IsActive && !s.IsDeleted);
        return ApiResponse<IEnumerable<SocialLinkDto>>.SuccessResponse(
            _mapper.Map<IEnumerable<SocialLinkDto>>(links.OrderBy(s => s.DisplayOrder)));
    }

    public async Task<ApiResponse<SocialLinkDto>> GetByIdAsync(int id)
    {
        var link = await _unitOfWork.SocialLinks.GetByIdAsync(id);
        if (link == null || link.IsDeleted) return ApiResponse<SocialLinkDto>.FailureResponse("Social link not found.", 404);
        return ApiResponse<SocialLinkDto>.SuccessResponse(_mapper.Map<SocialLinkDto>(link));
    }

    public async Task<ApiResponse<SocialLinkDto>> CreateAsync(CreateSocialLinkDto dto)
    {
        var link = _mapper.Map<SocialLink>(dto);
        await _unitOfWork.SocialLinks.AddAsync(link);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<SocialLinkDto>.SuccessResponse(_mapper.Map<SocialLinkDto>(link), "Social link created.", 201);
    }

    public async Task<ApiResponse<SocialLinkDto>> UpdateAsync(int id, UpdateSocialLinkDto dto)
    {
        var link = await _unitOfWork.SocialLinks.GetByIdAsync(id);
        if (link == null || link.IsDeleted) return ApiResponse<SocialLinkDto>.FailureResponse("Social link not found.", 404);
        _mapper.Map(dto, link);
        link.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.SocialLinks.Update(link);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<SocialLinkDto>.SuccessResponse(_mapper.Map<SocialLinkDto>(link), "Social link updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var link = await _unitOfWork.SocialLinks.GetByIdAsync(id);
        if (link == null || link.IsDeleted) return ApiResponse<bool>.FailureResponse("Social link not found.", 404);
        link.IsDeleted = true;
        link.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.SocialLinks.Update(link);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Social link deleted.");
    }
}

public class HomeSectionService : IHomeSectionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HomeSectionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<HomeSectionDto>> GetHomeSectionAsync()
    {
        var home = await _unitOfWork.HomeSections.FirstOrDefaultAsync(h => !h.IsDeleted);
        if (home == null) return ApiResponse<HomeSectionDto>.FailureResponse("Home section not found.", 404);
        return ApiResponse<HomeSectionDto>.SuccessResponse(_mapper.Map<HomeSectionDto>(home));
    }

    public async Task<ApiResponse<HomeSectionDto>> UpdateHomeSectionAsync(UpdateHomeSectionDto dto)
    {
        var home = await _unitOfWork.HomeSections.GetByIdAsync(dto.Id);
        if (home == null) return ApiResponse<HomeSectionDto>.FailureResponse("Home section not found.", 404);
        _mapper.Map(dto, home);
        home.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.HomeSections.Update(home);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<HomeSectionDto>.SuccessResponse(_mapper.Map<HomeSectionDto>(home), "Home section updated.");
    }
}
