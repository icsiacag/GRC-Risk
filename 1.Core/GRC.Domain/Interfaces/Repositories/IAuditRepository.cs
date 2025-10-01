using GRC.Domain.Entities.Audit;
using GRC.Domain.Enums;

namespace GRC.Domain.Interfaces.Repositories;

public interface IAuditRepository : IRepository<AuditPlan>
{
    Task<IReadOnlyList<AuditPlan>> GetByYear(int year, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditPlan>> GetActivePlans(CancellationToken cancellationToken = default);
    Task<AuditPlan?> GetWithExecutionsAndFindings(Guid planId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditFinding>> GetOpenFindings(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditFinding>> GetFindingsByDepartment(string department, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditFinding>> GetCriticalFindings(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CorrectiveAction>> GetOverdueCorrectiveActions(CancellationToken cancellationToken = default);
    Task<Dictionary<AuditType, int>> GetAuditStatistics(int year, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(DateTime Month, int FindingCount)>> GetFindingTrends(int months, CancellationToken cancellationToken = default);
}
