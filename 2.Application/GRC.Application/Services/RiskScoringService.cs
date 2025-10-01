using GRC.Domain.Entities.RiskManagement;
using GRC.Domain.Interfaces.Services;
using GRC.Domain.ValueObjects;

namespace GRC.Application.Services;

/// <summary>
/// Risk scoring application service.
/// Domain service'in application layer wrapper'ı.
/// </summary>
public interface IRiskScoringApplicationService
{
    Task<RiskScore> CalculateInherentRiskAsync(int likelihood, int impact);
    Task<RiskScore> CalculateResidualRiskAsync(Guid riskId, CancellationToken cancellationToken = default);
    Task<bool> IsRiskAcceptableAsync(Guid riskId, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, int>> CalculatePortfolioScoresAsync(List<Guid> riskIds, CancellationToken cancellationToken = default);
}

public class RiskScoringApplicationService : IRiskScoringApplicationService
{
    private readonly IRiskCalculationService _domainService;
    private readonly Domain.Interfaces.Repositories.IRiskRepository _riskRepository;

    public RiskScoringApplicationService(
        IRiskCalculationService domainService,
        Domain.Interfaces.Repositories.IRiskRepository riskRepository)
    {
        _domainService = domainService;
        _riskRepository = riskRepository;
    }

    public async Task<RiskScore> CalculateInherentRiskAsync(int likelihood, int impact)
    {
        return await Task.FromResult(_domainService.CalculateRiskScore(likelihood, impact));
    }

    public async Task<RiskScore> CalculateResidualRiskAsync(Guid riskId, CancellationToken cancellationToken = default)
    {
        var risk = await _riskRepository.GetWithDetailsAsync(riskId, cancellationToken);

        if (risk == null)
            throw new InvalidOperationException($"Risk not found: {riskId}");

        return _domainService.CalculateResidualRisk(risk.InherentRisk, risk.Controls);
    }

    public async Task<bool> IsRiskAcceptableAsync(Guid riskId, CancellationToken cancellationToken = default)
    {
        var risk = await _riskRepository.GetByIdAsync(riskId, cancellationToken);

        if (risk == null)
            return false;

        return !_domainService.IsRiskAboveAppetite(risk);
    }

    public async Task<Dictionary<Guid, int>> CalculatePortfolioScoresAsync(
        List<Guid> riskIds,
        CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<Guid, int>();

        foreach (var riskId in riskIds)
        {
            var risk = await _riskRepository.GetByIdAsync(riskId, cancellationToken);
            if (risk != null)
            {
                var score = risk.ResidualRisk?.Score ?? risk.InherentRisk.Score;
                result[riskId] = score;
            }
        }

        return result;
    }
}
