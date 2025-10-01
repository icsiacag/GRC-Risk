using GRC.Domain.Entities.RiskManagement;
using GRC.Domain.Enums;

namespace GRC.Domain.Interfaces.Repositories;

public interface IRiskRepository : IRepository<Risk>
{
    Task<IReadOnlyList<Risk>> GetByCategory(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Risk>> GetByOwner(string ownerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Risk>> GetByRiskLevel(RiskLevel level, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Risk>> GetByScoreRange(int minScore, int maxScore, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Risk>> GetRisksExceedingAppetite(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Risk>> GetOverdueForReview(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Risk>> GetWithActiveMitigations(CancellationToken cancellationToken = default);
    Task<Dictionary<(int Likelihood, int Impact), int>> GetHeatMapData(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(DateTime Month, int AvgScore, int RiskCount)>> GetTrends(int months, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetRiskDistributionByDepartment(CancellationToken cancellationToken = default);
    Task<Risk?> GetWithDetailsAsync(Guid riskId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Risk>> GetCreatedBetween(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Risk>> GetTopRisks(int count, CancellationToken cancellationToken = default);
}
