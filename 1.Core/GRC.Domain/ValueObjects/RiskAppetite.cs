using Ardalis.GuardClauses;
using GRC.SharedKernel.Guards;
using System;

namespace GRC.Domain.ValueObjects;

/// <summary>
/// Risk iştahı value object.
/// Organizasyonun kabul etmeye hazır olduğu risk seviyesi.
/// Yönetim kurulu tarafından belirlenir.
/// </summary>
public class RiskAppetite : IEquatable<RiskAppetite>
{
    /// <summary>
    /// Kabul edilebilir risk skoru (1-100).
    /// Bu değerin üzerindeki riskler yönetim onayı gerektirir.
    /// </summary>
    public int AppetiteScore { get; private set; }

    /// <summary>
    /// Risk iştahını onaylayan yetkili.
    /// </summary>
    public string ApprovedBy { get; private set; } = string.Empty;

    /// <summary>
    /// Onay tarihi.
    /// </summary>
    public DateTime ApprovalDate { get; private set; }

    /// <summary>
    /// Gerekçe/açıklama.
    /// </summary>
    public string Justification { get; private set; } = string.Empty;

    /// <summary>
    /// Sonraki gözden geçirme tarihi.
    /// Risk iştahı yıllık olarak gözden geçirilmelidir.
    /// </summary>
    public DateTime NextReviewDate { get; private set; }

    private RiskAppetite() { }

    private RiskAppetite(int appetiteScore, string approvedBy, string justification)
    {
        Guard.Against.InvalidRiskScore(appetiteScore, nameof(appetiteScore));
        Guard.Against.NullOrWhiteSpace(approvedBy, nameof(approvedBy));
        Guard.Against.NullOrWhiteSpace(justification, nameof(justification));

        AppetiteScore = appetiteScore;
        ApprovedBy = approvedBy;
        Justification = justification;
        ApprovalDate = DateTime.UtcNow;
        NextReviewDate = DateTime.UtcNow.AddYears(1); // Yıllık gözden geçirme
    }

    public static RiskAppetite Create(int appetiteScore, string approvedBy, string justification)
    {
        return new RiskAppetite(appetiteScore, approvedBy, justification);
    }

    /// <summary>
    /// Verilen risk skorunun iştahın üzerinde olup olmadığını kontrol eder.
    /// </summary>
    public bool IsExceeded(int riskScore)
    {
        return riskScore > AppetiteScore;
    }

    /// <summary>
    /// Gözden geçirme gerekip gerekmediğini kontrol eder.
    /// </summary>
    public bool NeedsReview()
    {
        return DateTime.UtcNow >= NextReviewDate;
    }

    public bool Equals(RiskAppetite? other)
    {
        if (other is null) return false;
        return AppetiteScore == other.AppetiteScore &&
               ApprovalDate == other.ApprovalDate;
    }

    public override bool Equals(object? obj)
    {
        return obj is RiskAppetite other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AppetiteScore, ApprovalDate);
    }

    public override string ToString()
    {
        return $"RiskAppetite(Score:{AppetiteScore}, ApprovedBy:{ApprovedBy})";
    }
}
