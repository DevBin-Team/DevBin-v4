using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPasteContentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Pastes");

            migrationBuilder.CreateTable(
                name: "PasteContents",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasteContents", x => x.Hash);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasteContents");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Pastes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
