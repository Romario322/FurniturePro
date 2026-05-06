using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Interfaces.Repositories.Parts;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Parts;

public class PriceRepository(AppDbContext context)
    : BaseRepository<Price, int, AppDbContext>(context), IPriceRepository
{ }
