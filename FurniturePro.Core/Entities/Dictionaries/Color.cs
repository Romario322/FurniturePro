using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Dictionaries;

public class Color : DictionaryEntity<int>
{
    public List<Part>? Parts { get; set; }
}
