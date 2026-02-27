using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TAMonthTableAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TALabHours");

            migrationBuilder.CreateTable(
                name: "TALabMonths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TALabMonths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TALabMonths_Enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TALabDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TALabMonthId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TALabDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TALabDays_Enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TALabDays_TALabMonths_TALabMonthId",
                        column: x => x.TALabMonthId,
                        principalTable: "TALabMonths",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TALabSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TALabDayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    RemoteLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TALabSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TALabSlots_TALabDays_TALabDayId",
                        column: x => x.TALabDayId,
                        principalTable: "TALabDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TALabDays_EnrollmentId",
                table: "TALabDays",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TALabDays_TALabMonthId",
                table: "TALabDays",
                column: "TALabMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_TALabMonths_EnrollmentId",
                table: "TALabMonths",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TALabSlots_TALabDayId",
                table: "TALabSlots",
                column: "TALabDayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TALabSlots");

            migrationBuilder.DropTable(
                name: "TALabDays");

            migrationBuilder.DropTable(
                name: "TALabMonths");

            migrationBuilder.CreateTable(
                name: "TALabHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LabDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    RemoteLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TALabHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TALabHours_Enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TALabHours_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TALabHours_EnrollmentId",
                table: "TALabHours",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TALabHours_LabId",
                table: "TALabHours",
                column: "LabId");
        }
    }
}
