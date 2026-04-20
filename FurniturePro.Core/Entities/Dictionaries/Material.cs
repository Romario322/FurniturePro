using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Dictionaries;

public class Material : DictionaryEntity<int>
{
    public List<Part>? Parts { get; set; }
}
