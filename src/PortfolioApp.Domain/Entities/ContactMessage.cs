using PortfolioApp.Domain.Common;
using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Budget { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public ContactStatus Status { get; set; } = ContactStatus.New;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime? ReadAt { get; set; }
    public DateTime? RepliedAt { get; set; }
    public string AdminNotes { get; set; } = string.Empty;
}
