using MediatR;
using GRC.Application.Common.Models;

namespace GRC.Application.UseCases.Risks.Queries.GetRiskMatrix;

public class GetRiskMatrixQuery : IRequest<RiskMatrixDto>
{
}

public class RiskMatrixDto
{
    public Dictionary<string, int> Data { get; set; } = new Dictionary<string, int>();
    public List<RiskMatrixItemDto> Items { get; set; } = new List<RiskMatrixItemDto>();
}

public class RiskMatrixItemDto
{
    public int Likelihood { get; set; }
    public int Impact { get; set; }
    public int Count { get; set; }
    public List<Guid> RiskIds { get; set; } = new List<Guid>();
}
