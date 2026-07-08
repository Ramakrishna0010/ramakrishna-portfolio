namespace PortfolioApp.Application.DTOs.Education;

public class EducationDto
{
    public int Id { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public string InstitutionLogoUrl { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Activities { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentlyStudying { get; set; }
    public string Location { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
}

public class CreateEducationDto
{
    public string InstitutionName { get; set; } = string.Empty;
    public string InstitutionLogoUrl { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Activities { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentlyStudying { get; set; }
    public string Location { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
}

public class UpdateEducationDto : CreateEducationDto
{
    public int Id { get; set; }
}
