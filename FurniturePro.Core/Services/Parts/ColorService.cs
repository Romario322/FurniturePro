using AutoMapper;
using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Interfaces.Repositories.Parts;
using FurniturePro.Core.Interfaces.Services.Parts;
using FurniturePro.Core.Models.Dto.Parts.Create;
using FurniturePro.Core.Models.Dto.Parts.Read;
using FurniturePro.Core.Models.Dto.Parts.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.Parts;

public class ColorService : BaseService<Color, int, ColorDto, CreateColorDto, UpdateColorDto>, IColorService
{
    public ColorService(IColorRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
