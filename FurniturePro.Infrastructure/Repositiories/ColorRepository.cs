using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class ColorRepository(AppDbContext context) : BaseRepository<Color, int, AppDbContext>(context), IColorRepository
{

}
