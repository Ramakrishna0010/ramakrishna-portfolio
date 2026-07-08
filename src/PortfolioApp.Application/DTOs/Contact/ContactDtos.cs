using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Application.DTOs.Contact;

public class ContactMessageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Budget { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public ContactStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? RepliedAt { get; set; }
    public string AdminNotes { get; set; } = string.Empty;
}

public class CreateContactMessageDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Budget { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
}

public class UpdateContactStatusDto
{
    public int Id { get; set; }
    public ContactStatus Status { get; set; }
    public string AdminNotes { get; set; } = string.Empty;
}
