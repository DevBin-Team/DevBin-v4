using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class BugfixesDeletedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "Pastes",
                newName: "ExpireDate");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Pastes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Pastes");

            migrationBuilder.RenameColumn(
                name: "ExpireDate",
                table: "Pastes",
                newName: "ExpirationDate");
        }
    }
}
