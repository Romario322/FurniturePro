using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    // Dictionaries
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Status> Statuses { get; set; }

    // Base entities
    public DbSet<Part> Parts { get; set; }
    public DbSet<Furniture> Furnitures { get; set; }
    public DbSet<Order> Orders { get; set; }

    // Dependent entities
    public DbSet<Count> Counts { get; set; }
    public DbSet<Price> Prices { get; set; }

    // Connections
    public DbSet<FurnitureComposition> FurnitureCompositions { get; set; }
    public DbSet<OrderComposition> OrderCompositions { get; set; }
    public DbSet<StatusChange> StatusChanges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Dictionaries
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ColorConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialConfiguration());
        modelBuilder.ApplyConfiguration(new StatusConfiguration());

        // Base entities
        modelBuilder.ApplyConfiguration(new PartConfiguration());
        modelBuilder.ApplyConfiguration(new FurnitureConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());

        // Dependent entities
        modelBuilder.ApplyConfiguration(new CountConfiguration());
        modelBuilder.ApplyConfiguration(new PriceConfiguration());

        // Connections
        modelBuilder.ApplyConfiguration(new FurnitureCompositionConfiguration());
        modelBuilder.ApplyConfiguration(new OrderCompositionConfiguration());
        modelBuilder.ApplyConfiguration(new StatusChangeConfiguration());
    }
}
