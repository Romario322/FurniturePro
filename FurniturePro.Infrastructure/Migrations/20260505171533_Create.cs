using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurniturePro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FurnitureCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LengthFormula = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    WidthFormula = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReplacementGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplacementGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Furnitures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaseWidth = table.Column<int>(type: "integer", nullable: false),
                    BaseHeight = table.Column<int>(type: "integer", nullable: false),
                    BaseDepth = table.Column<int>(type: "integer", nullable: false),
                    Markup = table.Column<int>(type: "integer", nullable: false),
                    Activity = table.Column<bool>(type: "boolean", nullable: false),
                    FurnitureCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Furnitures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Furnitures_FurnitureCategories_FurnitureCategoryId",
                        column: x => x.FurnitureCategoryId,
                        principalTable: "FurnitureCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Thickness = table.Column<int>(type: "integer", nullable: true),
                    WasteCoefficient = table.Column<int>(type: "integer", nullable: true),
                    WeightPerUnit = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    Activity = table.Column<bool>(type: "boolean", nullable: false),
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    ColorId = table.Column<int>(type: "integer", nullable: false),
                    PartTypeId = table.Column<int>(type: "integer", nullable: false),
                    PartCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parts_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parts_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parts_PartCategories_PartCategoryId",
                        column: x => x.PartCategoryId,
                        principalTable: "PartCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parts_PartTypes_PartTypeId",
                        column: x => x.PartTypeId,
                        principalTable: "PartTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Login = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    HashPassword = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Activity = table.Column<bool>(type: "boolean", nullable: false),
                    SystemRoleId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_SystemRoles_SystemRoleId",
                        column: x => x.SystemRoleId,
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FurnitureParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    FurnitureId = table.Column<int>(type: "integer", nullable: false),
                    PartRoleId = table.Column<int>(type: "integer", nullable: false),
                    ReplacementGroupId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FurnitureParts_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FurnitureParts_PartRoles_PartRoleId",
                        column: x => x.PartRoleId,
                        principalTable: "PartRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FurnitureParts_ReplacementGroups_ReplacementGroupId",
                        column: x => x.ReplacementGroupId,
                        principalTable: "ReplacementGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReplacementGroupItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    ReplacementGroupId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplacementGroupItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReplacementGroupItems_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReplacementGroupItems_ReplacementGroups_ReplacementGroupId",
                        column: x => x.ReplacementGroupId,
                        principalTable: "ReplacementGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeletedIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    ResponsibleEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeletedIds_Employees_ResponsibleEmployeeId",
                        column: x => x.ResponsibleEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    ResponsibleEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Employees_ResponsibleEmployeeId",
                        column: x => x.ResponsibleEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prices_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderCompositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Length = table.Column<int>(type: "integer", nullable: true),
                    Width = table.Column<int>(type: "integer", nullable: true),
                    Depth = table.Column<int>(type: "integer", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    FurnitureId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCompositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCompositions_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderCompositions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusChanges_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusChanges_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderPartDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CostPerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    SawingLength = table.Column<int>(type: "integer", nullable: false),
                    SawingWidth = table.Column<int>(type: "integer", nullable: false),
                    OrderCompositionId = table.Column<int>(type: "integer", nullable: false),
                    PartId = table.Column<int>(type: "integer", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPartDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPartDetails_OrderCompositions_OrderCompositionId",
                        column: x => x.OrderCompositionId,
                        principalTable: "OrderCompositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderPartDetails_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PartRoles",
                columns: new[] { "Id", "Description", "LengthFormula", "Name", "UpdateDate", "WidthFormula" },
                values: new object[,]
                {
                    { 1, null, "H", "Боковина", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "W" },
                    { 2, null, "D", "Крышка", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "W" },
                    { 3, null, "D - 2 * T", "Дно", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "W" },
                    { 4, null, "D - 2 * T", "Полка", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "W - 20" },
                    { 5, null, "H - 4", "Задняя стенка", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "W - 4" },
                    { 6, null, "H - 4", "Фасад (Одинарный)", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "W - 4" },
                    { 7, null, "H - 4", "Фасад (Двойной распашной)", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "W / 2 - 4" },
                    { 8, "Роль для крепежа и комплектующих. Габариты не высчитываются.", "0", "Фурнитура", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "0" }
                });

            migrationBuilder.InsertData(
                table: "PartTypes",
                columns: new[] { "Id", "Description", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "Типичные детали (крепеж, ножки, ручки), не требующие распила и обработки.", "Фурнитура и комплектующие", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Листовые и погонажные материалы (ЛДСП, ДВП, МДФ, профили), требующие деталировки.", "Материал для распила", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Description", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "Заказ оформлен, ожидает подтверждения", "Новый", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Заказ передан в цех на производство", "В работе", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "Мебель изготовлена и находится на складе", "Готов", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "Заказ передан клиенту", "Отгружен", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "Заказ отменен", "Отменен", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "SystemRoles",
                columns: new[] { "Id", "Description", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "Полный доступ к системе", "Администратор", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Работа с клиентами, заказами и ценами", "Менеджер по продажам", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "Ведение каталога мебели и спецификаций", "Конструктор", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "Просмотр заказов, перевод статусов производства", "Мастер цеха", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Phone",
                table: "Clients",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UpdateDate",
                table: "Clients",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_Name",
                table: "Colors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_UpdateDate",
                table: "Colors",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedIds_ResponsibleEmployeeId",
                table: "DeletedIds",
                column: "ResponsibleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedIds_TableName_EntityId",
                table: "DeletedIds",
                columns: new[] { "TableName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_DeletedIds_UpdateDate",
                table: "DeletedIds",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Login",
                table: "Employees",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SystemRoleId",
                table: "Employees",
                column: "SystemRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UpdateDate",
                table: "Employees",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureCategories_Name",
                table: "FurnitureCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureCategories_UpdateDate",
                table: "FurnitureCategories",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureParts_FurnitureId",
                table: "FurnitureParts",
                column: "FurnitureId");

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureParts_PartRoleId",
                table: "FurnitureParts",
                column: "PartRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureParts_ReplacementGroupId",
                table: "FurnitureParts",
                column: "ReplacementGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureParts_UpdateDate",
                table: "FurnitureParts",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_FurnitureCategoryId",
                table: "Furnitures",
                column: "FurnitureCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_Sku",
                table: "Furnitures",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_UpdateDate",
                table: "Furnitures",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Name",
                table: "Materials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UpdateDate",
                table: "Materials",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCompositions_FurnitureId",
                table: "OrderCompositions",
                column: "FurnitureId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCompositions_OrderId",
                table: "OrderCompositions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCompositions_UpdateDate",
                table: "OrderCompositions",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPartDetails_OrderCompositionId",
                table: "OrderPartDetails",
                column: "OrderCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPartDetails_PartId",
                table: "OrderPartDetails",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPartDetails_UpdateDate",
                table: "OrderPartDetails",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ResponsibleEmployeeId",
                table: "Orders",
                column: "ResponsibleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UpdateDate",
                table: "Orders",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_PartCategories_Name",
                table: "PartCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartCategories_UpdateDate",
                table: "PartCategories",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_PartRoles_Name",
                table: "PartRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartRoles_UpdateDate",
                table: "PartRoles",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_ColorId",
                table: "Parts",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_MaterialId",
                table: "Parts",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_PartCategoryId",
                table: "Parts",
                column: "PartCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_PartTypeId",
                table: "Parts",
                column: "PartTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_Sku",
                table: "Parts",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_UpdateDate",
                table: "Parts",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_PartTypes_Name",
                table: "PartTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartTypes_UpdateDate",
                table: "PartTypes",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_EmployeeId",
                table: "Prices",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_PartId",
                table: "Prices",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_UpdateDate",
                table: "Prices",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementGroupItems_PartId",
                table: "ReplacementGroupItems",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementGroupItems_ReplacementGroupId_PartId",
                table: "ReplacementGroupItems",
                columns: new[] { "ReplacementGroupId", "PartId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementGroupItems_UpdateDate",
                table: "ReplacementGroupItems",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementGroups_Name",
                table: "ReplacementGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementGroups_UpdateDate",
                table: "ReplacementGroups",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChanges_OrderId",
                table: "StatusChanges",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChanges_StatusId",
                table: "StatusChanges",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChanges_UpdateDate",
                table: "StatusChanges",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Name",
                table: "Statuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_UpdateDate",
                table: "Statuses",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_Name",
                table: "SystemRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_UpdateDate",
                table: "SystemRoles",
                column: "UpdateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedIds");

            migrationBuilder.DropTable(
                name: "FurnitureParts");

            migrationBuilder.DropTable(
                name: "OrderPartDetails");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "ReplacementGroupItems");

            migrationBuilder.DropTable(
                name: "StatusChanges");

            migrationBuilder.DropTable(
                name: "PartRoles");

            migrationBuilder.DropTable(
                name: "OrderCompositions");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "ReplacementGroups");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Furnitures");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "PartCategories");

            migrationBuilder.DropTable(
                name: "PartTypes");

            migrationBuilder.DropTable(
                name: "FurnitureCategories");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "SystemRoles");
        }
    }
}
