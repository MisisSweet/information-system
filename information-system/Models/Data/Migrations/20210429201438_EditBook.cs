using Microsoft.EntityFrameworkCore.Migrations;

namespace information_system.Migrations
{
    public partial class EditBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookPicture",
                schema: "Identity",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                schema: "Identity",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookPicture",
                schema: "Identity",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Count",
                schema: "Identity",
                table: "Books");
        }
    }
}
