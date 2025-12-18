using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cosycommune.Data.Migrations
{
    /// <inheritdoc />
    public partial class RoomEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Rooms",
                type: "TEXT",
                nullable: true);
        }
    }
}
