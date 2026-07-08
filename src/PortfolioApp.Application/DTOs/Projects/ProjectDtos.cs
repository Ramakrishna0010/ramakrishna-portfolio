using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Application.DTOs.Projects;

public class ProjectDto
{
    public int Id { get; set; }
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
    public ProjectStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public string Category { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsActive { get; set; }
    public List<ProjectImageDto> Images { get; set; } = new();
}

public class ProjectImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsThumbnail { get; set; }
}

public class CreateProjectDto
{
    public string Title { get; set; } = string.Empty;
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
    public bool IsFeatured { get; set; }
}

public class UpdateProjectDto : CreateProjectDto
{
    public int Id { get; set; }
}
