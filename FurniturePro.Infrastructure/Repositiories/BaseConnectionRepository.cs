using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
        await _dbSet.AsNoTracking().ToListAsync(ct);

    public virtual async Task<TConnectionEntity?> GetByIdsAsync(TId id1, TId id2, CancellationToken ct = default) =>
        await _dbSet.AsNoTracking().FirstOrDefaultAsync(it => it.Entity1Id!.Equals(id1) && it.Entity2Id!.Equals(id2), ct);

    public virtual async Task<List<TConnectionEntity>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var date = DateTime.ParseExact(
            dateTime,
            "yyyy-MM-dd'T'HH:mm:ss.ffffff",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
        );
        date = date.AddTicks(9);
        return await _dbSet.AsNoTracking().Where(ent => ent.UpdateDate > date).ToListAsync(ct);
    }

    public virtual async Task<TConnectionEntity> CreateAsync(TConnectionEntity entity, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        entity.UpdateDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, DateTimeKind.Utc);
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return entity;
    }

    public virtual async Task<List<TConnectionEntity>> CreateRangeAsync(List<TConnectionEntity> entities, CancellationToken ct = default)
    {
        entities.ForEach(ent => ent.UpdateDate = DateTime.UtcNow);
        await _context.AddRangeAsync(entities, ct);
        await _context.SaveChangesAsync(ct);

        return entities;
    }

    public virtual async Task UpdateAsync(TConnectionEntity entity, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        entity.UpdateDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, DateTimeKind.Utc);
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public virtual async Task UpdateRangeAsync(List<TConnectionEntity> entities, CancellationToken ct = default)
    {
        entities.ForEach(ent => ent.UpdateDate = DateTime.UtcNow);
        _context.UpdateRange(entities);
        await _context.SaveChangesAsync(ct);
    }

    public virtual async Task DeleteByIdsAsync(TId id1, TId id2, CancellationToken ct = default)
    {
        var entity = await GetByIdsAsync(id1, id2, ct) ??
            throw new Exception($"Сущность типа {typeof(TConnectionEntity)} связывающая сущность типа {typeof(TEntity1)} с id {id1} и сущность типа {typeof(TEntity2)} с id {id2} не существует.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }


    public async Task DeleteRangeByIdsAsync(IEnumerable<(TId Id1, TId Id2)> ids, CancellationToken ct = default)
    {
        // 1. Быстрая проверка
        if (ids == null || !ids.Any()) return;

        // 2. Группируем удаление по Id Мебели (Id1), чтобы делать меньше запросов
        var groupedByFurniture = ids.GroupBy(x => x.Id1);

        var listToDelete = new List<TConnectionEntity>();

        foreach (var group in groupedByFurniture)
        {
            var furnitureId = group.Key;
            // Собираем ID деталей, которые надо удалить у этой мебели
            var partsToRemove = group.Select(x => x.Id2).ToHashSet();

            // 4. ЗАГРУЗКА
            // Загружаем ВСЕ детали для этой мебели. 
            // Используем конкретные свойства IdFurniture/IdPart (или Entity1Id/Entity2Id, проверьте вашу сущность)
            // Это решает проблему трансляции типов, так как EF точно знает, что это int.
            var existingCompositions = await _dbSet.AsNoTracking()
                .Where(x => x.Entity1Id!.Equals(furnitureId))
                .ToListAsync(ct);

            // 5. ФИЛЬТРАЦИЯ В ПАМЯТИ
            // Находим те записи, IdPart которых есть в списке на удаление
            foreach (var item in existingCompositions)
            {
                if (partsToRemove.Contains(item.Entity2Id))
                {
                    listToDelete.Add(item);
                }
            }
        }

        // 6. УДАЛЕНИЕ
        if (listToDelete.Any())
        {
            _dbSet.RemoveRange(listToDelete);
            await _context.SaveChangesAsync(ct);
        }
    }
}
