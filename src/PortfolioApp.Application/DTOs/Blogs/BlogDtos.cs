using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Application.DTOs.Blogs;

public class BlogDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public BlogStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int ReadTimeMinutes { get; set; }
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string MetaKeywords { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateBlogDto
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public BlogStatus Status { get; set; } = BlogStatus.Draft;
    public int ReadTimeMinutes { get; set; }
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string MetaKeywords { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
}

public class UpdateBlogDto : CreateBlogDto
{
    public int Id { get; set; }
}

public class BlogListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public int ReadTimeMinutes { get; set; }
    public bool IsFeatured { get; set; }
    public string AuthorName { get; set; } = string.Empty;
}
