using GRC.Domain.Enums;

namespace GRC.Application.Contracts.DTOs.Risks;

/// <summary>
/// Risk özet bilgisi DTO (liste görünümü için).
/// </summary>
public class RiskSummaryDto
{
    public Guid RiskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid RiskCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public RiskType Type { get; set; }
    public RiskStatus Status { get; set; }
    public string OwnerId { get; set; } = string.Empty;

    // Inherent Risk
    public int InherentScore { get; set; }
    public RiskLevel InherentLevel { get; set; }

    // Residual Risk
    public int? ResidualScore { get; set; }
    public RiskLevel? ResidualLevel { get; set; }

    public string? Department { get; set; }
    public string? Process { get; set; }
    public int ControlCount { get; set; }
    public DateTime? LastAssessmentDate { get; set; }
    public DateTime? NextReviewDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
