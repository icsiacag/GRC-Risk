using GRC.SharedKernel.Events;

namespace GRC.Domain.Events;

public record RiskCreatedEvent(
    Guid RiskId,
    string Title,
    int InherentScore) : DomainEventBase;

public record RiskUpdatedEvent(
    Guid RiskId,
    string Title) : DomainEventBase;

public record RiskAssessedEvent(
    Guid RiskId,
    int InherentScore,
    int? ResidualScore) : DomainEventBase;

public record RiskOwnershipTransferredEvent(
    Guid RiskId,
    string PreviousOwnerId,
    string NewOwnerId,
    string Reason) : DomainEventBase;

public record ControlAssignedToRiskEvent(
    Guid RiskId,
    Guid ControlId,
    string ControlName) : DomainEventBase;

public record ControlRemovedFromRiskEvent(
    Guid RiskId,
    Guid ControlId,
    string Reason) : DomainEventBase;

public record RiskMitigationPlannedEvent(
    Guid RiskId,
    Guid MitigationId,
    string Strategy) : DomainEventBase;

public record ResidualRiskChangedEvent(
    Guid RiskId,
    int InherentScore,
    int ResidualScore) : DomainEventBase;

public record RiskAppetiteExceededEvent(
    Guid RiskId,
    int CurrentScore,
    int AppetiteThreshold) : DomainEventBase;

public record RiskClosedEvent(
    Guid RiskId,
    string Reason,
    string ClosedBy) : DomainEventBase;

public record RiskReopenedEvent(
    Guid RiskId,
    string Reason,
    string ReopenedBy) : DomainEventBase;

public record RiskDeletedEvent(
    Guid RiskId,
    string DeletedBy) : DomainEventBase;
