using FluentValidation;
using FluentValidation.AspNetCore;
using PortfolioApp.Application.Interfaces;
using PortfolioApp.Application.Mappings;
using PortfolioApp.Application.Services;
using PortfolioApp.Application.Validators;

namespace PortfolioApp.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAboutService, AboutService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IResumeService, ResumeService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ITestimonialService, TestimonialService>();
        services.AddScoped<ISocialLinkService, SocialLinkService>();
        services.AddScoped<IHomeSectionService, HomeSectionService>();

        return services;
    }
}
