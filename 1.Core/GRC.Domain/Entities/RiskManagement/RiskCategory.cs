using Ardalis.GuardClauses;
using GRC.SharedKernel.Entities;
using System;

namespace GRC.Domain.Entities.RiskManagement;

/// <summary>
/// Risk kategorisi entity.
/// Riskler kategori bazında gruplandırılır.
/// Örnek: Operasyonel, Finansal, Stratejik, Uyumluluk
/// </summary>
public class RiskCategory : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Kategori adı (benzersiz olmalı).
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Kategori açıklaması.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Kategori kodu (raporlama için).
    /// Örnek: OPS, FIN, STR
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Kategori rengi (UI'da görsel ayrım için).
    /// Hex format: #FF5733
    /// </summary>
    public string? ColorCode { get; private set; }

    /// <summary>
    /// Kategorinin aktif olup olmadığı.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Kategori sırası (gösterim için).
    /// </summary>
    public int DisplayOrder { get; private set; }

    /// <summary>
    /// Üst kategori (hiyerarşi için - opsiyonel).
    /// </summary>
    public Guid? ParentCategoryId { get; private set; }

    /// <summary>
    /// Bu kategoriye ait riskler.
    /// Navigation property.
    /// </summary>
    public ICollection<Risk> Risks { get; private set; } = new List<Risk>();

    // EF Core için
    private RiskCategory() : base() { }

    private RiskCategory(
        string name,
        string description,
        string code,
        int displayOrder,
        string? colorCode = null,
        Guid? parentCategoryId = null) : base()
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(code, nameof(code));

        Name = name;
        Description = description;
        Code = code.ToUpperInvariant();
        DisplayOrder = displayOrder;
        ColorCode = colorCode;
        ParentCategoryId = parentCategoryId;
    }

    public static RiskCategory Create(
        string name,
        string description,
        string code,
        int displayOrder,
        string? colorCode = null,
        Guid? parentCategoryId = null)
    {
        return new RiskCategory(name, description, code, displayOrder, colorCode, parentCategoryId);
    }

    public void Update(string name, string description, string? colorCode = null)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
        Description = description;
        ColorCode = colorCode;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateDisplayOrder(int order)
    {
        if (order < 0)
            throw new ArgumentException("Display order cannot be negative", nameof(order));

        DisplayOrder = order;
    }
}
