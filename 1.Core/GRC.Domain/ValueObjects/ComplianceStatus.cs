using GRC.Domain.Enums;

namespace GRC.Domain.ValueObjects;

/// <summary>
/// Uyum durumu value object.
/// Compliance kontrollerinin sonucunu temsil eder.
/// </summary>
public class ComplianceStatus : IEquatable<ComplianceStatus>
{
    /// <summary>
    /// Uyum seviyesi.
    /// </summary>
    public ComplianceLevel Level { get; private set; }

    /// <summary>
    /// Uyum yüzdesi (0-100).
    /// </summary>
    public int CompliancePercentage { get; private set; }

    /// <summary>
    /// Son kontrol tarihi.
    /// </summary>
    public DateTime LastCheckDate { get; private set; }

    /// <summary>
    /// Kontrol eden kişi.
    /// </summary>
    public string CheckedBy { get; private set; } = string.Empty;

    /// <summary>
    /// Uyum durumu notları.
    /// </summary>
    public string? Notes { get; private set; }

    private ComplianceStatus() { }

    private ComplianceStatus(
        ComplianceLevel level,
        int compliancePercentage,
        string checkedBy,
        string? notes = null)
    {
        if (compliancePercentage < 0 || compliancePercentage > 100)
            throw new ArgumentOutOfRangeException(nameof(compliancePercentage));

        Level = level;
        CompliancePercentage = compliancePercentage;
        CheckedBy = checkedBy;
        Notes = notes;
        LastCheckDate = DateTime.UtcNow;
    }

    public static ComplianceStatus Create(
        ComplianceLevel level,
        int compliancePercentage,
        string checkedBy,
        string? notes = null)
    {
        return new ComplianceStatus(level, compliancePercentage, checkedBy, notes);
    }

    /// <summary>
    /// Uyum seviyesini otomatik belirler (yüzdeden).
    /// </summary>
    public static ComplianceStatus CreateFromPercentage(
        int compliancePercentage,
        string checkedBy,
        string? notes = null)
    {
        var level = compliancePercentage switch
        {
            >= 95 => ComplianceLevel.FullyCompliant,
            >= 80 => ComplianceLevel.SubstantiallyCompliant,
            >= 60 => ComplianceLevel.PartiallyCompliant,
            _ => ComplianceLevel.NonCompliant
        };

        return new ComplianceStatus(level, compliancePercentage, checkedBy, notes);
    }

    public bool IsCompliant()
    {
        return Level is ComplianceLevel.FullyCompliant or ComplianceLevel.SubstantiallyCompliant;
    }

    public string GetStatusColorCode()
    {
        return Level switch
        {
            ComplianceLevel.FullyCompliant => "#28a745",           // Yeşil
            ComplianceLevel.SubstantiallyCompliant => "#5cb85c",   // Açık yeşil
            ComplianceLevel.PartiallyCompliant => "#ffc107",       // Sarı
            ComplianceLevel.NonCompliant => "#dc3545",             // Kırmızı
            _ => "#6c757d"
        };
    }

    public bool Equals(ComplianceStatus? other)
    {
        if (other is null) return false;
        return Level == other.Level && CompliancePercentage == other.CompliancePercentage;
    }

    public override bool Equals(object? obj)
    {
        return obj is ComplianceStatus other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Level, CompliancePercentage);
    }

    public override string ToString()
    {
        return $"ComplianceStatus(Level:{Level}, Percentage:{CompliancePercentage}%)";
    }
}
