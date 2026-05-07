using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Parts;

public class Material : DictionaryEntity<int>
{
    public List<Part>? Parts { get; set; }

    public override string ToString()
    {
        return $"Материал: {Name}";
    }
}
