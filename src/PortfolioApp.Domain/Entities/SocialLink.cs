using PortfolioApp.Domain.Common;

namespace PortfolioApp.Domain.Entities;

public class SocialLink : BaseEntity
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool ShowInHeader { get; set; } = true;
    public bool ShowInFooter { get; set; } = true;
    public bool ShowInAbout { get; set; } = true;
}
