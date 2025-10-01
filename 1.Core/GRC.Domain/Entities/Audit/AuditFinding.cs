using GRC.SharedKernel.Entities;

namespace GRC.Domain.Entities.Audit;

public class AuditFinding : AuditableEntity
{
    // Basic implementation
    public Guid AuditExecutionId { get; private set; }
    public AuditExecution AuditExecution { get; private set; } = null!;
    // Diğer property'ler...
}
