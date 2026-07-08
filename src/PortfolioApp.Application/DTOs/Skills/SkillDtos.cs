using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Application.DTOs.Skills;

public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public string CategoryName => Category.ToString();
    public int Percentage { get; set; }
    public string IconClass { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public string Description { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSkillDto
{
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public int Percentage { get; set; }
    public string IconClass { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public string Description { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
}

public class UpdateSkillDto : CreateSkillDto
{
    public int Id { get; set; }
}

public class SkillGroupDto
{
    public string Category { get; set; } = string.Empty;
    public List<SkillDto> Skills { get; set; } = new();
}
