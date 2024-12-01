using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanaTomo.Migrations
{
    /// <inheritdoc />
    public partial class anki_logic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Easiness",
                table: "AnkiItems",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Interval",
                table: "AnkiItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextReviewDate",
                table: "AnkiItems",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RepetitionNumber",
                table: "AnkiItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Easiness",
                table: "AnkiItems");

            migrationBuilder.DropColumn(
                name: "Interval",
                table: "AnkiItems");

            migrationBuilder.DropColumn(
                name: "NextReviewDate",
                table: "AnkiItems");

            migrationBuilder.DropColumn(
                name: "RepetitionNumber",
                table: "AnkiItems");
        }
    }
}
