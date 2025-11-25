using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Dictionaries;

public class Category : DictionaryEntity<int>
{
    public List<Furniture>? Furnitures { get; set; }
}
