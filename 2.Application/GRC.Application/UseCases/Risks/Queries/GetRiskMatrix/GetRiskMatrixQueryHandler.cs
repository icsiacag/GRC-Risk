using MediatR;
using GRC.Domain.Interfaces.Repositories;

namespace GRC.Application.UseCases.Risks.Queries.GetRiskMatrix;

public class GetRiskMatrixQueryHandler : IRequestHandler<GetRiskMatrixQuery, RiskMatrixDto>
{
    private readonly IRiskRepository _riskRepository;

    public GetRiskMatrixQueryHandler(IRiskRepository riskRepository)
    {
        _riskRepository = riskRepository;
    }

    public async Task<RiskMatrixDto> Handle(GetRiskMatrixQuery request, CancellationToken cancellationToken)
    {
        var heatMapData = await _riskRepository.GetHeatMapData(cancellationToken);
        
        var result = new RiskMatrixDto();
        
        foreach (var item in heatMapData)
        {
            var key = $\""{item.Key.Likelihood}-{item.Key.Impact}\"";
            result.Data[key] = item.Value;
            
            result.Items.Add(new RiskMatrixItemDto
            {
                Likelihood = item.Key.Likelihood,
                Impact = item.Key.Impact,
                Count = item.Value
            });
        }

        return result;
    }
}
