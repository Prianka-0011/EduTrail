using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatefolderinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Polls_PostId",
                table: "Polls");

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Polls",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FolderPost",
                columns: table => new
                {
                    FoldersId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderPost", x => new { x.FoldersId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_FolderPost_Folders_FoldersId",
                        column: x => x.FoldersId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderPost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polls_PostId",
                table: "Polls",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FolderPost_PostsId",
                table: "FolderPost",
                column: "PostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderPost");

            migrationBuilder.DropIndex(
                name: "IX_Polls_PostId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "Polls");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_PostId",
                table: "Polls",
                column: "PostId");
        }
    }
}
