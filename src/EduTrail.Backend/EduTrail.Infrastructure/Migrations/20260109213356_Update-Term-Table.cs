using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTermTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Term_TermId",
                table: "CourseOfferings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Term",
                table: "Term");

            migrationBuilder.RenameTable(
                name: "Term",
                newName: "Terms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Terms",
                table: "Terms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Terms_TermId",
                table: "CourseOfferings",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Terms_TermId",
                table: "CourseOfferings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Terms",
                table: "Terms");

            migrationBuilder.RenameTable(
                name: "Terms",
                newName: "Term");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Term",
                table: "Term",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Term_TermId",
                table: "CourseOfferings",
                column: "TermId",
                principalTable: "Term",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
