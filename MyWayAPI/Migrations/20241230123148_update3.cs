using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWayAPI.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_WebUsers_OwnerId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Companies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_WebUsers_OwnerId",
                table: "Companies",
                column: "OwnerId",
                principalTable: "WebUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
