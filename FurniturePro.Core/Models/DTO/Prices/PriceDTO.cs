using FurniturePro.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Models.DTO.Prices;

public class PriceDTO
{
    public required int Id { get; set; }

    public required string PartName { get; set; }
    public required int PartId { get; set; }
    public required int Value { get; set; }
    public required DateTime Date { get; set; }
}
