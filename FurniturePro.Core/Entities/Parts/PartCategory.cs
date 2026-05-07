using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Parts;

public class PartCategory : DictionaryEntity<int>
{
    public List<Part>? Parts { get; set; }

    public override string ToString()
    {
        return $"Категория деталей: {Name}";
    }
}
