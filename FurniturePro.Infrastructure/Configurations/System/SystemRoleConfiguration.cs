using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Enums;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.System;

internal class SystemRoleConfiguration : DictionaryEntityConfiguration<SystemRole, SystemRoleEnum>
{
    public override void Configure(EntityTypeBuilder<SystemRole> builder)
    {
        base.Configure(builder);

        builder.ToTable("SystemRoles");

        builder.HasMany(e => e.Employees)
               .WithOne(emp => emp.SystemRole)
               .HasForeignKey(emp => emp.SystemRoleId)
               .OnDelete(DeleteBehavior.Restrict);

        var seedDate = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc);

        builder.HasData(
            new SystemRole
            {
                Id = SystemRoleEnum.Administrator,
                Name = "Администратор",
                Description = "Полный доступ к системе",
                UpdateDate = seedDate
            },
            new SystemRole
            {
                Id = SystemRoleEnum.SalesManager,
                Name = "Менеджер по продажам",
                Description = "Работа с клиентами, заказами и ценами",
                UpdateDate = seedDate
            },
            new SystemRole
            {
                Id = SystemRoleEnum.Constructor,
                Name = "Конструктор",
                Description = "Ведение каталога мебели и спецификаций",
                UpdateDate = seedDate
            },
            new SystemRole
            {
                Id = SystemRoleEnum.WorkshopMaster,
                Name = "Мастер цеха",
                Description = "Просмотр заказов, перевод статусов производства",
                UpdateDate = seedDate
            }
        );
    }
}