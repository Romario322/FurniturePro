using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Parts.Update;

public class UpdatePartDto : UpdateCatalogDto
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