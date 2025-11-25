using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Repositiories;

public class PriceRepository(AppDbContext context) : BaseRepository<Price, int, AppDbContext>(context), IPriceRepository
{
    public override async Task<List<Price>> GetAllAsync(CancellationToken ct = default) =>
        await _dbSet.Include(d => d.Part).ToListAsync(ct);

    public override async Task<Price?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _dbSet.Include(d => d.Part).FirstOrDefaultAsync(it => it.Id!.Equals(id), ct);

}
