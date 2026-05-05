using FurniturePro.Core.Entities.Users;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Users;

internal class SystemRoleConfiguration : DictionaryEntityConfiguration<SystemRole, int>
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
                Id = 1,
                Name = "Администратор",
                Description = "Полный доступ к системе",
                UpdateDate = seedDate
            },
            new SystemRole
            {
                Id = 2,
                Name = "Менеджер по продажам",
                Description = "Работа с клиентами, заказами и ценами",
                UpdateDate = seedDate
            },
            new SystemRole
            {
                Id = 3,
                Name = "Конструктор",
                Description = "Ведение каталога мебели и спецификаций",
                UpdateDate = seedDate
            },
            new SystemRole
            {
                Id = 4,
                Name = "Мастер цеха",
                Description = "Просмотр заказов, перевод статусов производства",
                UpdateDate = seedDate
            }
        );
    }
}
