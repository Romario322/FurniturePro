using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Enums;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Constructor;

internal class PartRoleConfiguration : DictionaryEntityConfiguration<PartRole, PartRoleEnum>
{
    public override void Configure(EntityTypeBuilder<PartRole> builder)
    {
        base.Configure(builder);

        builder.ToTable("PartRoles");

        builder.Property(e => e.LengthFormula)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.WidthFormula)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(e => e.FurnitureParts)
               .WithOne(fp => fp.PartRole)
               .HasForeignKey(fp => fp.PartRoleId)
               .OnDelete(DeleteBehavior.Restrict);

        var seedDate = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc);

        builder.HasData(
            new PartRole
            {
                Id = PartRoleEnum.Sidewall,
                Name = "Боковина",
                LengthFormula = "H",
                WidthFormula = "W",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = PartRoleEnum.Top,
                Name = "Крышка",
                LengthFormula = "D",
                WidthFormula = "W",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = PartRoleEnum.Bottom,
                Name = "Дно",
                LengthFormula = "D - 2 * T",
                WidthFormula = "W",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = PartRoleEnum.Shelf,
                Name = "Полка",
                LengthFormula = "D - 2 * T",
                WidthFormula = "W - 20",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = PartRoleEnum.BackWall,
                Name = "Задняя стенка",
                LengthFormula = "H - 4",
                WidthFormula = "W - 4",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = PartRoleEnum.FacadeSingle,
                Name = "Фасад (Одинарный)",
                LengthFormula = "H - 4",
                WidthFormula = "W - 4",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = PartRoleEnum.FacadeDouble,
                Name = "Фасад (Двойной распашной)",
                LengthFormula = "H - 4",
                WidthFormula = "W / 2 - 4",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = PartRoleEnum.Hardware,
                Name = "Фурнитура",
                Description = "Роль для крепежа и комплектующих. Габариты не высчитываются.",
                LengthFormula = "0",
                WidthFormula = "0",
                UpdateDate = seedDate
            }
        );
    }
}