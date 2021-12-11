using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookmanagementapi.Migrations
{
    public partial class AddedUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Book",
                table: "BookLists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BookLists_Book",
                table: "BookLists",
                column: "Book",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookLists_Book",
                table: "BookLists");

            migrationBuilder.AlterColumn<string>(
                name: "Book",
                table: "BookLists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
