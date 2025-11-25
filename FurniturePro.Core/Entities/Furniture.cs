using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class Furniture : DictionaryEntity<int>
{
    [Column(TypeName = "integer")]
    public int? Markup { get; set; }

    [Column(TypeName = "boolean")]
    public bool Activity { get; set; }

    public List<FurnitureComposition>? FurnitureCompositions { get; set; }

    public List<OrderComposition>? OrderCompositions { get; set; }

    [Column(TypeName = "integer")]
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}
