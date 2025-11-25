using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Repositories;

public interface IBaseConnectionRepository<TConnectionEntity, in TId, in TEntity1, in TEntity2> where TConnectionEntity : class, IConnectionEntity<TId, TEntity1, TEntity2>
{
    Task<List<TConnectionEntity>> GetAllAsync(CancellationToken ct = default);

    Task<TConnectionEntity?> GetByIdsAsync(TId id1, TId id2, CancellationToken ct = default);

    Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default);

    Task<TConnectionEntity> CreateAsync(TConnectionEntity entity, CancellationToken ct = default);

    Task UpdateAsync(TConnectionEntity entity, CancellationToken ct = default);

    Task DeleteByIdsAsync(TId id1, TId id2, CancellationToken ct = default);
}
