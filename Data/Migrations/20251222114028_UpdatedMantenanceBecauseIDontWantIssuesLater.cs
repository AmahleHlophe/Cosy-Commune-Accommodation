using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cosycommune.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMantenanceBecauseIDontWantIssuesLater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenance_Tenants_TenantId1",
                table: "Maintenance");

            migrationBuilder.DropIndex(
                name: "IX_Maintenance_TenantId1",
                table: "Maintenance");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "Maintenance");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "Maintenance",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_TenantId",
                table: "Maintenance",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenance_Tenants_TenantId",
                table: "Maintenance",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenance_Tenants_TenantId",
                table: "Maintenance");

            migrationBuilder.DropIndex(
                name: "IX_Maintenance_TenantId",
                table: "Maintenance");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Maintenance",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "TenantId1",
                table: "Maintenance",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_TenantId1",
                table: "Maintenance",
                column: "TenantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenance_Tenants_TenantId1",
                table: "Maintenance",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "Id");
        }
    }
}
