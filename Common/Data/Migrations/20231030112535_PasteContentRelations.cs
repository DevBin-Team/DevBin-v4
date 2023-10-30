using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class PasteContentRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "PasteContents",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Pastes_Hash",
                table: "Pastes",
                column: "Hash");

            migrationBuilder.AddForeignKey(
                name: "FK_Pastes_PasteContents_Hash",
                table: "Pastes",
                column: "Hash",
                principalTable: "PasteContents",
                principalColumn: "Hash",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pastes_PasteContents_Hash",
                table: "Pastes");

            migrationBuilder.DropIndex(
                name: "IX_Pastes_Hash",
                table: "Pastes");

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "PasteContents",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
