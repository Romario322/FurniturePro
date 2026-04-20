using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class OperationTypeRepository(AppDbContext context) : BaseRepository<OperationType, int, AppDbContext>(context), IOperationTypeRepository
{

}
