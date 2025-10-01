using Ardalis.GuardClauses;
using GRC.Domain.Enums;
using GRC.SharedKernel.Guards;
using System;

namespace GRC.Domain.ValueObjects;

/// <summary>
/// Risk skoru value object (immutable).
/// Likelihood ve Impact'ten hesaplanır.
/// 
/// Risk Skoru Hesaplama:
/// Score = Likelihood × Impact (1-25 arası)
/// Normalizasyon: (Score / 25) × 100 = 1-100 arası
/// </summary>
public class RiskScore : IEquatable<RiskScore>, IComparable<RiskScore>
{
    /// <summary>
    /// Olasılık seviyesi (1-5).
    /// 1: Very Low, 2: Low, 3: Medium, 4: High, 5: Very High
    /// </summary>
    public int Likelihood { get; private set; }

    /// <summary>
    /// Etki seviyesi (1-5).
    /// 1: Minimal, 2: Minor, 3: Moderate, 4: Major, 5: Catastrophic
    /// </summary>
    public int Impact { get; private set; }

    /// <summary>
    /// Hesaplanmış risk skoru (1-100).
    /// </summary>
    public int Score { get; private set; }

    /// <summary>
    /// Risk seviyesi (düşük, orta, yüksek, kritik).
    /// </summary>
    public RiskLevel Level { get; private set; }

    // EF Core için private constructor
    private RiskScore() { }

    private RiskScore(int likelihood, int impact)
    {
        Guard.Against.InvalidLikelihood(likelihood, nameof(likelihood));
        Guard.Against.InvalidImpact(impact, nameof(impact));

        Likelihood = likelihood;
        Impact = impact;
        Score = CalculateScore(likelihood, impact);
        Level = DetermineLevel(Score);
    }

    /// <summary>
    /// RiskScore oluşturur (Factory method).
    /// </summary>
    public static RiskScore Create(int likelihood, int impact)
    {
        return new RiskScore(likelihood, impact);
    }

    /// <summary>
    /// Risk skorunu hesaplar.
    /// Formula: (Likelihood × Impact / 25) × 100
    /// </summary>
    private static int CalculateScore(int likelihood, int impact)
    {
        var rawScore = likelihood * impact; // 1-25 arası
        var normalizedScore = (int)Math.Ceiling((rawScore / 25.0) * 100); // 1-100 arası
        return normalizedScore;
    }

    /// <summary>
    /// Skora göre risk seviyesini belirler.
    /// </summary>
    private static RiskLevel DetermineLevel(int score)
    {
        return score switch
        {
            <= 25 => RiskLevel.Low,      // 1-25: Düşük risk
            <= 50 => RiskLevel.Medium,   // 26-50: Orta risk
            <= 75 => RiskLevel.High,     // 51-75: Yüksek risk
            _ => RiskLevel.Critical      // 76-100: Kritik risk
        };
    }

    /// <summary>
    /// Risk seviyesi renk kodu (UI için).
    /// </summary>
    public string GetLevelColorCode()
    {
        return Level switch
        {
            RiskLevel.Low => "#28a745",      // Yeşil
            RiskLevel.Medium => "#ffc107",   // Sarı
            RiskLevel.High => "#fd7e14",     // Turuncu
            RiskLevel.Critical => "#dc3545", // Kırmızı
            _ => "#6c757d"                   // Gri (undefined)
        };
    }

    // Value object equality (değer bazlı eşitlik)
    public bool Equals(RiskScore? other)
    {
        if (other is null) return false;
        return Likelihood == other.Likelihood && Impact == other.Impact;
    }

    public override bool Equals(object? obj)
    {
        return obj is RiskScore other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Likelihood, Impact);
    }

    public int CompareTo(RiskScore? other)
    {
        if (other is null) return 1;
        return Score.CompareTo(other.Score);
    }

    public static bool operator ==(RiskScore? left, RiskScore? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RiskScore? left, RiskScore? right)
    {
        return !Equals(left, right);
    }

    public static bool operator >(RiskScore left, RiskScore right)
    {
        return left.Score > right.Score;
    }

    public static bool operator <(RiskScore left, RiskScore right)
    {
        return left.Score < right.Score;
    }

    public override string ToString()
    {
        return $"RiskScore(L:{Likelihood}, I:{Impact}, Score:{Score}, Level:{Level})";
    }
}
