using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Repositories;

public interface IBaseRepository<TEntity, in TId> where TEntity : class, IEntity<TId>
{
    Task<List<TEntity>> GetAllAsync(CancellationToken ct = default);

    Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task<List<TEntity>> GetAfterDateAsync(string dateTime, CancellationToken ct = default);

    Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);

    Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities, CancellationToken ct = default);

    Task UpdateAsync(TEntity entity, CancellationToken ct = default);

    Task DeleteByIdAsync(TId id, CancellationToken ct = default);
}
