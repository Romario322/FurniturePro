using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Enums;

namespace FurniturePro.Core.Entities.Constructor;

public class PartRole : DictionaryEntity<PartRoleEnum>
{
    public required string LengthFormula { get; set; }
    public required string WidthFormula { get; set; }

    public List<FurniturePart>? FurnitureParts { get; set; }
}
