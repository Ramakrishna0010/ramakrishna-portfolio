using PortfolioApp.Domain.Common;

namespace PortfolioApp.Domain.Entities;

public class Resume : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public bool IsCurrentVersion { get; set; } = false;
    public int DownloadCount { get; set; } = 0;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
