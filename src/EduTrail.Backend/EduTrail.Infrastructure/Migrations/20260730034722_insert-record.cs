using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class insertrecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                            table: "PostTypes",
                            columns: new[]
                            {
                    "Id",
                    "Name",
                    "Description",
                    "IsActive",
                    "CreatedDate",
                    "CreatedById",
                    "UpdatedDate",
                    "UpdatedById"
                            },
                            values: new object[,]
                            {
                    {
                        new Guid("9efb883c-9348-41c1-b51e-1dd463ec1f5b"),
                        "Note",
                        "Standard note post",
                        true,
                        DateTimeOffset.UtcNow,
                        null,
                        null,
                        null
                    },
                    {
                        new Guid("3002fb15-b1f7-47ba-90e8-34db886a901a"),
                        "Question",
                        "Question post",
                        true,
                        DateTimeOffset.UtcNow,
                        null,
                        null,
                        null
                    },
                    {
                        new Guid("f35db777-9824-4d40-976d-33262c1c23ce"),
                        "Poll",
                        "Poll post",
                        true,
                        DateTimeOffset.UtcNow,
                        null,
                        null,
                        null
                    }
                 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                           table: "PostTypes",
                           keyColumn: "Id",
                           keyValue: new Guid("9efb883c-9348-41c1-b51e-1dd463ec1f5b"));

            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: new Guid("3002fb15-b1f7-47ba-90e8-34db886a901a"));

            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: new Guid("f35db777-9824-4d40-976d-33262c1c23ce"));
        }
    }
}
