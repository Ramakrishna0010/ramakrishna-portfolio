namespace PortfolioApp.Application.DTOs.Resume;

public class ResumeDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string FileSizeFormatted => FileSizeBytes < 1024 * 1024
        ? $"{FileSizeBytes / 1024.0:F1} KB"
        : $"{FileSizeBytes / (1024.0 * 1024):F1} MB";
    public string ContentType { get; set; } = string.Empty;
    public bool IsCurrentVersion { get; set; }
    public int DownloadCount { get; set; }
    public DateTime UploadedAt { get; set; }
}
