using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addweektable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TALabDays_Enrollments_EnrollmentId",
                table: "TALabDays");

            migrationBuilder.DropForeignKey(
                name: "FK_TALabDays_TALabMonths_TALabMonthId",
                table: "TALabDays");

            migrationBuilder.DropForeignKey(
                name: "FK_TALabMonths_Enrollments_EnrollmentId",
                table: "TALabMonths");

            migrationBuilder.DropIndex(
                name: "IX_TALabDays_EnrollmentId",
                table: "TALabDays");

            migrationBuilder.DropIndex(
                name: "IX_TALabDays_TALabMonthId",
                table: "TALabDays");

            migrationBuilder.DropColumn(
                name: "TALabMonthId",
                table: "TALabDays");

            migrationBuilder.RenameColumn(
                name: "EnrollmentId",
                table: "TALabDays",
                newName: "TALabWeekId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EnrollmentId",
                table: "TALabMonths",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TALabWeekId1",
                table: "TALabDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TALabWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    TALabMonthId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TALabWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TALabWeeks_TALabMonths_TALabMonthId",
                        column: x => x.TALabMonthId,
                        principalTable: "TALabMonths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TALabDays_TALabWeekId1",
                table: "TALabDays",
                column: "TALabWeekId1");

            migrationBuilder.CreateIndex(
                name: "IX_TALabWeeks_TALabMonthId",
                table: "TALabWeeks",
                column: "TALabMonthId");

            migrationBuilder.AddForeignKey(
                name: "FK_TALabDays_TALabWeeks_TALabWeekId1",
                table: "TALabDays",
                column: "TALabWeekId1",
                principalTable: "TALabWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TALabMonths_Enrollments_EnrollmentId",
                table: "TALabMonths",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TALabDays_TALabWeeks_TALabWeekId1",
                table: "TALabDays");

            migrationBuilder.DropForeignKey(
                name: "FK_TALabMonths_Enrollments_EnrollmentId",
                table: "TALabMonths");

            migrationBuilder.DropTable(
                name: "TALabWeeks");

            migrationBuilder.DropIndex(
                name: "IX_TALabDays_TALabWeekId1",
                table: "TALabDays");

            migrationBuilder.DropColumn(
                name: "TALabWeekId1",
                table: "TALabDays");

            migrationBuilder.RenameColumn(
                name: "TALabWeekId",
                table: "TALabDays",
                newName: "EnrollmentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EnrollmentId",
                table: "TALabMonths",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "TALabMonthId",
                table: "TALabDays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TALabDays_EnrollmentId",
                table: "TALabDays",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TALabDays_TALabMonthId",
                table: "TALabDays",
                column: "TALabMonthId");

            migrationBuilder.AddForeignKey(
                name: "FK_TALabDays_Enrollments_EnrollmentId",
                table: "TALabDays",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TALabDays_TALabMonths_TALabMonthId",
                table: "TALabDays",
                column: "TALabMonthId",
                principalTable: "TALabMonths",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TALabMonths_Enrollments_EnrollmentId",
                table: "TALabMonths",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "Id");
        }
    }
}
