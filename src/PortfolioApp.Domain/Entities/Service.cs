using PortfolioApp.Domain.Common;
using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Domain.Entities;

public class Service : BaseEntity
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
    public bool IsFeatured { get; set; } = false;
    public string Color { get; set; } = string.Empty;
}
