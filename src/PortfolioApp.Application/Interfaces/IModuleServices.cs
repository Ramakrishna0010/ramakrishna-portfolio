using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Certificates;
using PortfolioApp.Application.DTOs.Achievements;
using PortfolioApp.Application.DTOs.Services;
using PortfolioApp.Application.DTOs.Testimonials;
using PortfolioApp.Application.DTOs.SocialLinks;
using PortfolioApp.Application.DTOs.Home;

namespace PortfolioApp.Application.Interfaces;

public interface ICertificateService
{
    Task<ApiResponse<IEnumerable<CertificateDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<CertificateDto>> GetByIdAsync(int id);
    Task<ApiResponse<CertificateDto>> CreateAsync(CreateCertificateDto dto);
    Task<ApiResponse<CertificateDto>> UpdateAsync(int id, UpdateCertificateDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}

public interface IAchievementService
{
    Task<ApiResponse<IEnumerable<AchievementDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<AchievementDto>> GetByIdAsync(int id);
    Task<ApiResponse<AchievementDto>> CreateAsync(CreateAchievementDto dto);
    Task<ApiResponse<AchievementDto>> UpdateAsync(int id, UpdateAchievementDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}

public interface IServiceService
{
    Task<ApiResponse<IEnumerable<ServiceDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<ServiceDto>> GetByIdAsync(int id);
    Task<ApiResponse<ServiceDto>> CreateAsync(CreateServiceDto dto);
    Task<ApiResponse<ServiceDto>> UpdateAsync(int id, UpdateServiceDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}

public interface ITestimonialService
{
    Task<ApiResponse<IEnumerable<TestimonialDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<TestimonialDto>> GetByIdAsync(int id);
    Task<ApiResponse<TestimonialDto>> CreateAsync(CreateTestimonialDto dto);
    Task<ApiResponse<TestimonialDto>> UpdateAsync(int id, UpdateTestimonialDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
    Task<ApiResponse<bool>> ApproveAsync(int id);
}

public interface ISocialLinkService
{
    Task<ApiResponse<IEnumerable<SocialLinkDto>>> GetAllAsync();
    Task<ApiResponse<SocialLinkDto>> GetByIdAsync(int id);
    Task<ApiResponse<SocialLinkDto>> CreateAsync(CreateSocialLinkDto dto);
    Task<ApiResponse<SocialLinkDto>> UpdateAsync(int id, UpdateSocialLinkDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}

public interface IHomeSectionService
{
    Task<ApiResponse<HomeSectionDto>> GetHomeSectionAsync();
    Task<ApiResponse<HomeSectionDto>> UpdateHomeSectionAsync(UpdateHomeSectionDto dto);
}
