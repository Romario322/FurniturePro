using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Models.Dto.Parts.Create;
using FurniturePro.Core.Models.Dto.Parts.Read;
using FurniturePro.Core.Models.Dto.Parts.Update;

namespace FurniturePro.Core.Interfaces.Services.Parts;

public interface IColorService : IBaseService<int, ColorDto, CreateColorDto, UpdateColorDto>
{
}
