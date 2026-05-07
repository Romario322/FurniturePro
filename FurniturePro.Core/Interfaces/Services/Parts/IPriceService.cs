using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Models.Dto.Parts.Create;
using FurniturePro.Core.Models.Dto.Parts.Read;
using FurniturePro.Core.Models.Dto.Parts.Update;

namespace FurniturePro.Core.Interfaces.Services.Parts;

public interface IPriceService : IBaseService<int, PriceDto, CreatePriceDto, UpdatePriceDto>
{
}
