using FurniturePro.Core.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.HasIndex(c => c.Phone).IsUnique();

        builder.HasKey(et => et.Id);

        builder.HasMany(et => et.Orders)
               .WithOne(e => e.Client)
               .HasForeignKey(e => e.ClientId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(et => et.UpdateDate);
    }
}
