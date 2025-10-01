using Ardalis.GuardClauses;
using GRC.Domain.Enums;
using GRC.SharedKernel.Entities;
using System;

namespace GRC.Domain.Entities.RiskManagement;

/// <summary>
/// Kontrol entity.
/// Riski azaltmak için uygulanan kontrol mekanizması.
/// </summary>
public class Control : AuditableEntity
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
    /// Kontrol adı.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Kontrol açıklaması.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Kontrol tipi (Preventive, Detective, Corrective).
    /// </summary>
    public ControlType Type { get; private set; }

    /// <summary>
    /// Kontrol etkinliği.
    /// </summary>
    public ControlEffectiveness Effectiveness { get; private set; }

    /// <summary>
    /// Kontrol durumu.
    /// </summary>
    public ControlStatus Status { get; private set; }

    /// <summary>
    /// Kontrolü uygulayan kişi/departman.
    /// </summary>
    public string ImplementedBy { get; private set; } = string.Empty;

    /// <summary>
    /// Uygulama tarihi.
    /// </summary>
    public DateTime? ImplementationDate { get; private set; }

    /// <summary>
    /// Son test tarihi.
    /// </summary>
    public DateTime? LastTestDate { get; private set; }

    /// <summary>
    /// Bir sonraki test tarihi.
    /// </summary>
    public DateTime? NextTestDate { get; private set; }

    /// <summary>
    /// Test frekansı (gün cinsinden).
    /// Örnek: 90 = üç ayda bir
    /// </summary>
    public int TestFrequencyDays { get; private set; } = 90;

    /// <summary>
    /// Kontrol maliyeti (yıllık).
    /// </summary>
    public decimal? AnnualCost { get; private set; }

    /// <summary>
    /// Otomatik mi manuel mi.
    /// </summary>
    public bool IsAutomated { get; private set; }

    /// <summary>
    /// Notlar.
    /// </summary>
    public string? Notes { get; private set; }

    // EF Core için
    private Control() : base() { }

    private Control(
        Guid riskId,
        string name,
        string description,
        ControlType type,
        ControlEffectiveness effectiveness,
        string implementedBy,
        bool isAutomated = false) : base()
    {
        Guard.Against.Default(riskId, nameof(riskId));
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(implementedBy, nameof(implementedBy));

        RiskId = riskId;
        Name = name;
        Description = description;
        Type = type;
        Effectiveness = effectiveness;
        ImplementedBy = implementedBy;
        IsAutomated = isAutomated;
        Status = ControlStatus.Planned;
    }

    public static Control Create(
        Guid riskId,
        string name,
        string description,
        ControlType type,
        ControlEffectiveness effectiveness,
        string implementedBy,
        bool isAutomated = false)
    {
        return new Control(
            riskId,
            name,
            description,
            type,
            effectiveness,
            implementedBy,
            isAutomated);
    }

    /// <summary>
    /// Kontrolü aktive eder.
    /// </summary>
    public void Activate()
    {
        Status = ControlStatus.Active;
        ImplementationDate = DateTime.UtcNow;
        NextTestDate = DateTime.UtcNow.AddDays(TestFrequencyDays);
    }

    /// <summary>
    /// Kontrol testi kaydeder.
    /// </summary>
    public void RecordTest(ControlEffectiveness testResult, string testedBy, string? findings = null)
    {
        Guard.Against.NullOrWhiteSpace(testedBy, nameof(testedBy));

        LastTestDate = DateTime.UtcNow;
        NextTestDate = DateTime.UtcNow.AddDays(TestFrequencyDays);
        Effectiveness = testResult;

        if (!string.IsNullOrWhiteSpace(findings))
        {
            Notes = $"[{DateTime.UtcNow:yyyy-MM-dd}] Test by {testedBy}: {findings}\n{Notes}";
        }
    }

    /// <summary>
    /// Kontrol etkinlik faktörünü döndürür (residual risk hesaplaması için).
    /// </summary>
    public decimal GetReductionFactor()
    {
        return Effectiveness switch
        {
            ControlEffectiveness.HighlyEffective => 0.40m,
            ControlEffectiveness.Effective => 0.25m,
            ControlEffectiveness.PartiallyEffective => 0.10m,
            ControlEffectiveness.Ineffective => 0.00m,
            _ => 0.00m
        };
    }

    public void UpdateCost(decimal annualCost)
    {
        if (annualCost < 0)
            throw new ArgumentException("Cost cannot be negative", nameof(annualCost));

        AnnualCost = annualCost;
    }

    public void Deactivate(string reason)
    {
        Guard.Against.NullOrWhiteSpace(reason, nameof(reason));

        Status = ControlStatus.Inactive;
        Notes = $"[{DateTime.UtcNow:yyyy-MM-dd}] Deactivated: {reason}\n{Notes}";
    }
}
