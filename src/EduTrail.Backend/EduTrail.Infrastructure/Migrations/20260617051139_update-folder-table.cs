using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatefoldertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseOfferingId",
                table: "Folders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Folders_CourseOfferingId",
                table: "Folders",
                column: "CourseOfferingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_CourseOfferings_CourseOfferingId",
                table: "Folders",
                column: "CourseOfferingId",
                principalTable: "CourseOfferings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_CourseOfferings_CourseOfferingId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_CourseOfferingId",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "CourseOfferingId",
                table: "Folders");
        }
    }
}
