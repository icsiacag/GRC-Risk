using GRC.Domain.Entities.Compliance;
using GRC.Domain.ValueObjects;

namespace GRC.Domain.Interfaces.Services;

public interface IComplianceValidationService
{
    Task<ComplianceStatus> ValidateRequirement(ComplianceRequirement requirement, CancellationToken cancellationToken = default);
    Task<ComplianceStatus> ValidateFramework(ComplianceFramework framework, CancellationToken cancellationToken = default);
    bool IsEvidenceSufficient(ComplianceEvidence evidence, ComplianceRequirement requirement);
    Task<IReadOnlyList<(ComplianceRequirement Requirement, string Gap)>> AnalyzeGaps(Guid frameworkId, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, ComplianceStatus>> RunAutomatedChecks(CancellationToken cancellationToken = default);
    int CalculateComplianceRisk(ComplianceFramework framework);
}
