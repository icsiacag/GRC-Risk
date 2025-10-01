using Ardalis.GuardClauses;
using GRC.Domain.ValueObjects;
using GRC.SharedKernel.Entities;
using GRC.SharedKernel.Guards;
using System;

namespace GRC.Domain.Entities.RiskManagement;

/// <summary>
/// Risk değerlendirme entity.
/// Bir riskin belirli bir zamandaki değerlendirmesini temsil eder.
/// Tarihsel risk trend analizi için kullanılır.
/// </summary>
public class RiskAssessment : AuditableEntity
{
    /// <summary>
    /// Değerlendirilen risk ID'si (Foreign Key).
    /// </summary>
    public Guid RiskId { get; private set; }

    /// <summary>
    /// Risk navigation property.
    /// </summary>
    public Risk Risk { get; private set; } = null!;

    /// <summary>
    /// Değerlendirme tarihi.
    /// </summary>
    public DateTime AssessmentDate { get; private set; }

    /// <summary>
    /// Değerlendirmeyi yapan kişi.
    /// </summary>
    public string AssessedBy { get; private set; } = string.Empty;

    /// <summary>
    /// Değerlendirme skoru (snapshot).
    /// </summary>
    public RiskScore Score { get; private set; } = null!;

    /// <summary>
    /// Kullanılan değerlendirme metodolojisi.
    /// Örnek: "ISO 31000", "NIST Framework", "Qualitative Analysis"
    /// </summary>
    public string Methodology { get; private set; } = string.Empty;

    /// <summary>
    /// Değerlendirme bulguları/notları.
    /// </summary>
    public string? Findings { get; private set; }

    /// <summary>
    /// Öneriler.
    /// </summary>
    public string? Recommendations { get; private set; }

    // EF Core için
    private RiskAssessment() : base() { }

    private RiskAssessment(
        Guid riskId,
        int likelihoodLevel,
        int impactLevel,
        string assessedBy,
        string methodology,
        string? findings = null,
        string? recommendations = null) : base()
    {
        Guard.Against.Default(riskId, nameof(riskId));
        Guard.Against.InvalidLikelihood(likelihoodLevel, nameof(likelihoodLevel));
        Guard.Against.InvalidImpact(impactLevel, nameof(impactLevel));
        Guard.Against.NullOrWhiteSpace(assessedBy, nameof(assessedBy));
        Guard.Against.NullOrWhiteSpace(methodology, nameof(methodology));

        RiskId = riskId;
        AssessedBy = assessedBy;
        Methodology = methodology;
        Findings = findings;
        Recommendations = recommendations;
        AssessmentDate = DateTime.UtcNow;

        Score = RiskScore.Create(likelihoodLevel, impactLevel);
    }

    public static RiskAssessment Create(
        Guid riskId,
        int likelihoodLevel,
        int impactLevel,
        string assessedBy,
        string methodology,
        string? findings = null,
        string? recommendations = null)
    {
        return new RiskAssessment(
            riskId,
            likelihoodLevel,
            impactLevel,
            assessedBy,
            methodology,
            findings,
            recommendations);
    }

    public void UpdateFindings(string findings, string? recommendations = null)
    {
        Findings = findings;
        Recommendations = recommendations;
    }
}
