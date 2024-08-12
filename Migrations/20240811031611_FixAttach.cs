using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixAttach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Attachments",
                newName: "FileType");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Attachments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedAt",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Attachments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedAt",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "Attachments",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Attachments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
