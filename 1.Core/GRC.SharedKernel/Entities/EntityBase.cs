using System;

namespace GRC.SharedKernel.Entities;

/// <summary>
/// Tüm entity'lerin türetileceği temel sınıf.
/// Her entity'nin benzersiz bir ID'si olmalıdır.
/// </summary>
/// <typeparam name="TId">ID'nin tipi (genellikle Guid veya int)</typeparam>
public abstract class EntityBase<TId> : IEquatable<EntityBase<TId>>
{
    /// <summary>
    /// Entity'nin benzersiz kimliği.
    /// Primary key olarak kullanılır.
    /// </summary>
    public TId Id { get; protected set; } = default!;

    /// <summary>
    /// Entity'nin oluşturulma tarihi.
    /// Database'de otomatik set edilir.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Entity'nin son güncellenme tarihi.
    /// Her update'te otomatik güncellenir.
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Entity'nin veritabanı kaydının versiyonu.
    /// Optimistic concurrency kontrolü için kullanılır.
    /// </summary>
    public byte[] RowVersion { get; protected set; } = Array.Empty<byte>();

    protected EntityBase()
    {
        // EF Core için parametre-siz constructor gerekli
        CreatedAt = DateTime.UtcNow;
    }

    protected EntityBase(TId id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// İki entity'nin ID'lerine göre eşitliğini kontrol eder.
    /// Domain'de entity identity önemlidir (not value equality).
    /// </summary>
    public bool Equals(EntityBase<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        // Transient entity'ler (henüz kaydedilmemiş) asla eşit değildir
        if (EqualityComparer<TId>.Default.Equals(Id, default) ||
            EqualityComparer<TId>.Default.Equals(other.Id, default))
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is EntityBase<TId> entity && Equals(entity);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id);
    }

    public static bool operator ==(EntityBase<TId>? left, EntityBase<TId>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EntityBase<TId>? left, EntityBase<TId>? right)
    {
        return !Equals(left, right);
    }
}

/// <summary>
/// Guid ID kullanan entity'ler için convenience class.
/// GRC sisteminde varsayılan olarak Guid kullanıyoruz (distributed systems için uygun).
/// </summary>
public abstract class EntityBase : EntityBase<Guid>
{
    protected EntityBase() : base()
    {
        Id = Guid.NewGuid(); // Yeni entity oluşturulduğunda otomatik ID
    }

    protected EntityBase(Guid id) : base(id)
    {
    }
}
