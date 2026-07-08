using PortfolioApp.Domain.Common;
using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Domain.Entities;

public class Blog : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public BlogStatus Status { get; set; } = BlogStatus.Draft;
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public int ReadTimeMinutes { get; set; }
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string MetaKeywords { get; set; } = string.Empty;
    public bool IsFeatured { get; set; } = false;
    public int AuthorId { get; set; }
    public ApplicationUser Author { get; set; } = null!;
}
