namespace PortfolioApp.Application.DTOs.Experience;

public class ExperienceDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyLogoUrl { get; set; } = string.Empty;
    public string CompanyWebsite { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsRemote { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentJob { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Responsibilities { get; set; } = string.Empty;
    public string TechnologiesUsed { get; set; } = string.Empty;
    public string Achievements { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public string Duration => IsCurrentJob
        ? $"{StartDate:MMM yyyy} - Present"
        : $"{StartDate:MMM yyyy} - {EndDate:MMM yyyy}";
}

public class CreateExperienceDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyLogoUrl { get; set; } = string.Empty;
    public string CompanyWebsite { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsRemote { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentJob { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Responsibilities { get; set; } = string.Empty;
    public string TechnologiesUsed { get; set; } = string.Empty;
    public string Achievements { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
}

public class UpdateExperienceDto : CreateExperienceDto
{
    public int Id { get; set; }
}
