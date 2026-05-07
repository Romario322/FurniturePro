using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Constructor;

public class ReplacementGroup : DictionaryEntity<int>
{
    public List<ReplacementGroupItem>? ReplacementGroupItems { get; set; }
    public List<FurniturePart>? FurnitureParts { get; set; }

    public override string ToString()
    {
        return $"Группа замен: {Name}";
    }
}
