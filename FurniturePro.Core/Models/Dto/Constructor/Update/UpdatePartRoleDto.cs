using FurniturePro.Core.Enums;
using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Constructor.Update;

public class UpdatePartRoleDto : UpdateDictionaryDto<PartRoleEnum>
{
    public required string LengthFormula { get; set; }
    public required string WidthFormula { get; set; }
}