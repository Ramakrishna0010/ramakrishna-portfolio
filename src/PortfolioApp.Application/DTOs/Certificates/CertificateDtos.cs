namespace PortfolioApp.Application.DTOs.Certificates;

public class CertificateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IssuedBy { get; set; } = string.Empty;
    public string IssuedByLogoUrl { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool DoesNotExpire { get; set; }
    public string CredentialId { get; set; } = string.Empty;
    public string CredentialUrl { get; set; } = string.Empty;
    public string CertificateImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
}

public class CreateCertificateDto
{
    public string Name { get; set; } = string.Empty;
    public string IssuedBy { get; set; } = string.Empty;
    public string IssuedByLogoUrl { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool DoesNotExpire { get; set; }
    public string CredentialId { get; set; } = string.Empty;
    public string CredentialUrl { get; set; } = string.Empty;
    public string CertificateImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
}

public class UpdateCertificateDto : CreateCertificateDto
{
    public int Id { get; set; }
}
