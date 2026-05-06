using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Enums;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Parts;

internal class PartTypeConfiguration : DictionaryEntityConfiguration<PartType, PartTypeEnum>
{
    public override void Configure(EntityTypeBuilder<PartType> builder)
    {
        base.Configure(builder);

        builder.ToTable("PartTypes");

        var seedDate = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc);

        builder.HasData(
            new PartType
            {
                Id = PartTypeEnum.Hardware,
                Name = "Фурнитура и комплектующие",
                Description = "Типичные детали (крепеж, ножки, ручки), не требующие распила и обработки.",
                UpdateDate = seedDate
            },
            new PartType
            {
                Id = PartTypeEnum.CuttingMaterial,
                Name = "Материал для распила",
                Description = "Листовые и погонажные материалы (ЛДСП, ДВП, МДФ, профили), требующие деталировки.",
                UpdateDate = seedDate
            }
        );
    }
}
