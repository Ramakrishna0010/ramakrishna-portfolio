using PortfolioApp.Domain.Common;
using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TechnologyStack { get; set; } = string.Empty;
    public string Architecture { get; set; } = string.Empty;
    public string GitHubUrl { get; set; } = string.Empty;
    public string LiveDemoUrl { get; set; } = string.Empty;
    public string VideoDemoUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string Features { get; set; } = string.Empty;
    public string Responsibilities { get; set; } = string.Empty;
    public string Challenges { get; set; } = string.Empty;
    public string Outcomes { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Duration { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; } = ProjectStatus.Completed;
    public string Category { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; } = false;
    public int ViewCount { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
}

public class ProjectImage : BaseEntity
{
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public string ImageUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsThumbnail { get; set; } = false;
}
