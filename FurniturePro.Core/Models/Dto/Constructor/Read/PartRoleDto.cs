using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.Constructor.Read;

public class PartRoleDto : DictionaryDto<PartRoleEnum>
{
    public required string LengthFormula { get; set; }
    public required string WidthFormula { get; set; }
}