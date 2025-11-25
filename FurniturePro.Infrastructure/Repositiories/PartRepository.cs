using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Repositiories;

public class PartRepository(AppDbContext context) : BaseRepository<Part, int, AppDbContext>(context), IPartRepository
{
    public override async Task<List<Part>> GetAllAsync(CancellationToken ct = default) =>
        await _dbSet.Include(d => d.Color).Include(d => d.Material).ToListAsync(ct);

    public override async Task<Part?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _dbSet.Include(d => d.Color).Include(d => d.Material).FirstOrDefaultAsync(it => it.Id!.Equals(id), ct);

}
