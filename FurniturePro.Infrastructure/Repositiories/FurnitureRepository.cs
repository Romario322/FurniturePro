using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Repositiories;

public class FurnitureRepository(AppDbContext context) : BaseRepository<Furniture, int, AppDbContext>(context), IFurnitureRepository
{
    public override async Task<List<Furniture>> GetAllAsync(CancellationToken ct = default) =>
        await _dbSet.Include(d => d.Category).Include(d => d.FurnitureCompositions)!.ThenInclude(d => d.Entity2).ToListAsync(ct);

    public override async Task<Furniture?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _dbSet.Include(d => d.Category).Include(d => d.FurnitureCompositions)!.ThenInclude(d => d.Entity2).FirstOrDefaultAsync(it => it.Id!.Equals(id), ct);
}
