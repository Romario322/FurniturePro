using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Parts;

public class PartType : DictionaryEntity<int>
{
    public List<Part>? Parts { get; set; }
}
