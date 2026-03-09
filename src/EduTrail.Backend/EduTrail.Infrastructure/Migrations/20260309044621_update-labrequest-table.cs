using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatelabrequesttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabRequests_Courses_CourseId",
                table: "LabRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LabRequests_Users_AssignedTeacherId",
                table: "LabRequests");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "LabRequests",
                newName: "CourseOfferingId");

            migrationBuilder.RenameIndex(
                name: "IX_LabRequests_CourseId",
                table: "LabRequests",
                newName: "IX_LabRequests_CourseOfferingId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabRequests_CourseOfferings_CourseOfferingId",
                table: "LabRequests",
                column: "CourseOfferingId",
                principalTable: "CourseOfferings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabRequests_Enrollments_AssignedTeacherId",
                table: "LabRequests",
                column: "AssignedTeacherId",
                principalTable: "Enrollments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabRequests_CourseOfferings_CourseOfferingId",
                table: "LabRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LabRequests_Enrollments_AssignedTeacherId",
                table: "LabRequests");

            migrationBuilder.RenameColumn(
                name: "CourseOfferingId",
                table: "LabRequests",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_LabRequests_CourseOfferingId",
                table: "LabRequests",
                newName: "IX_LabRequests_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabRequests_Courses_CourseId",
                table: "LabRequests",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabRequests_Users_AssignedTeacherId",
                table: "LabRequests",
                column: "AssignedTeacherId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
