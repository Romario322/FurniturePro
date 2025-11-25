using FurniturePro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class FurnitureConfiguration : IEntityTypeConfiguration<Furniture>
{
    public void Configure(EntityTypeBuilder<Furniture> builder)
    {
        builder.ToTable("furnitures");

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasKey(et => et.Id);

        builder.HasMany(et => et.FurnitureCompositions)
               .WithOne(e => e.Entity1)
               .HasForeignKey(e => e.Entity1Id)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(et => et.OrderCompositions)
               .WithOne(e => e.Entity2)
               .HasForeignKey(e => e.Entity2Id)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
