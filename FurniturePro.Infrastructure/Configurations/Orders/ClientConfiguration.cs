using FurniturePro.Core.Entities.Orders;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Orders;

internal class ClientConfiguration : BaseEntityConfiguration<Client, int>
{
    public override void Configure(EntityTypeBuilder<Client> builder)
    {
        base.Configure(builder);

        builder.ToTable("Clients");

        builder.Property(e => e.FullName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.Phone)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasIndex(e => e.Phone)
               .IsUnique();

        builder.Property(e => e.Email)
               .IsRequired()
               .HasMaxLength(200);
    }
}
