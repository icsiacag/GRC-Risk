using MediatR;

namespace GRC.SharedKernel.Events;

/// <summary>
/// Domain event marker interface.
/// Domain event'ler MediatR INotification'dan türer.
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Event'in oluşma zamanı.
    /// </summary>
    DateTime OccurredOn { get; }

    /// <summary>
    /// Event ID (tracking için).
    /// </summary>
    Guid EventId { get; }
}
