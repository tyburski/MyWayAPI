using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWayAPI.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DropDate",
                table: "RouteEvents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DropLatitude",
                table: "RouteEvents",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DropLongitude",
                table: "RouteEvents",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropDate",
                table: "RouteEvents");

            migrationBuilder.DropColumn(
                name: "DropLatitude",
                table: "RouteEvents");

            migrationBuilder.DropColumn(
                name: "DropLongitude",
                table: "RouteEvents");
        }
    }
}
