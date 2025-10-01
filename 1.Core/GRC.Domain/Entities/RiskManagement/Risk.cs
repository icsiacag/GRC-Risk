using Ardalis.GuardClauses;
using GRC.Domain.Enums;
using GRC.Domain.Events;
using GRC.Domain.ValueObjects;
using GRC.SharedKernel.Entities;
using GRC.SharedKernel.Events;
using GRC.SharedKernel.Guards;
using System;

namespace GRC.Domain.Entities.RiskManagement;

/// <summary>
/// Risk entity - GRC sisteminin merkezi aggregate root'u.
/// Bir riski ve onunla ilişkili tüm bilgileri temsil eder.
/// 
/// Risk Yönetimi İş Kuralları:
/// 1. Her risk bir kategoriye ait olmalıdır
/// 2. Risk skoru otomatik hesaplanır (Likelihood × Impact)
/// 3. Residual risk, inherent risk'ten büyük olamaz
/// 4. Risk sahibi (owner) zorunludur
/// 5. Yüksek riskler zorunlu olarak kontrol gerektirir
/// </summary>
public class Risk : AuditableEntity, IAggregateRoot, IHasDomainEvents, ISoftDeletable
{
    // Domain Events Collection
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    #region Properties

    /// <summary>
    /// Risk başlığı - kısa açıklayıcı başlık.
    /// Örnek: "Veri merkezi yangın riski"
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Risk detaylı açıklaması.
    /// Riskin ne olduğu, hangi koşullarda gerçekleşebileceği.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Risk kategorisi ID'si.
    /// Foreign key - RiskCategory entity'sine.
    /// </summary>
    public Guid RiskCategoryId { get; private set; }

    /// <summary>
    /// Risk kategorisi navigation property.
    /// </summary>
    public RiskCategory Category { get; private set; } = null!;

    /// <summary>
    /// Risk tipi (Strategic, Operational, Financial, vb.)
    /// </summary>
    public RiskType Type { get; private set; }

    /// <summary>
    /// Risk durumu (Identified, Assessed, Mitigated, Closed)
    /// </summary>
    public RiskStatus Status { get; private set; }

    /// <summary>
    /// Risk sahibi (sorumlu kişi) ID'si.
    /// User entity'sine referans.
    /// </summary>
    public string OwnerId { get; private set; } = string.Empty;

    /// <summary>
    /// Inherent Risk - kontrol öncesi doğal risk seviyesi.
    /// Value object (immutable).
    /// </summary>
    public RiskScore InherentRisk { get; private set; } = null!;

    /// <summary>
    /// Residual Risk - kontroller uygulandıktan sonra kalan risk.
    /// Value object (immutable).
    /// </summary>
    public RiskScore? ResidualRisk { get; private set; }

    /// <summary>
    /// Risk iştahı (kabul edilebilir risk seviyesi).
    /// Yönetim tarafından belirlenir.
    /// </summary>
    public RiskAppetite? RiskAppetite { get; private set; }

    /// <summary>
    /// Risk değerlendirmeleri koleksiyonu.
    /// Bir risk birden fazla kez değerlendirilebilir (tarihsel trend için).
    /// </summary>
    private readonly List<RiskAssessment> _assessments = new();
    public IReadOnlyCollection<RiskAssessment> Assessments => _assessments.AsReadOnly();

    /// <summary>
    /// Risk kontrolleri koleksiyonu.
    /// Riski azaltmak için uygulanan kontroller.
    /// </summary>
    private readonly List<Control> _controls = new();
    public IReadOnlyCollection<Control> Controls => _controls.AsReadOnly();

    /// <summary>
    /// Risk azaltım planları.
    /// </summary>
    private readonly List<RiskMitigation> _mitigations = new();
    public IReadOnlyCollection<RiskMitigation> Mitigations => _mitigations.AsReadOnly();

    /// <summary>
    /// Riskin son değerlendirme tarihi.
    /// </summary>
    public DateTime? LastAssessmentDate { get; private set; }

    /// <summary>
    /// Bir sonraki değerlendirme tarihi.
    /// Risk yönetim politikasına göre belirlenir (genellikle 3-6 ay).
    /// </summary>
    public DateTime? NextReviewDate { get; private set; }

