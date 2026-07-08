using PortfolioApp.Domain.Common;

namespace PortfolioApp.Domain.Entities;

public class Experience : BaseEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyLogoUrl { get; set; } = string.Empty;
    public string CompanyWebsite { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsRemote { get; set; } = false;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentJob { get; set; } = false;
    public string Description { get; set; } = string.Empty;
    public string Responsibilities { get; set; } = string.Empty;
    public string TechnologiesUsed { get; set; } = string.Empty;
    public string Achievements { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; } = false;
}
