using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FurniturePro.Infrastructure.Repositiories;

public class DeletedIdRepository(AppDbContext context) : BaseRepository<DeletedId, int, AppDbContext>(context), IDeletedIdRepository
{
    public async Task<List<DeletedId>> GetAllAsync(string tableName, CancellationToken ct = default)
    {
        return await _dbSet.Where(ent => ent.TableName == tableName).ToListAsync(ct);
    }

    public async Task<List<DeletedId>> GetAfterDateAsync(string dateTime, string tableName, CancellationToken ct = default)
    {
        var date = DateTime.ParseExact(
            dateTime,
            "yyyy-MM-dd'T'HH:mm:ss.ffffff",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
        );
        return await _dbSet.AsNoTracking().Where(ent => ent.UpdateDate > date).ToListAsync(ct);
    }
}
