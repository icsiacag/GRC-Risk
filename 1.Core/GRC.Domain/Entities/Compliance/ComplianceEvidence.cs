using GRC.SharedKernel.Entities;

namespace GRC.Domain.Entities.Compliance;

public class ComplianceEvidence : AuditableEntity
{
    // Basic implementation
    public Guid RequirementId { get; private set; }
    public ComplianceRequirement Requirement { get; private set; } = null!;
    // Diğer property'ler...
}
