using Microsoft.EntityFrameworkCore.Migrations;

namespace EduTrail.Infrastructure.Migrations.DataMigrations
{
    public abstract partial class InitialDataMigration : Migration
    {
        private readonly Guid _defaultAdminId = Guid.Parse(Environment.GetEnvironmentVariable("DEFAULT_ADMIN_ID") ?? "bf1ec798-50be-4449-9820-af261774542c");
        private readonly Guid _defaultAdminRoleId = Guid.Parse(Environment.GetEnvironmentVariable("DEFAULT_ADMIN_ROLE_ID") ?? "8f3b2a91-6e5c-4c7b-9e91-1a2d4f8c3b10");
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "Description", "CreatedDate", "CreatedById" },
                values: new object[,]
                {
            { Guid.Parse("8f3b2a91-6e5c-4c7b-9e91-1a2d4f8c3b10"), "Instructor", "Role for course instructors", DateTime.UtcNow, Guid.Parse("00000000-0000-0000-0000-000000000000") },
            { Guid.Parse("2c9d7f41-8a3e-4f2b-b6a5-9e1c3d4a7f82"), "Student", "Role for students", DateTime.UtcNow, Guid.Parse("00000000-0000-0000-0000-000000000000") },
            { Guid.Parse("5a1e4c7d-9b82-4f36-a3c1-6d9e2f8b0a55"), "TA", "Role for teaching assistants", DateTime.UtcNow, Guid.Parse("00000000-0000-0000-0000-000000000000") }
                });

            migrationBuilder.InsertData(
                table: "TermTypes",
                columns: new[] { "Id", "Name", "CreatedDate", "CreatedById", "UpdatedDate", "UpdatedById" },
                values: new object[,]
                {
            { Guid.Parse("7e8f9f7e-75b3-4866-94a3-464f8711c544"), "Spring", DateTime.UtcNow, null, DateTime.UtcNow, null },
            { Guid.Parse("f262de21-7519-4468-b63a-653dafc6b8f9"), "Fall", DateTime.UtcNow, null, DateTime.UtcNow, null },
            { Guid.Parse("855021e3-8d31-47b2-b787-65e1ddbb4fe0"), "Winter", DateTime.UtcNow, null, DateTime.UtcNow, null },
            { Guid.Parse("f2231caa-ad7f-42f6-8283-043d54af790c"), "Summer", DateTime.UtcNow, null, DateTime.UtcNow, null }
                });

            migrationBuilder.InsertData(
                table: "StatusTypes",
                columns: new[] { "Id", "Name", "Description", "CreatedDate", "CreatedById", "UpdatedDate", "UpdatedById" },
                values: new object[,]
                {
            { Guid.Parse("ce5e6303-3ac6-4af1-92b4-f708da026d20"), "Help Request", "Status type for help request workflow", DateTime.UtcNow, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name", "Description", "StatusTypeId", "CreatedDate", "CreatedById", "UpdatedDate", "UpdatedById" },
                values: new object[,]
                {
            { Guid.Parse("627407c8-700d-46fc-a3e5-02dc368fb75e"), "Pending", "Help request is pending", Guid.Parse("ce5e6303-3ac6-4af1-92b4-f708da026d20"), DateTime.UtcNow, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]
                {
            "Id", "FirstName", "MiddleName", "LastName", "Email", "ImageUrl",
            "CanvasUserId", "SISId", "PasswordHash", "PasswordSalt", "IsActive",
            "CreatedDate", "CreatedById", "UpdatedDate", "UpdatedById"
                },
                values: new object[,]
                {
            {
                _defaultAdminId, "Admin", null, "Power", "adminpower@yopmail.com", null,
                null, null,
                "OIvraEJwj1gYf1uQu+8lYeyw8ripiCmFWWCenFC20h7aG7GOOgmhftzqN02nxt4b5h97rymt0a7rRVUG7afLUg==",
                "wLsTHb6azf2oj7JQ/vEA9cb/1g/axBQS4BSdoero47jX6XPkwEVFtllcKbr3t5e6hd7JDDEdhFjNUtSfmoUKtfa3wJY9F9rRLXCoLAdeRJGln8YsbJgKvfz5mDgsXao+7PuelGiPKcm/lTE0FIQ+F0H47dOmRbpgAE1qA8FkEqc=",
                true, DateTime.UtcNow, Guid.Parse("8f3b2a91-6e5c-4c7b-9e91-1a2d4f8c3b10"), DateTime.UtcNow, Guid.Parse("8f3b2a91-6e5c-4c7b-9e91-1a2d4f8c3b10")
            }
                });
                migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "UsersId", "RolesId" },
                values: new object[] { _defaultAdminId, _defaultAdminRoleId }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("Users", "Email", "adminpower@yopmail.com");
            migrationBuilder.DeleteData("Statuses", "Id", Guid.Parse("627407c8-700d-46fc-a3e5-02dc368fb75e"));
            migrationBuilder.DeleteData("StatusTypes", "Id", Guid.Parse("ce5e6303-3ac6-4af1-92b4-f708da026d20"));
            migrationBuilder.DeleteData("TermTypes", "Id", Guid.Parse("7e8f9f7e-75b3-4866-94a3-464f8711c544"));
            migrationBuilder.DeleteData("TermTypes", "Id", Guid.Parse("f262de21-7519-4468-b63a-653dafc6b8f9"));
            migrationBuilder.DeleteData("TermTypes", "Id", Guid.Parse("855021e3-8d31-47b2-b787-65e1ddbb4fe0"));
            migrationBuilder.DeleteData("TermTypes", "Id", Guid.Parse("f2231caa-ad7f-42f6-8283-043d54af790c"));
            migrationBuilder.DeleteData("Roles", "Id", Guid.Parse("8f3b2a91-6e5c-4c7b-9e91-1a2d4f8c3b10"));
            migrationBuilder.DeleteData("Roles", "Id", Guid.Parse("2c9d7f41-8a3e-4f2b-b6a5-9e1c3d4a7f82"));
            migrationBuilder.DeleteData("Roles", "Id", Guid.Parse("5a1e4c7d-9b82-4f36-a3c1-6d9e2f8b0a55"));
        }

    }
}
