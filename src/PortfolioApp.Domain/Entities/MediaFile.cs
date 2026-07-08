using PortfolioApp.Domain.Common;

namespace PortfolioApp.Domain.Entities;

public class MediaFile : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string Category { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StorageProvider { get; set; } = "Local";
    public string BlobName { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public int UploadedByUserId { get; set; }
}
