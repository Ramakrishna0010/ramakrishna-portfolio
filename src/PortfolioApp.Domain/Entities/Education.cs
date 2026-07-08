using PortfolioApp.Domain.Common;

namespace PortfolioApp.Domain.Entities;

public class Education : BaseEntity
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
    public bool IsCurrentlyStudying { get; set; } = false;
    public string Location { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; } = false;
}
