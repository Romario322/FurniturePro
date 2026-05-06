using FurniturePro.Core.Entities.Abstractions;

namespace FurniturePro.Core.Entities.Catalog;

public class FurnitureCategory : DictionaryEntity<int>
{
    public List<Furniture>? Furnitures { get; set; }
}
