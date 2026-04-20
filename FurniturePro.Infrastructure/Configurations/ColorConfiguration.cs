using FurniturePro.Core.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable("colors");

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasKey(et => et.Id);

        builder.HasMany(et => et.Parts)
               .WithOne(e => e.Color)
               .HasForeignKey(e => e.ColorId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
