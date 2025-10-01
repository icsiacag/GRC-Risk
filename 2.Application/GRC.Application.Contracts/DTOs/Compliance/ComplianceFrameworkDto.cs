using GRC.Domain.Enums;

namespace GRC.Application.Contracts.DTOs.Compliance;

public class ComplianceFrameworkDto
{
    public Guid FrameworkId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ComplianceFrameworkType Type { get; set; }
    public string Version { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool RequiresCertification { get; set; }
    public DateTime? CertificationDate { get; set; }
    public DateTime? CertificationExpiryDate { get; set; }
    public string? CertificationBody { get; set; }
    public string? CertificateNumber { get; set; }
    public int CompliancePercentage { get; set; }
    public int RequirementCount { get; set; }
    public bool IsCertificationExpired { get; set; }
    public bool IsCertificationExpiringSoon { get; set; }
}

public class ComplianceRequirementDto
{
    public Guid Id { get; set; }
    public string RequirementId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
    public ComplianceStatusDto? Status { get; set; }
    public int EvidenceCount { get; set; }
    public bool HasSufficientEvidence { get; set; }
}

public class ComplianceStatusDto
{
    public ComplianceLevel Level { get; set; }
    public int CompliancePercentage { get; set; }
    public DateTime LastCheckDate { get; set; }
    public string CheckedBy { get; set; } = string.Empty;
}

public class ComplianceEvidenceDto
{
    public Guid EvidenceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string UploadedBy { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }
    public bool IsApproved { get; set; }
    public bool IsExpired { get; set; }
}
