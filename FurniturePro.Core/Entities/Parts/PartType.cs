using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Enums;

namespace FurniturePro.Core.Entities.Parts;

public class PartType : DictionaryEntity<PartTypeEnum>
{
    public List<Part>? Parts { get; set; }

    public override string ToString()
    {
        return $"Тип детали: {Name}";
    }
}
