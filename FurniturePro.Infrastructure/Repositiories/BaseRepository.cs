using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
        await _dbSet.AsNoTracking().ToListAsync(ct);

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking().FirstOrDefaultAsync(it => it.Id!.Equals(id), ct);

    public virtual async Task<List<TEntity>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var date = DateTime.ParseExact(
            dateTime,
            "yyyy-MM-dd'T'HH:mm:ss.ffffff",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
        );
        return await _dbSet.AsNoTracking().Where(ent => ent.UpdateDate > date).ToListAsync(ct);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.UtcNow;
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return entity;
    }

    public virtual async Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities, CancellationToken ct = default)
    {
        entities.ForEach(ent => ent.UpdateDate = DateTime.UtcNow);
        await _context.AddRangeAsync(entities, ct);
        await _context.SaveChangesAsync(ct);

        return entities;
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.UtcNow;
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
