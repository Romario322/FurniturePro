namespace FurniturePro.Core.Models.DTO.DeletedIds;

public class DeletedIdDTO
{
    public required int Id { get; set; }

    public required string TableName { get; set; }
    public required int EntityId { get; set; }
    public required DateTime UpdateDate { get; set; }
}
