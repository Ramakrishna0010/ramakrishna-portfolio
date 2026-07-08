namespace PortfolioApp.Application.DTOs.Home;

public class HomeSectionDto
{
    public int Id { get; set; }
    public string HeroTitle { get; set; } = string.Empty;
    public string HeroSubtitle { get; set; } = string.Empty;
    public string HeroDescription { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public string HeroVideoUrl { get; set; } = string.Empty;
    public string CtaPrimaryText { get; set; } = string.Empty;
    public string CtaPrimaryUrl { get; set; } = string.Empty;
    public string CtaSecondaryText { get; set; } = string.Empty;
    public string CtaSecondaryUrl { get; set; } = string.Empty;
    public string TypewriterTexts { get; set; } = string.Empty;
    public string BackgroundType { get; set; } = string.Empty;
    public string BackgroundValue { get; set; } = string.Empty;
    public bool ShowParticles { get; set; }
    public bool ShowScrollIndicator { get; set; }
}

public class UpdateHomeSectionDto
{
    public int Id { get; set; }
    public string HeroTitle { get; set; } = string.Empty;
    public string HeroSubtitle { get; set; } = string.Empty;
    public string HeroDescription { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public string HeroVideoUrl { get; set; } = string.Empty;
    public string CtaPrimaryText { get; set; } = string.Empty;
    public string CtaPrimaryUrl { get; set; } = string.Empty;
    public string CtaSecondaryText { get; set; } = string.Empty;
    public string CtaSecondaryUrl { get; set; } = string.Empty;
    public string TypewriterTexts { get; set; } = string.Empty;
    public string BackgroundType { get; set; } = string.Empty;
    public string BackgroundValue { get; set; } = string.Empty;
    public bool ShowParticles { get; set; }
    public bool ShowScrollIndicator { get; set; }
}
