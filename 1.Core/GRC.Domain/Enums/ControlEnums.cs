namespace GRC.Domain.Enums;

/// <summary>
/// Kontrol tipi - COSO framework standardı.
/// </summary>
public enum ControlType
{
    /// <summary>
    /// Önleyici (Preventive) kontrol - riski oluşmadan önler.
    /// Örnek: Firewall, şifre politikası, access control
    /// </summary>
    Preventive = 1,

    /// <summary>
    /// Tespit edici (Detective) kontrol - riski sonradan tespit eder.
    /// Örnek: Log monitoring, audit trail, reconciliation
    /// </summary>
    Detective = 2,

    /// <summary>
    /// Düzeltici (Corrective) kontrol - oluşan zararı düzeltir.
    /// Örnek: Backup restore, incident response plan
    /// </summary>
    Corrective = 3,

    /// <summary>
    /// Direktif (Directive) kontrol - yönlendirici/eğitici.
    /// Örnek: Politika dökümanı, training programs
    /// </summary>
    Directive = 4
}

/// <summary>
/// Kontrol etkinliği - test sonuçlarından belirlenir.
/// </summary>
public enum ControlEffectiveness
{
    /// <summary>
    /// Etkisiz - kontrol işlevini yerine getirmiyor.
    /// Risk azaltımında katkı sağlamıyor (0% reduction).
    /// </summary>
    Ineffective = 0,

    /// <summary>
    /// Kısmen etkili - kontrol kısmen çalışıyor.
    /// Sınırlı risk azaltımı sağlıyor (10% reduction).
    /// </summary>
    PartiallyEffective = 1,

    /// <summary>
    /// Etkili - kontrol beklenen şekilde çalışıyor.
    /// Önemli risk azaltımı sağlıyor (25% reduction).
    /// </summary>
    Effective = 2,

    /// <summary>
    /// Çok etkili - kontrol mükemmel çalışıyor.
    /// Maksimum risk azaltımı sağlıyor (40% reduction).
    /// </summary>
    HighlyEffective = 3
}

/// <summary>
/// Kontrol durumu.
/// </summary>
public enum ControlStatus
{
    /// <summary>
    /// Planlanmış - henüz devreye alınmamış.
    /// </summary>
    Planned = 1,

    /// <summary>
    /// Aktif - kontrol çalışıyor.
    /// </summary>
    Active = 2,

    /// <summary>
    /// Devre dışı - geçici olarak kapalı.
    /// </summary>
    Inactive = 3,

    /// <summary>
    /// İptal edilmiş - artık kullanılmıyor.
    /// </summary>
    Cancelled = 4
}
