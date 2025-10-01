using GRC.Domain.Enums;

namespace GRC.Application.Contracts.DTOs.Risks;

/// <summary>
/// Risk detaylı bilgi DTO.
/// </summary>
public class RiskDetailDto
{
    public Guid RiskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RiskCategoryDto Category { get; set; } = null!;
    public RiskType Type { get; set; }
    public RiskStatus Status { get; set; }
    public string OwnerId { get; set; } = string.Empty;

    // Risk Scores
    public RiskScoreDto InherentRisk { get; set; } = null!;
    public RiskScoreDto? ResidualRisk { get; set; }
    public RiskAppetiteDto? RiskAppetite { get; set; }

    // Collections
    public List<ControlDto> Controls { get; set; } = new();
    public List<RiskAssessmentDto> Assessments { get; set; } = new();
    public List<RiskMitigationDto> Mitigations { get; set; } = new();

    public string? Department { get; set; }
    public string? Process { get; set; }
    public string? Notes { get; set; }

    public DateTime? LastAssessmentDate { get; set; }
    public DateTime? NextReviewDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? ClosureReason { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}

/// <summary>
/// Risk score DTO.
/// </summary>
public class RiskScoreDto
{
    public int Likelihood { get; set; }
    public int Impact { get; set; }
    public int Score { get; set; }
    public RiskLevel Level { get; set; }
}

/// <summary>
/// Risk appetite DTO.
/// </summary>
public class RiskAppetiteDto
{
    public int AppetiteScore { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime ApprovalDate { get; set; }
    public string Justification { get; set; } = string.Empty;
}
