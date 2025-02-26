using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Esport.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTournamentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TournamentDate",
                table: "Tournaments",
                newName: "StartDate");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Tournaments",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Tournaments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Tournaments");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Tournaments",
                newName: "TournamentDate");
        }
    }
}
