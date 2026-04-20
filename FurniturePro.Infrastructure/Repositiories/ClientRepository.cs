using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class ClientRepository(AppDbContext context) : BaseRepository<Client, int, AppDbContext>(context), IClientRepository
{

}
