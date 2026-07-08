namespace PortfolioApp.Application.DTOs.Media;

public class MediaFileDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string Category { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StorageProvider { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UploadMediaDto
{
    public string Category { get; set; } = "general";
    public string AltText { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
