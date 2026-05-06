using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Constructor.Update;

public class UpdateFurniturePartDto : UpdateBaseDto<int>
{
    public required int Quantity { get; set; }

    public required int FurnitureId { get; set; }
    public required int PartRoleId { get; set; }
    public required int ReplacementGroupId { get; set; }
}