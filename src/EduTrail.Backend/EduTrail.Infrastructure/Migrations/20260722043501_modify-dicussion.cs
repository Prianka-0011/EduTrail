using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifydicussion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EnrollmentId",
                table: "PostDiscussion",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostDiscussion_EnrollmentId",
                table: "PostDiscussion",
                column: "EnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostDiscussion_Enrollments_EnrollmentId",
                table: "PostDiscussion",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostDiscussion_Enrollments_EnrollmentId",
                table: "PostDiscussion");

            migrationBuilder.DropIndex(
                name: "IX_PostDiscussion_EnrollmentId",
                table: "PostDiscussion");

            migrationBuilder.DropColumn(
                name: "EnrollmentId",
                table: "PostDiscussion");
        }
    }
}
