using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetalabhours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "TALabHours");

            migrationBuilder.AddColumn<DateOnly>(
                name: "LabDate",
                table: "TALabHours",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabDate",
                table: "TALabHours");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "TALabHours",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
