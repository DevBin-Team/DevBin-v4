using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixFolderMissingUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_AspNetUsers_ApplicationUserId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_ApplicationUserId",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Folders");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_UserId",
                table: "Folders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_AspNetUsers_UserId",
                table: "Folders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_AspNetUsers_UserId",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_UserId",
                table: "Folders");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Folders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ApplicationUserId",
                table: "Folders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_AspNetUsers_ApplicationUserId",
                table: "Folders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
