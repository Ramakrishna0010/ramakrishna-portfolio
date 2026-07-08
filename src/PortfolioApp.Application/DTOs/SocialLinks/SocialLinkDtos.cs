namespace PortfolioApp.Application.DTOs.SocialLinks;

public class SocialLinkDto
{
    public int Id { get; set; }
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool ShowInHeader { get; set; }
    public bool ShowInFooter { get; set; }
    public bool ShowInAbout { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSocialLinkDto
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

public class UpdateSocialLinkDto : CreateSocialLinkDto
{
    public int Id { get; set; }
}
