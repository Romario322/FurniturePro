using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Constructors;
using FurniturePro.Core.Entities.Orders;

namespace FurniturePro.Core.Entities.Parts;

public class Part : CatalogEntity<int>
{
    public int? Thickness { get; set; }
    public int? WasteCoefficient { get; set; }
    public required decimal WeightPerUnit { get; set; }
    public bool Activity { get; set; }

    public required int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required int ColorId { get; set; }
    public Color? Color { get; set; }
    public required int PartTypeId { get; set; }
    public PartType? PartType { get; set; }
    public required int PartCategoryId { get; set; }
    public PartCategory? PartCategory { get; set; }

    public List<ReplacementGroupItem>? ReplacementGroupItems { get; set; }
    public List<Price>? Prices { get; set; }
    public List<OrderPartDetail>? OrderPartDetails { get; set; }
}