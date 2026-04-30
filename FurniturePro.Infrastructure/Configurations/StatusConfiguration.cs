using FurniturePro.Core.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FurniturePro.Infrastructure.Configurations;

internal class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("statuses");

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasKey(et => et.Id);

        builder.HasMany(et => et.StatusChanges)
               .WithOne(e => e.Entity2)
               .HasForeignKey(e => e.Entity2Id)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(et => et.UpdateDate);

        // --- SEED DATA (Начальные данные) ---
        // Фиксируем дату, чтобы EF Core не пытался обновлять записи при каждом запуске из-за разницы во времени
        var defaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            // Основная ветка (1-8)
            new Status { Id = 1, Name = "Создан", UpdateDate = defaultDate },
            new Status { Id = 2, Name = "В обработке", UpdateDate = defaultDate },
            new Status { Id = 3, Name = "Ожидает оплаты", UpdateDate = defaultDate },
            new Status { Id = 4, Name = "Оплачен", UpdateDate = defaultDate },
            new Status { Id = 5, Name = "Собирается", UpdateDate = defaultDate },
            new Status { Id = 6, Name = "Передан в доставку", UpdateDate = defaultDate },
            new Status { Id = 7, Name = "В пути", UpdateDate = defaultDate },
            new Status { Id = 8, Name = "Доставлен", UpdateDate = defaultDate },

            // Терминальный статус
            new Status { Id = 9, Name = "Завершен", UpdateDate = defaultDate },

            // Побочные статусы и отмены (10+)
            new Status { Id = 10, Name = "Отменен", UpdateDate = defaultDate },
            new Status { Id = 11, Name = "Средства возвращены", UpdateDate = defaultDate },
            new Status { Id = 12, Name = "Возврат на склад", UpdateDate = defaultDate }
        );
    }
}