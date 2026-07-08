using AutoMapper;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Application.DTOs.Auth;
using PortfolioApp.Application.DTOs.About;
using PortfolioApp.Application.DTOs.Skills;
using PortfolioApp.Application.DTOs.Experience;
using PortfolioApp.Application.DTOs.Education;
using PortfolioApp.Application.DTOs.Projects;
using PortfolioApp.Application.DTOs.Certificates;
using PortfolioApp.Application.DTOs.Achievements;
using PortfolioApp.Application.DTOs.Services;
using PortfolioApp.Application.DTOs.Testimonials;
using PortfolioApp.Application.DTOs.Blogs;
using PortfolioApp.Application.DTOs.Contact;
using PortfolioApp.Application.DTOs.Resume;
using PortfolioApp.Application.DTOs.SocialLinks;
using PortfolioApp.Application.DTOs.Media;
using PortfolioApp.Application.DTOs.Home;

namespace PortfolioApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Auth
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<RegisterDto, ApplicationUser>()
            .ForMember(d => d.PasswordHash, o => o.Ignore());

        // About
        CreateMap<About, AboutDto>();
        CreateMap<CreateAboutDto, About>();
        CreateMap<UpdateAboutDto, About>();

        // Skills
        CreateMap<Skill, SkillDto>();
        CreateMap<CreateSkillDto, Skill>();
        CreateMap<UpdateSkillDto, Skill>();

        // Experience
        CreateMap<Experience, ExperienceDto>();
        CreateMap<CreateExperienceDto, Experience>();
        CreateMap<UpdateExperienceDto, Experience>();

        // Education
        CreateMap<Education, EducationDto>();
        CreateMap<CreateEducationDto, Education>();
        CreateMap<UpdateEducationDto, Education>();

        // Projects
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectImage, ProjectImageDto>();
        CreateMap<CreateProjectDto, Project>()
            .ForMember(d => d.Slug, o => o.Ignore());
        CreateMap<UpdateProjectDto, Project>()
            .ForMember(d => d.Slug, o => o.Ignore());

        // Certificates
        CreateMap<Certificate, CertificateDto>();
        CreateMap<CreateCertificateDto, Certificate>();
        CreateMap<UpdateCertificateDto, Certificate>();

        // Achievements
        CreateMap<Achievement, AchievementDto>();
        CreateMap<CreateAchievementDto, Achievement>();
        CreateMap<UpdateAchievementDto, Achievement>();

        // Services
        CreateMap<Domain.Entities.Service, ServiceDto>();
        CreateMap<CreateServiceDto, Domain.Entities.Service>();
        CreateMap<UpdateServiceDto, Domain.Entities.Service>();

        // Testimonials
        CreateMap<Testimonial, TestimonialDto>();
        CreateMap<CreateTestimonialDto, Testimonial>();
        CreateMap<UpdateTestimonialDto, Testimonial>();

        // Blogs
        CreateMap<Blog, BlogDto>()
            .ForMember(d => d.AuthorName, o => o.MapFrom(s => $"{s.Author.FirstName} {s.Author.LastName}"));
        CreateMap<Blog, BlogListDto>()
            .ForMember(d => d.AuthorName, o => o.MapFrom(s => $"{s.Author.FirstName} {s.Author.LastName}"));
        CreateMap<CreateBlogDto, Blog>()
            .ForMember(d => d.Slug, o => o.Ignore())
            .ForMember(d => d.AuthorId, o => o.Ignore());
        CreateMap<UpdateBlogDto, Blog>()
            .ForMember(d => d.Slug, o => o.Ignore())
            .ForMember(d => d.AuthorId, o => o.Ignore());

        // Contact
        CreateMap<ContactMessage, ContactMessageDto>();
        CreateMap<CreateContactMessageDto, ContactMessage>();

        // Resume
        CreateMap<Resume, ResumeDto>();

        // Social Links
        CreateMap<SocialLink, SocialLinkDto>();
        CreateMap<CreateSocialLinkDto, SocialLink>();
        CreateMap<UpdateSocialLinkDto, SocialLink>();

        // Media
        CreateMap<MediaFile, MediaFileDto>();

        // Home
        CreateMap<HomeSection, HomeSectionDto>();
        CreateMap<UpdateHomeSectionDto, HomeSection>();
    }
}
