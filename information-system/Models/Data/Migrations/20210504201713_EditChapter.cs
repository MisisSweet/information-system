using Microsoft.EntityFrameworkCore.Migrations;

namespace information_system.Migrations
{
    public partial class EditChapter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Chapters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Chapters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
