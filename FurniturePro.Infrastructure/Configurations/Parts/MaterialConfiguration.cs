using FurniturePro.Core.Entities.Parts;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Parts;

internal class MaterialConfiguration : DictionaryEntityConfiguration<Material, int>
{
    public override void Configure(EntityTypeBuilder<Material> builder)
    {
        base.Configure(builder);

        builder.ToTable("Materials");

        builder.HasMany(e => e.Parts)
               .WithOne();
    }
}
