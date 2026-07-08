namespace PortfolioApp.Application.DTOs.Testimonials;

public class TestimonialDto
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientTitle { get; set; } = string.Empty;
    public string ClientCompany { get; set; } = string.Empty;
    public string ClientImageUrl { get; set; } = string.Empty;
    public string ClientLinkedIn { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string ProjectWorkedOn { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsApproved { get; set; }
    public bool IsActive { get; set; }
}

public class CreateTestimonialDto
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
    public bool IsFeatured { get; set; }
}

public class UpdateTestimonialDto : CreateTestimonialDto
{
    public int Id { get; set; }
}
