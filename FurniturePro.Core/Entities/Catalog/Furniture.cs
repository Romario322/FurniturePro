using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Entities.Orders;

namespace FurniturePro.Core.Entities.Catalog;

public class Furniture : CatalogEntity<int>
{
    public required int BaseWidth { get; set; }
    public required int BaseHeight { get; set; }
    public required int BaseDepth { get; set; }
    public required int Markup { get; set; }
    public bool Activity { get; set; }

    public required int FurnitureCategoryId { get; set; }
    public FurnitureCategory? FurnitureCategory { get; set; }

    public List<FurniturePart>? FurnitureParts { get; set; }
    public List<OrderComposition>? OrderCompositions { get; set; }

    public override string ToString()
    {
        return $"Мебель: {Name} (Артикул: {Sku})";
    }
}
