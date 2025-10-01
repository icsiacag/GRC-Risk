using Ardalis.GuardClauses;
using GRC.Domain.Enums;
using GRC.SharedKernel.Entities;
using System;

namespace GRC.Domain.Entities.RiskManagement;

/// <summary>
/// Risk azaltım planı entity.
/// Riski azaltmak için alınacak aksiyonları temsil eder.
/// </summary>
public class RiskMitigation : AuditableEntity
{
    /// <summary>
    /// İlişkili risk ID'si.
    /// </summary>
    public Guid RiskId { get; private set; }

    /// <summary>
    /// Risk navigation property.
    /// </summary>
    public Risk Risk { get; private set; } = null!;

    /// <summary>
    /// Azaltım stratejisi.
    /// Örnek: "Implement backup system", "Transfer to insurance"
    /// </summary>
    public string Strategy { get; private set; } = string.Empty;

    /// <summary>
    /// Detaylı açıklama.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Sorumlu kişi.
    /// </summary>
    public string ResponsiblePerson { get; private set; } = string.Empty;

    /// <summary>
    /// Hedef tamamlanma tarihi.
    /// </summary>
    public DateTime TargetDate { get; private set; }

    /// <summary>
    /// Gerçek tamamlanma tarihi.
    /// </summary>
    public DateTime? CompletionDate { get; private set; }

    /// <summary>
    /// Plan durumu.
    /// </summary>
    public MitigationStatus Status { get; private set; }

    /// <summary>
    /// Tahmini maliyet.
    /// </summary>
    public decimal? EstimatedCost { get; private set; }

    /// <summary>
    /// Gerçek maliyet.
    /// </summary>
    public decimal? ActualCost { get; private set; }

    /// <summary>
    /// Tamamlanma yüzdesi (0-100).
    /// </summary>
    public int ProgressPercentage { get; private set; }

    /// <summary>
    /// Notlar ve güncellemeler.
    /// </summary>
    public string? Notes { get; private set; }

    // EF Core için
    private RiskMitigation() : base() { }

    private RiskMitigation(
        Guid riskId,
        string strategy,
        string description,
        DateTime targetDate,
        string responsiblePerson,
        decimal? estimatedCost = null) : base()
    {
        Guard.Against.Default(riskId, nameof(riskId));
        Guard.Against.NullOrWhiteSpace(strategy, nameof(strategy));
        Guard.Against.NullOrWhiteSpace(responsiblePerson, nameof(responsiblePerson));

        RiskId = riskId;
        Strategy = strategy;
        Description = description;
        TargetDate = targetDate;
        ResponsiblePerson = responsiblePerson;
        EstimatedCost = estimatedCost;
        Status = MitigationStatus.Planned;
        ProgressPercentage = 0;
    }

    public static RiskMitigation Create(
        Guid riskId,
        string strategy,
        string description,
        DateTime targetDate,
        string responsiblePerson,
        decimal? estimatedCost = null)
    {
        return new RiskMitigation(
            riskId,
            strategy,
            description,
            targetDate,
            responsiblePerson,
            estimatedCost);
    }

    public void UpdateProgress(int percentage, string? notes = null)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Progress must be between 0 and 100");

        ProgressPercentage = percentage;

        if (percentage == 100 && Status != MitigationStatus.Completed)
        {
            Complete();
        }
        else if (percentage > 0 && Status == MitigationStatus.Planned)
        {
            Status = MitigationStatus.InProgress;
        }

        if (!string.IsNullOrWhiteSpace(notes))
        {
            Notes = $"[{DateTime.UtcNow:yyyy-MM-dd}] Progress {percentage}%: {notes}\n{Notes}";
        }
    }

    public void Complete(decimal? actualCost = null)
    {
        Status = MitigationStatus.Completed;
        CompletionDate = DateTime.UtcNow;
        ProgressPercentage = 100;

        if (actualCost.HasValue)
        {
            ActualCost = actualCost.Value;
        }
    }

    public void Cancel(string reason)
    {
        Guard.Against.NullOrWhiteSpace(reason, nameof(reason));

        Status = MitigationStatus.Cancelled;
        Notes = $"[{DateTime.UtcNow:yyyy-MM-dd}] Cancelled: {reason}\n{Notes}";
    }

    public void ExtendDeadline(DateTime newTargetDate, string justification)
    {
        Guard.Against.NullOrWhiteSpace(justification, nameof(justification));

        if (newTargetDate <= TargetDate)
            throw new ArgumentException("New target date must be after current target date", nameof(newTargetDate));

        TargetDate = newTargetDate;
        Notes = $"[{DateTime.UtcNow:yyyy-MM-dd}] Deadline extended: {justification}\n{Notes}";
    }

    public bool IsOverdue()
    {
        return Status != MitigationStatus.Completed &&
               Status != MitigationStatus.Cancelled &&
               DateTime.UtcNow.Date > TargetDate.Date;
    }
}
