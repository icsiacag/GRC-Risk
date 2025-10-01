using GRC.SharedKernel.Entities;

namespace GRC.Domain.Entities.Audit;

public class CorrectiveAction : AuditableEntity
{
    // Basic implementation
    public Guid AuditFindingId { get; private set; }
    public AuditFinding AuditFinding { get; private set; } = null!;
    // Diğer property'ler...
}
