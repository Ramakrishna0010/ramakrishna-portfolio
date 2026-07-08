using PortfolioApp.Domain.Common;

namespace PortfolioApp.Domain.Entities;

public class Achievement : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime AchievedDate { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
    public string AwardUrl { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; } = false;
}
