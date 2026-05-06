using FurniturePro.Core.Models.Dto.Abstractions.Create;

namespace FurniturePro.Core.Models.Dto.Constructor.Create;

public class CreatePartRoleDto : CreateDictionaryDto
{
    public required string LengthFormula { get; set; }
    public required string WidthFormula { get; set; }
}