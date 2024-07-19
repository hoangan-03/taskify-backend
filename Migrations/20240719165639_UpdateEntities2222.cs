using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities2222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Tasks",
                newName: "Deadline");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Tasks",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "Tasks",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Tasks",
                newName: "Date");
        }
    }
}
