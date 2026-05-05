using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Entities.Orders;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Furniture;

public class Furniture : DictionaryEntity<int>
{
    [Column(TypeName = "varchar(200)")]
    public required string Sku { get; set; }

    [Column(TypeName = "integer")]
    public required int BaseLength { get; set; }

    [Column(TypeName = "integer")]
    public required int BaseWidth { get; set; }

    [Column(TypeName = "integer")]
    public required int BaseDepth { get; set; }

    [Column(TypeName = "integer")]
    public required int Markup { get; set; }

    [Column(TypeName = "boolean")]
    public bool Activity { get; set; }


    [Column(TypeName = "integer")]
    public required int FurnitureCategoryId { get; set; }
    public FurnitureCategory? FurnitureCategory { get; set; }

    public List<FurniturePart>? FurnitureParts { get; set; }
    public List<OrderComposition>? OrderCompositions { get; set; }
}
