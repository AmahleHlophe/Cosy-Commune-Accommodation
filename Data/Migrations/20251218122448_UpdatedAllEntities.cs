using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cosycommune.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tenants",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Tenants",
                newName: "Email");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tenants",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Tenants",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Tenants",
                newName: "LastName");
        }
    }
}
