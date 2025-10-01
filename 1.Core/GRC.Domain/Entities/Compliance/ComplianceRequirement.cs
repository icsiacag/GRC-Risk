using GRC.SharedKernel.Entities;

namespace GRC.Domain.Entities.Compliance;

public class ComplianceRequirement : AuditableEntity
{
    // Basic implementation
    public Guid FrameworkId { get; private set; }
    public ComplianceFramework Framework { get; private set; } = null!;
    // Diğer property'ler...
}
