using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Repositiories;

public class CountRepository(AppDbContext context) : BaseRepository<Count, int, AppDbContext>(context), ICountRepository
{
    public override async Task<List<Count>> GetAllAsync(CancellationToken ct = default) =>
        await _dbSet.Include(d => d.Part).ToListAsync(ct);

    public override async Task<Count?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _dbSet.Include(d => d.Part).FirstOrDefaultAsync(it => it.Id!.Equals(id), ct);

}
