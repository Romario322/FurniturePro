using AutoMapper;
using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Interfaces.Repositories.Parts;
using FurniturePro.Core.Interfaces.Services.Parts;
using FurniturePro.Core.Models.Dto.Parts.Create;
using FurniturePro.Core.Models.Dto.Parts.Read;
using FurniturePro.Core.Models.Dto.Parts.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.Parts;

public class MaterialService : BaseService<Material, int, MaterialDto, CreateMaterialDto, UpdateMaterialDto>, IMaterialService
{
    public MaterialService(IMaterialRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
