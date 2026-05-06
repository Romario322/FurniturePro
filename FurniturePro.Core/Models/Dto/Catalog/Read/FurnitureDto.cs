using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.Catalog.Read;

public class FurnitureDto : CatalogDto<int>
{
    public required int BaseWidth { get; set; }
    public required int BaseHeight { get; set; }
    public required int BaseDepth { get; set; }
    public required int Markup { get; set; }
    public bool Activity { get; set; }

    public required int FurnitureCategoryId { get; set; }
}