    /// <summary>
    /// Risk kapatılma tarihi (eğer kapatıldıysa).
    /// </summary>
    public DateTime? ClosedDate { get; private set; }

    /// <summary>
    /// Kapatılma gerekçesi.
    /// </summary>
    public string? ClosureReason { get; private set; }

    /// <summary>
    /// Departman veya iş birimi.
    /// </summary>
    public string? Department { get; private set; }

    /// <summary>
    /// İlgili süreç veya sistem.
    /// </summary>
    public string? Process { get; private set; }

    /// <summary>
    /// Ek notlar veya yorumlar.
    /// </summary>
    public string? Notes { get; private set; }

    // Soft Delete Properties
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    #endregion

    #region Constructors

    // EF Core için parametre-siz constructor
    private Risk() : base()
    {
    }

    /// <summary>
    /// Yeni bir risk oluşturur.
    /// Factory method kullanımı önerilir (Risk.Create).
    /// </summary>
    private Risk(
        string title,
        string description,
        Guid riskCategoryId,
        RiskType type,
        string ownerId,
        RiskScore inherentRisk,
        string? department = null,
        string? process = null) : base()
    {
        // Guard clauses - validation
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.NullOrWhiteSpace(description, nameof(description));
        Guard.Against.Default(riskCategoryId, nameof(riskCategoryId));
        Guard.Against.NullOrWhiteSpace(ownerId, nameof(ownerId));
        Guard.Against.Null(inherentRisk, nameof(inherentRisk));

        Title = title;
        Description = description;
        RiskCategoryId = riskCategoryId;
        Type = type;
        OwnerId = ownerId;
        InherentRisk = inherentRisk;
        Status = RiskStatus.Identified;
        Department = department;
        Process = process;

        // Next review date - 3 ay sonra (varsayılan)
        NextReviewDate = DateTime.UtcNow.AddMonths(3);

        // Domain event publish
        AddDomainEvent(new RiskCreatedEvent(Id, title, inherentRisk.Score));
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Yeni bir risk oluşturur (Factory method pattern).
    /// Bu method kullanılarak risk oluşturulması önerilir.
    /// </summary>
    public static Risk Create(
        string title,
        string description,
        Guid riskCategoryId,
        RiskType type,
        string ownerId,
        int likelihoodLevel,
        int impactLevel,
        string? department = null,
        string? process = null)
    {
        // RiskScore value object oluştur
        var inherentRisk = RiskScore.Create(likelihoodLevel, impactLevel);

        return new Risk(
            title,
            description,
            riskCategoryId,
            type,
            ownerId,
            inherentRisk,
            department,
            process);
    }

    #endregion

    #region Business Logic Methods

    /// <summary>
    /// Risk bilgilerini günceller.
    /// İş kuralı: Kapalı riskler güncellenemez.
    /// </summary>
    public void Update(
        string title,
        string description,
        RiskType type,
        string? department = null,
        string? process = null,
        string? notes = null)
    {
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.NullOrWhiteSpace(description, nameof(description));

        if (Status == RiskStatus.Closed)
            throw new InvalidOperationException("Cannot update a closed risk");

        Title = title;
        Description = description;
        Type = type;
        Department = department;
        Process = process;
        Notes = notes;

        AddDomainEvent(new RiskUpdatedEvent(Id, title));
    }

    /// <summary>
    /// Risk sahibini değiştirir.
    /// İş kuralı: Yeni sahip mevcut sahibinden farklı olmalı.
    /// </summary>
    public void TransferOwnership(string newOwnerId, string reason)
    {
        Guard.Against.NullOrWhiteSpace(newOwnerId, nameof(newOwnerId));
        Guard.Against.NullOrWhiteSpace(reason, nameof(reason));

        if (OwnerId == newOwnerId)
            throw new InvalidOperationException("New owner cannot be the same as current owner");

        var previousOwner = OwnerId;
        OwnerId = newOwnerId;

        AddDomainEvent(new RiskOwnershipTransferredEvent(
            Id,
            previousOwner,
            newOwnerId,
            reason));
    }

    /// <summary>
    /// Yeni bir değerlendirme ekler.
    /// Inherent risk güncellenir ve residual risk hesaplanır.
    /// </summary>
    public void AddAssessment(
        int likelihoodLevel,
        int impactLevel,
        string assessedBy,
        string methodology,
        string? findings = null)
    {
        Guard.Against.InvalidLikelihood(likelihoodLevel, nameof(likelihoodLevel));
        Guard.Against.InvalidImpact(impactLevel, nameof(impactLevel));
        Guard.Against.NullOrWhiteSpace(assessedBy, nameof(assessedBy));

        var assessment = RiskAssessment.Create(
            Id,
            likelihoodLevel,
            impactLevel,
            assessedBy,
            methodology,
            findings);

        _assessments.Add(assessment);

        // Inherent risk'i güncelle
        InherentRisk = RiskScore.Create(likelihoodLevel, impactLevel);
        LastAssessmentDate = DateTime.UtcNow;

        // Status'ü güncelle
        if (Status == RiskStatus.Identified)
            Status = RiskStatus.Assessed;

        // Residual risk hesapla (eğer kontroller varsa)
        RecalculateResidualRisk();

        AddDomainEvent(new RiskAssessedEvent(
            Id,
            InherentRisk.Score,
            ResidualRisk?.Score));
    }

    /// <summary>
    /// Risk'e kontrol ekler.
    /// İş kuralı: Aynı kontrol birden fazla kez eklenemez.
    /// </summary>
    public void AssignControl(
        string controlName,
        string controlDescription,
        ControlType controlType,
        ControlEffectiveness effectiveness,
        string implementedBy)
    {
        Guard.Against.NullOrWhiteSpace(controlName, nameof(controlName));
        Guard.Against.NullOrWhiteSpace(implementedBy, nameof(implementedBy));

        // Aynı isimde kontrol var mı kontrol et
        if (_controls.Any(c => c.Name.Equals(controlName, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Control '{controlName}' already exists for this risk");

        var control = Control.Create(
            Id,
            controlName,
            controlDescription,
            controlType,
            effectiveness,
            implementedBy);

        _controls.Add(control);

        // Residual risk yeniden hesapla
        RecalculateResidualRisk();

        AddDomainEvent(new ControlAssignedToRiskEvent(Id, control.Id, controlName));
    }

    /// <summary>
    /// Kontrolü kaldırır.
    /// </summary>
    public void RemoveControl(Guid controlId, string reason)
    {
        Guard.Against.Default(controlId, nameof(controlId));
        Guard.Against.NullOrWhiteSpace(reason, nameof(reason));

        var control = _controls.FirstOrDefault(c => c.Id == controlId);
        if (control == null)
            throw new InvalidOperationException($"Control {controlId} not found");

        _controls.Remove(control);

        // Residual risk yeniden hesapla
        RecalculateResidualRisk();

        AddDomainEvent(new ControlRemovedFromRiskEvent(Id, controlId, reason));
    }

    /// <summary>
    /// Azaltım planı ekler.
    /// </summary>
    public void AddMitigation(
        string strategy,
        string description,
        DateTime targetDate,
        string responsiblePerson,
        decimal? estimatedCost = null)
    {
        Guard.Against.NullOrWhiteSpace(strategy, nameof(strategy));
        Guard.Against.NullOrWhiteSpace(responsiblePerson, nameof(responsiblePerson));

        if (targetDate < DateTime.UtcNow.Date)
            throw new ArgumentException("Target date cannot be in the past", nameof(targetDate));

        var mitigation = RiskMitigation.Create(
            Id,
            strategy,
            description,
            targetDate,
            responsiblePerson,
            estimatedCost);

        _mitigations.Add(mitigation);

        AddDomainEvent(new RiskMitigationPlannedEvent(Id, mitigation.Id, strategy));
    }

    /// <summary>
    /// Risk iştahını belirler.
    /// Yönetim tarafından onaylanmalıdır.
    /// </summary>
    public void SetRiskAppetite(int appetiteScore, string approvedBy, string justification)
    {
        Guard.Against.InvalidRiskScore(appetiteScore, nameof(appetiteScore));
        Guard.Against.NullOrWhiteSpace(approvedBy, nameof(approvedBy));

        RiskAppetite = ValueObjects.RiskAppetite.Create(appetiteScore, approvedBy, justification);

        // Risk iştahı aşıldıysa uyarı event'i
        if (ResidualRisk != null && ResidualRisk.Score > appetiteScore)
        {
            AddDomainEvent(new RiskAppetiteExceededEvent(Id, ResidualRisk.Score, appetiteScore));
        }
    }

    /// <summary>
    /// Riski kapatır.
    /// İş kuralı: Sadece düşük riskler veya tamamen azaltılmış riskler kapatılabilir.
    /// </summary>
    public void Close(string closedBy, string reason)
    {
        Guard.Against.NullOrWhiteSpace(closedBy, nameof(closedBy));
        Guard.Against.NullOrWhiteSpace(reason, nameof(reason));

        if (Status == RiskStatus.Closed)
            throw new InvalidOperationException("Risk is already closed");

        // İş kuralı: Yüksek riskler kapatılamaz
        if (ResidualRisk != null && ResidualRisk.Level == RiskLevel.High)
            throw new InvalidOperationException("Cannot close a high-level risk");

        Status = RiskStatus.Closed;
        ClosedDate = DateTime.UtcNow;
        ClosureReason = reason;

        AddDomainEvent(new RiskClosedEvent(Id, reason, closedBy));
    }

    /// <summary>
    /// Kapalı riski yeniden açar.
    /// </summary>
    public void Reopen(string reopenedBy, string reason)
    {
        Guard.Against.NullOrWhiteSpace(reopenedBy, nameof(reopenedBy));
        Guard.Against.NullOrWhiteSpace(reason, nameof(reason));

        if (Status != RiskStatus.Closed)
            throw new InvalidOperationException("Only closed risks can be reopened");

        Status = RiskStatus.Assessed;
        ClosedDate = null;
        ClosureReason = null;

        AddDomainEvent(new RiskReopenedEvent(Id, reason, reopenedBy));
    }

    /// <summary>
    /// Residual risk'i yeniden hesaplar.
    /// Kontrollerin etkinliğine göre inherent risk'ten düşer.
    /// 
    /// Hesaplama mantığı:
    /// - Her kontrol etkinliğine göre bir azaltım yüzdesi sağlar
    /// - Highly Effective: %40 azaltım
    /// - Effective: %25 azaltım
    /// - Partially Effective: %10 azaltım
    /// - Ineffective: %0 azaltım
    /// </summary>
    private void RecalculateResidualRisk()
    {
        if (!_controls.Any())
        {
            ResidualRisk = InherentRisk; // Kontrol yoksa residual = inherent
            return;
        }

        // Aktif kontrollerin toplam etkinlik faktörünü hesapla
        var totalReductionFactor = _controls
            .Where(c => c.Status == ControlStatus.Active)
            .Sum(c => c.GetReductionFactor());

        // Maksimum %80 azaltım (güvenlik için buffer)
        totalReductionFactor = Math.Min(totalReductionFactor, 0.80m);

        // Yeni residual score hesapla
        var residualScore = (int)Math.Ceiling(
            InherentRisk.Score * (1 - totalReductionFactor));

        // Minimum 1 olmalı (risk asla sıfır olamaz)
        residualScore = Math.Max(residualScore, 1);

        // ResidualRisk value object'i güncelle
        var newLikelihood = (int)Math.Ceiling(InherentRisk.Likelihood * (1 - totalReductionFactor / 2));
        var newImpact = (int)Math.Ceiling(InherentRisk.Impact * (1 - totalReductionFactor / 2));

        newLikelihood = Math.Max(1, Math.Min(5, newLikelihood));
        newImpact = Math.Max(1, Math.Min(5, newImpact));

        ResidualRisk = RiskScore.Create(newLikelihood, newImpact);

        // Residual risk değişikliği event'i
        AddDomainEvent(new ResidualRiskChangedEvent(
            Id,
            InherentRisk.Score,
            ResidualRisk.Score));
    }

    /// <summary>
    /// Bir sonraki değerlendirme tarihini günceller.
    /// </summary>
    public void SetNextReviewDate(DateTime reviewDate)
    {
        if (reviewDate <= DateTime.UtcNow)
            throw new ArgumentException("Next review date must be in the future", nameof(reviewDate));

        NextReviewDate = reviewDate;
    }

    #endregion

    #region Soft Delete

    public void Delete(string userId)
    {
        Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

        if (Status != RiskStatus.Closed)
            throw new InvalidOperationException("Only closed risks can be deleted");

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = userId;

        AddDomainEvent(new RiskDeletedEvent(Id, userId));
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }

    #endregion

    #region Domain Events

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    #endregion
}
