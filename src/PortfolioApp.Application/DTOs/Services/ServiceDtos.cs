using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Application.DTOs.Services;

public class ServiceDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public string ServiceTypeName => ServiceType.ToString();
    public string Features { get; set; } = string.Empty;
    public string TechnologiesUsed { get; set; } = string.Empty;
    public decimal? StartingPrice { get; set; }
    public string PricingUnit { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public string Color { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateServiceDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public string Features { get; set; } = string.Empty;
    public string TechnologiesUsed { get; set; } = string.Empty;
    public decimal? StartingPrice { get; set; }
    public string PricingUnit { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public string Color { get; set; } = string.Empty;
}

public class UpdateServiceDto : CreateServiceDto
{
    public int Id { get; set; }
}
