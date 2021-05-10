using Microsoft.EntityFrameworkCore.Migrations;

namespace information_system.Migrations
{
    public partial class WorkAddIsWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isWork",
                table: "Works",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isWork",
                table: "Works");
        }
    }
}
