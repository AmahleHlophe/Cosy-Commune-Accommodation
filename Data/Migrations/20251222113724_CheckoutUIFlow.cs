using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cosycommune.Data.Migrations
{
    /// <inheritdoc />
    public partial class CheckoutUIFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checkouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    VacateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amenity = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Unit = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Priority = table.Column<string>(type: "TEXT", nullable: false),
                    LocationInUnit = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PreferredAccessTime = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintenance_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_TenantId1",
                table: "Maintenance",
                column: "TenantId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkouts");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "Maintenance");
        }
    }
}
