using FurniturePro.Core.Entities;

namespace FurniturePro.Core.Repositories;

public interface IDeletedIdRepository : IBaseRepository<DeletedId, int>
{
    Task<List<DeletedId>> GetAllAsync(string tableName, CancellationToken ct = default);

    Task<List<DeletedId>> GetAfterDateAsync(string dateTime, string tableName, CancellationToken ct = default);
}
