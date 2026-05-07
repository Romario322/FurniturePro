using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Enums;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.System;

internal class EmployeeConfiguration : BaseEntityConfiguration<Employee, int>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        base.Configure(builder);

        builder.ToTable("Employees");

        builder.Property(e => e.FullName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.Login)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasIndex(e => e.Login)
               .IsUnique();

        builder.Property(e => e.HashPassword)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.Activity).IsRequired();

        builder.HasOne(e => e.SystemRole)
               .WithMany(r => r.Employees)
               .HasForeignKey(e => e.SystemRoleId)
               .OnDelete(DeleteBehavior.Restrict);

        var seedDate = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc);

        builder.HasData(
            new Employee
            {
                Id = 1,
                FullName = "Системный Администратор",
                Login = "Admin",
                HashPassword = BCrypt.Net.BCrypt.HashPassword("admin"),
                Activity = true,
                SystemRoleId = SystemRoleEnum.Administrator,
                UpdateDate = seedDate
            }
        );
    }
}