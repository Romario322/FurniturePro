using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Repositiories;

public class OrderRepository(AppDbContext context) : BaseRepository<Order, int, AppDbContext>(context), IOrderRepository
{
    public override async Task<List<Order>> GetAllAsync(CancellationToken ct = default) =>
        await _dbSet.Include(d => d.OrderCompositions)!.ThenInclude(d => d.Entity2).Include(d => d.StatusChanges)!.ThenInclude(d => d.Entity2).ToListAsync(ct);

    public override async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _dbSet.Include(d => d.OrderCompositions)!.ThenInclude(d => d.Entity2).Include(d => d.StatusChanges)!.ThenInclude(d => d.Entity2).FirstOrDefaultAsync(it => it.Id!.Equals(id), ct);
}
