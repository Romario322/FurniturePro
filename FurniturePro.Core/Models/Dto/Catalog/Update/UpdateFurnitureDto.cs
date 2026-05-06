using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Catalog.Update;

public class UpdateFurnitureDto : UpdateCatalogDto<int>
{
    public required int BaseWidth { get; set; }
    public required int BaseHeight { get; set; }
    public required int BaseDepth { get; set; }
    public required int Markup { get; set; }
    public bool Activity { get; set; }

    public required int FurnitureCategoryId { get; set; }
}