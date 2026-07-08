using PortfolioApp.Domain.Common;

namespace PortfolioApp.Domain.Entities;

public class Testimonial : BaseEntity
{
    public string ClientName { get; set; } = string.Empty;
    public string ClientTitle { get; set; } = string.Empty;
    public string ClientCompany { get; set; } = string.Empty;
    public string ClientImageUrl { get; set; } = string.Empty;
    public string ClientLinkedIn { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public string ProjectWorkedOn { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; } = false;
    public bool IsApproved { get; set; } = true;
}
