namespace GRC.Application.Contracts.DTOs.Risks;

/// <summary>
/// Risk trend analizi DTO.
/// </summary>
public class RiskTrendsDto
{
    public int Months { get; set; }
    public List<RiskTrendDataPointDto> TrendData { get; set; } = new();
}

public class RiskTrendDataPointDto
{
    public DateTime Month { get; set; }
    public int AverageScore { get; set; }
    public int RiskCount { get; set; }
}
