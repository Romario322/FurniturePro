using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Constructors;

public class PartRole : DictionaryEntity<int>
{
    public required string LengthFormula { get; set; }
    public required string WidthFormula { get; set; }

    public List<FurniturePart>? FurnitureParts { get; set; }
}
