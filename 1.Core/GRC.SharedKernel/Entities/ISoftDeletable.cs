namespace GRC.SharedKernel.Entities;

/// <summary>
/// Soft delete (mantıksal silme) özelliği sağlayan interface.
/// GRC sisteminde compliance gereği veriler fiziksel olarak silinmez.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Entity'nin silinip silinmediğini belirtir.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Entity'nin silinme tarihi.
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// Entity'yi silen kullanıcının ID'si.
    /// </summary>
    string? DeletedBy { get; }

    /// <summary>
    /// Entity'yi soft delete yapar.
    /// </summary>
    void Delete(string userId);

    /// <summary>
    /// Silinen entity'yi geri yükler.
    /// </summary>
    void Restore();
}
