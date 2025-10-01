using GRC.Domain.Entities.Compliance;
using GRC.Domain.Enums;

namespace GRC.Domain.Interfaces.Repositories;

public interface IComplianceRepository : IRepository<ComplianceFramework>
{
    Task<IReadOnlyList<ComplianceFramework>> GetActiveFrameworks(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComplianceFramework>> GetByType(ComplianceFrameworkType type, CancellationToken cancellationToken = default);
    Task<ComplianceFramework?> GetWithRequirements(Guid frameworkId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComplianceRequirement>> GetComplianceGaps(Guid frameworkId, CancellationToken cancellationToken = default);
    Task<int> CalculateCompliancePercentage(Guid frameworkId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComplianceRequirement>> GetRequirementsWithoutEvidence(Guid frameworkId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComplianceCheck>> GetUpcomingChecks(int daysAhead, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, (int Total, int Compliant, int NonCompliant)>> GetComplianceSummary(CancellationToken cancellationToken = default);
}
