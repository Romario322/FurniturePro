using FurniturePro.Core.Entities.Constructors;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Constructors;

internal class PartRoleConfiguration : DictionaryEntityConfiguration<PartRole, int>
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
                Id = 1,
                Name = "Боковина",
                LengthFormula = "H",
                WidthFormula = "W",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = 2,
                Name = "Крышка",
                LengthFormula = "D",
                WidthFormula = "W",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = 3,
                Name = "Дно",
                LengthFormula = "D - 2 * T",
                WidthFormula = "W",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = 4,
                Name = "Полка",
                LengthFormula = "D - 2 * T",
                WidthFormula = "W - 20",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = 5,
                Name = "Задняя стенка",
                LengthFormula = "H - 4",
                WidthFormula = "W - 4",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = 6,
                Name = "Фасад (Одинарный)",
                LengthFormula = "H - 4",
                WidthFormula = "W - 4",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = 7,
                Name = "Фасад (Двойной распашной)",
                LengthFormula = "H - 4",
                WidthFormula = "W / 2 - 4",
                UpdateDate = seedDate
            },
            new PartRole
            {
                Id = 8,
                Name = "Фурнитура",
                Description = "Роль для крепежа и комплектующих. Габариты не высчитываются.",
                LengthFormula = "0",
                WidthFormula = "0",
                UpdateDate = seedDate
            }
        );
    }
}