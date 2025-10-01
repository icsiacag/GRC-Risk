namespace GRC.Domain.Enums;

/// <summary>
/// Uyum seviyesi.
/// </summary>
public enum ComplianceLevel
{
    /// <summary>
    /// Tam uyumlu - tüm gereklilikler karşılanıyor (95-100%).
    /// </summary>
    FullyCompliant = 1,

    /// <summary>
    /// Büyük ölçüde uyumlu - çoğu gereklilik karşılanıyor (80-94%).
    /// </summary>
    SubstantiallyCompliant = 2,

    /// <summary>
    /// Kısmen uyumlu - bazı gereklilikler eksik (60-79%).
    /// </summary>
    PartiallyCompliant = 3,

    /// <summary>
    /// Uyumsuz - önemli eksiklikler var (&lt;60%).
    /// </summary>
    NonCompliant = 4
}

/// <summary>
/// Uyum çerçevesi tipleri.
/// </summary>
public enum ComplianceFrameworkType
{
    /// <summary>
    /// ISO 27001 - Bilgi güvenliği yönetim sistemi.
    /// </summary>
    ISO27001 = 1,

    /// <summary>
    /// SOC 2 - Service Organization Control.
    /// </summary>
    SOC2 = 2,

    /// <summary>
    /// GDPR - General Data Protection Regulation (Avrupa).
    /// </summary>
    GDPR = 3,

    /// <summary>
    /// KVKK - Kişisel Verilerin Korunması Kanunu (Türkiye).
    /// </summary>
    KVKK = 4,

    /// <summary>
    /// PCI DSS - Payment Card Industry Data Security Standard.
    /// </summary>
    PCIDSS = 5,

    /// <summary>
    /// HIPAA - Health Insurance Portability and Accountability Act.
    /// </summary>
    HIPAA = 6,

    /// <summary>
    /// ISO 9001 - Kalite yönetim sistemi.
    /// </summary>
    ISO9001 = 7,

    /// <summary>
    /// NIST Cybersecurity Framework.
    /// </summary>
    NIST = 8,

    /// <summary>
    /// Custom - Organizasyona özel framework.
    /// </summary>
    Custom = 99
}
