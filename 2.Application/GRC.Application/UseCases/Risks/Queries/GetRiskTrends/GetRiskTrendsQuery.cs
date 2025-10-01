using MediatR;
using GRC.Application.Common.Models;

namespace GRC.Application.UseCases.Risks.Queries.GetRiskTrends;

public class GetRiskTrendsQuery : IRequest<RiskTrendsDto>
{
    public int Months { get; set; } = 12;
}

public class RiskTrendsDto
{
    public List<RiskTrendItemDto> Trends { get; set; } = new List<RiskTrendItemDto>();
}

public class RiskTrendItemDto
{
    public DateTime Month { get; set; }
    public int AverageScore { get; set; }
    public int RiskCount { get; set; }
    public int HighRiskCount { get; set; }
    public int CriticalRiskCount { get; set; }
}
