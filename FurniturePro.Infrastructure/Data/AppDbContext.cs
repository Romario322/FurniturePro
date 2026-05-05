using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Constructors;
using FurniturePro.Core.Entities.FurnitureEntities;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace FurniturePro.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    #region Системные таблицы и Аудит
    public DbSet<DeletedId> DeletedIds { get; set; }
    #endregion

    #region Пользователи
    public DbSet<SystemRole> SystemRoles { get; set; }
    public DbSet<Employee> Employees { get; set; }
    #endregion

    #region Справочники и Каталог Деталей
    public DbSet<Color> Colors { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<PartCategory> PartCategories { get; set; }
    public DbSet<PartType> PartTypes { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Price> Prices { get; set; }
    #endregion

    #region Каталог Мебели и Конструктор
    public DbSet<FurnitureCategory> FurnitureCategories { get; set; }
    public DbSet<Furniture> Furnitures { get; set; }
    public DbSet<PartRole> PartRoles { get; set; }
    public DbSet<ReplacementGroup> ReplacementGroups { get; set; }
    public DbSet<ReplacementGroupItem> ReplacementGroupItems { get; set; }
    public DbSet<FurniturePart> FurnitureParts { get; set; }
    #endregion

    #region Заказы, Производство и Клиенты
    public DbSet<Status> Statuses { get; set; }
    public DbSet<StatusChange> StatusChanges { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderComposition> OrderCompositions { get; set; }
    public DbSet<OrderPartDetail> OrderPartDetails { get; set; }
    public DbSet<Client> Clients { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}