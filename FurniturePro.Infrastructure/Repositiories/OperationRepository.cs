using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class OperationRepository(AppDbContext context) : BaseRepository<Operation, int, AppDbContext>(context), IOperationRepository
{

}
