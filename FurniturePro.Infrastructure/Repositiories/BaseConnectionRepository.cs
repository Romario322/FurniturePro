using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Repositiories;

public abstract class BaseConnectionRepository<TConnectionEntity, TId, TEntity1, TEntity2, TDbContext> : IBaseConnectionRepository<TConnectionEntity, TId, TEntity1, TEntity2>
where TConnectionEntity : class, IConnectionEntity<TId, TEntity1, TEntity2>
where TDbContext : DbContext
{
    protected readonly TDbContext _context;

    protected readonly DbSet<TConnectionEntity> _dbSet;

    protected BaseConnectionRepository(TDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TConnectionEntity>();
    }

    public virtual async Task<List<TConnectionEntity>> GetAllAsync(CancellationToken ct = default) =>
        await _dbSet.ToListAsync(ct);

    public virtual async Task<TConnectionEntity?> GetByIdsAsync(TId id1, TId id2, CancellationToken ct = default) =>
        await _dbSet.FirstOrDefaultAsync(it => it.Entity1Id!.Equals(id1) && it.Entity2Id!.Equals(id2), ct);

    public virtual async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default)
    {
        var entity = await _dbSet.OrderByDescending(it => it.UpdateDate).FirstOrDefaultAsync(ct) ??
            throw new Exception($"Сущность типа {typeof(TConnectionEntity)} не существует.");

        return entity.UpdateDate;
    }

    public virtual async Task<TConnectionEntity> CreateAsync(TConnectionEntity entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.Now;
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return entity;
    }

    public virtual async Task UpdateAsync(TConnectionEntity entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.Now;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public virtual async Task DeleteByIdsAsync(TId id1, TId id2, CancellationToken ct = default)
    {
        var entity = await GetByIdsAsync(id1, id2, ct) ??
            throw new Exception($"Сущность типа {typeof(TConnectionEntity)} связывающая сущность типа {typeof(TEntity1)} с id {id1} и сущность типа {typeof(TEntity2)} с id {id2} не существует.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}
