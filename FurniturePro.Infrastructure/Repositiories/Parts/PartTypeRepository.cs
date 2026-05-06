using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.Parts;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Parts;

public class PartTypeRepository(AppDbContext context)
    : BaseRepository<PartType, PartTypeEnum, AppDbContext>(context), IPartTypeRepository
{ }
