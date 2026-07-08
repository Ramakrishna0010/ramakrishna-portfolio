using PortfolioApp.Domain.Common;

namespace PortfolioApp.Domain.Entities;

public class Certificate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string IssuedBy { get; set; } = string.Empty;
    public string IssuedByLogoUrl { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool DoesNotExpire { get; set; } = false;
    public string CredentialId { get; set; } = string.Empty;
    public string CredentialUrl { get; set; } = string.Empty;
    public string CertificateImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; } = false;
}
