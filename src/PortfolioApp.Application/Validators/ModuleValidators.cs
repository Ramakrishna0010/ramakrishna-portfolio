using FluentValidation;
using PortfolioApp.Application.DTOs.Skills;
using PortfolioApp.Application.DTOs.Experience;
using PortfolioApp.Application.DTOs.Education;
using PortfolioApp.Application.DTOs.Projects;
using PortfolioApp.Application.DTOs.Blogs;
using PortfolioApp.Application.DTOs.Contact;
using PortfolioApp.Application.DTOs.Certificates;
using PortfolioApp.Application.DTOs.Achievements;
using PortfolioApp.Application.DTOs.Services;
using PortfolioApp.Application.DTOs.Testimonials;

namespace PortfolioApp.Application.Validators;

public class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Percentage).InclusiveBetween(0, 100).WithMessage("Percentage must be between 0 and 100.");
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0);
    }
}

public class CreateExperienceDtoValidator : AbstractValidator<CreateExperienceDto>
{
    public CreateExperienceDtoValidator()
    {
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue && !x.IsCurrentJob)
            .WithMessage("End date must be after start date.");
    }
}

public class CreateEducationDtoValidator : AbstractValidator<CreateEducationDto>
{
    public CreateEducationDtoValidator()
    {
        RuleFor(x => x.InstitutionName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Degree).NotEmpty().MaximumLength(200);
        RuleFor(x => x.FieldOfStudy).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartDate).NotEmpty();
    }
}

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.TechnologyStack).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty();
    }
}

public class CreateBlogDtoValidator : AbstractValidator<CreateBlogDto>
{
    public CreateBlogDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.ReadTimeMinutes).GreaterThan(0);
    }
}

public class CreateContactMessageDtoValidator : AbstractValidator<CreateContactMessageDto>
{
    public CreateContactMessageDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Message).NotEmpty().MinimumLength(10).MaximumLength(5000);
        RuleFor(x => x.Phone)
            .Matches(@"^\+?[\d\s\-\(\)]{7,20}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Invalid phone number format.");
    }
}

public class CreateCertificateDtoValidator : AbstractValidator<CreateCertificateDto>
{
    public CreateCertificateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.IssuedBy).NotEmpty().MaximumLength(200);
        RuleFor(x => x.IssueDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
    }
}

public class CreateAchievementDtoValidator : AbstractValidator<CreateAchievementDto>
{
    public CreateAchievementDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.AchievedDate).NotEmpty();
    }
}

public class CreateServiceDtoValidator : AbstractValidator<CreateServiceDto>
{
    public CreateServiceDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
    }
}

public class CreateTestimonialDtoValidator : AbstractValidator<CreateTestimonialDto>
{
    public CreateTestimonialDtoValidator()
    {
        RuleFor(x => x.ClientName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Content).NotEmpty().MinimumLength(20);
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
    }
}
