using GRC.SharedKernel.Entities;

namespace GRC.Domain.Entities.Audit;

public class AuditExecution : AuditableEntity
{
    // Basic implementation
    public Guid AuditPlanId { get; private set; }
    public AuditPlan AuditPlan { get; private set; } = null!;
    // Diğer property'ler...
}
