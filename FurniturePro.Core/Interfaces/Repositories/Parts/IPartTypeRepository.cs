using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.Abstractions;

namespace FurniturePro.Core.Interfaces.Repositories.Parts;

public interface IPartTypeRepository : IBaseRepository<PartType, PartTypeEnum> { }
