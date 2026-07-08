using PortfolioApp.Domain.Common;
using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public int Percentage { get; set; }
    public string IconClass { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; } = false;
    public string Description { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
}
