using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Repositiories;

public abstract class BaseRepository<TEntity, TId, TDbContext> : IBaseRepository<TEntity, TId>
where TEntity : class, IEntity<TId>
where TDbContext : DbContext
{
    protected readonly TDbContext _context;

    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(TDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken ct = default) =>
        await _dbSet.ToListAsync(ct);

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default) =>
        await _dbSet.FirstOrDefaultAsync(it => it.Id!.Equals(id), ct);

    public virtual async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default)
    {
        var entity = await _dbSet.OrderByDescending(it => it.UpdateDate).FirstOrDefaultAsync(ct) ??
            throw new Exception($"Сущность типа {typeof(TEntity)} не существует.");

        return entity.UpdateDate;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.Now;
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.Now;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public virtual async Task DeleteByIdAsync(TId id, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(id, ct) ??
            throw new Exception($"Сущность типа {typeof(TEntity)} с id {id} не существует.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}
