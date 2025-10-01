using GRC.Domain.Enums;

namespace GRC.Application.Contracts.DTOs.Risks;

/// <summary>
/// Risk matrix (heat map) DTO.
/// </summary>
public class RiskMatrixDto
{
    /// <summary>
    /// Matrix verisi. Key: "Likelihood,Impact", Value: Risk listesi
    /// </summary>
    public Dictionary<string, List<RiskMatrixItemDto>> MatrixData { get; set; } = new();

    public int TotalRisks { get; set; }
    public bool UseResidualRisk { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Department { get; set; }
}

public class RiskMatrixItemDto
{
    public Guid RiskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Score { get; set; }
    public RiskLevel Level { get; set; }
    public string OwnerId { get; set; } = string.Empty;
}
