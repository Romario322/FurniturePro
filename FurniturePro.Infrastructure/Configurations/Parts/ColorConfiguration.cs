using FurniturePro.Core.Entities.Parts;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Parts;

internal class ColorConfiguration : DictionaryEntityConfiguration<Color, int>
{
    public override void Configure(EntityTypeBuilder<Color> builder)
    {
        base.Configure(builder);

        builder.ToTable("Colors");

        builder.HasMany(e => e.Parts)
               .WithOne();
    }
}
