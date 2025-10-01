namespace GRC.SharedKernel.Entities;

/// <summary>
/// Aggregate root marker interface.
/// Repository'ler sadece aggregate root'lar için oluşturulur.
/// </summary>
public interface IAggregateRoot
{
    // Marker interface - implementation yok
    // Amacı: Repository'lerde generic constraint olarak kullanım
    // Örnek: IRepository<T> where T : IAggregateRoot
}
