using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class BugfixPasteUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Pastes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Pastes",
                type: "int",
                nullable: true);
        }
    }
}
