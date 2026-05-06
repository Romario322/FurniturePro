using FurniturePro.Core.Models.Dto.Abstractions.Create;

namespace FurniturePro.Core.Models.Dto.Parts.Create;

public class CreatePartDto : CreateCatalogDto
{
    public int? Thickness { get; set; }
    public int? WasteCoefficient { get; set; }
    public required decimal WeightPerUnit { get; set; }
    public bool Activity { get; set; }

    public required int MaterialId { get; set; }
    public required int ColorId { get; set; }
    public required int PartTypeId { get; set; }
    public required int PartCategoryId { get; set; }
}