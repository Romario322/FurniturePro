using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class StatusChangeRepository(AppDbContext context) : BaseConnectionRepository<StatusChange, int, Order, Status, AppDbContext>(context), IStatusChangeRepository
{

    public override async Task<StatusChange> CreateAsync(StatusChange entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.UtcNow;
        entity.Date = entity.Date.ToUniversalTime();
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return entity;
    }

    public override async Task UpdateAsync(StatusChange entity, CancellationToken ct = default)
    {
        entity.UpdateDate = DateTime.UtcNow;
        entity.Date = entity.Date.ToUniversalTime();
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
    }
}
