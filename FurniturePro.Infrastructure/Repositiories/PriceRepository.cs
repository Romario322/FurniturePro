using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class PriceRepository(AppDbContext context) : BaseRepository<Price, int, AppDbContext>(context), IPriceRepository
{

    public override async Task<Price> CreateAsync(Price entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.UtcNow;
        entity.Date = entity.Date.ToUniversalTime();
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return entity;
    }

    public override async Task UpdateAsync(Price entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.UtcNow;
        entity.Date = entity.Date.ToUniversalTime();
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

}
