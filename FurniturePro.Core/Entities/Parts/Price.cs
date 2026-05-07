using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.System;

namespace FurniturePro.Core.Entities.Parts;

public class Price : BaseEntity<int>
{
    public required decimal Value { get; set; }

    public required DateTime Date { get; set; }

    public required int PartId { get; set; }
    public Part? Part { get; set; }

    public required int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public override string ToString()
    {
        return $"Цена {Value} руб. от {Date:dd.MM.yyyy} (Деталь ID: {PartId}, Установил: ID {EmployeeId})";
    }
}