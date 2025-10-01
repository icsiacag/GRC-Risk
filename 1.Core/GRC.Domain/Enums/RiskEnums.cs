namespace GRC.Domain.Enums;

/// <summary>
/// Risk tipi enumerasyonu.
/// Riskin hangi kategoride olduğunu belirtir.
/// </summary>
public enum RiskType
{
    /// <summary>
    /// Stratejik riskler - organizasyonun stratejik hedeflerini etkiler.
    /// Örnek: Pazar payı kaybı, rekabet artışı
    /// </summary>
    Strategic = 1,

    /// <summary>
    /// Operasyonel riskler - günlük operasyonlarla ilgili.
    /// Örnek: Sistem arızası, insan hatası
    /// </summary>
    Operational = 2,

    /// <summary>
    /// Finansal riskler - mali kayıplara yol açabilir.
    /// Örnek: Kur riski, kredi riski
    /// </summary>
    Financial = 3,

    /// <summary>
    /// Uyumluluk (compliance) riskleri - yasal/düzenleyici gereklilikler.
    /// Örnek: GDPR ihlali, ISO sertifikasyon kaybı
    /// </summary>
    Compliance = 4,

    /// <summary>
    /// İtibar (reputational) riskleri - kurumsal imaja zarar.
    /// Örnek: Medya skandalı, müşteri şikayetleri
    /// </summary>
    Reputational = 5,

    /// <summary>
    /// Teknolojik riskler - IT ve teknoloji altyapısı.
    /// Örnek: Siber saldırı, veri kaybı
    /// </summary>
    Technological = 6,

    /// <summary>
    /// Çevresel riskler - doğal afetler ve çevre faktörleri.
    /// Örnek: Deprem, sel
    /// </summary>
    Environmental = 7,

    /// <summary>
    /// İnsan kaynakları riskleri - personel ile ilgili.
    /// Örnek: Yetenek kaybı, grev
    /// </summary>
    HumanResources = 8
}

/// <summary>
/// Risk durumu yaşam döngüsü.
/// </summary>
public enum RiskStatus
{
    /// <summary>
    /// Risk tanımlanmış ama henüz değerlendirilmemiş.
    /// </summary>
    Identified = 1,

    /// <summary>
    /// Risk değerlendirilmiş (scoring yapılmış).
    /// </summary>
    Assessed = 2,

    /// <summary>
    /// Azaltım planı devam ediyor.
    /// </summary>
    InMitigation = 3,

    /// <summary>
    /// Risk kabul edilmiş (accept edilmiş).
    /// </summary>
    Accepted = 4,

    /// <summary>
    /// Risk transfer edilmiş (örn: sigorta).
    /// </summary>
    Transferred = 5,

    /// <summary>
    /// Risk kapatılmış (artık geçerli değil).
    /// </summary>
    Closed = 6
}

/// <summary>
/// Risk seviyesi (hesaplanmış skordan belirlenir).
/// </summary>
public enum RiskLevel
{
    /// <summary>
    /// Düşük risk (1-25 puan).
    /// Kabul edilebilir, rutin kontrol yeterli.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Orta risk (26-50 puan).
    /// Yönetim farkındalığı gerekli, kontrol planla.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// Yüksek risk (51-75 puan).
    /// Acil aksiyon gerekli, üst yönetim bilgilendirme.
    /// </summary>
    High = 3,

    /// <summary>
    /// Kritik risk (76-100 puan).
    /// Anında müdahale, yönetim kurulu bilgilendirme zorunlu.
    /// </summary>
    Critical = 4
}
