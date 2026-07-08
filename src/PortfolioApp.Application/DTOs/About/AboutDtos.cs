namespace PortfolioApp.Application.DTOs.About;

public class AboutDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ShortBio { get; set; } = string.Empty;
    public string ProfileImageUrl { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public int ProjectsCompleted { get; set; }
    public int HappyClients { get; set; }
    public string ResumeUrl { get; set; } = string.Empty;
    public bool IsAvailableForWork { get; set; }
    public string AvailabilityStatus { get; set; } = string.Empty;
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
}

public class CreateAboutDto
{
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ShortBio { get; set; } = string.Empty;
    public string ProfileImageUrl { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public int ProjectsCompleted { get; set; }
    public int HappyClients { get; set; }
    public bool IsAvailableForWork { get; set; }
    public string AvailabilityStatus { get; set; } = string.Empty;
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
}

public class UpdateAboutDto : CreateAboutDto
{
    public int Id { get; set; }
}
