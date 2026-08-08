using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatepostdiscussionmodeladddpohforuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookmarkedDate",
                table: "PostUserActions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBookmarked",
                table: "PostUserActions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "PostUserActions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LikedDate",
                table: "PostUserActions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShareCount",
                table: "PostUserActions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookmarkedDate",
                table: "PostUserActions");

            migrationBuilder.DropColumn(
                name: "IsBookmarked",
                table: "PostUserActions");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "PostUserActions");

            migrationBuilder.DropColumn(
                name: "LikedDate",
                table: "PostUserActions");

            migrationBuilder.DropColumn(
                name: "ShareCount",
                table: "PostUserActions");
        }
    }
}
