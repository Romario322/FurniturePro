using FurniturePro.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Models.DTO.Counts;

public class CountDTO
{
    public required int Id { get; set; }

    public required string PartName { get; set; }
    public required int PartId { get; set; }
    public required int Remaining { get; set; }
    public required int WrittenOff { get; set; }
    public required int Updated { get; set; }
    public required DateTime Date { get; set; }
}
