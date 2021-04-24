using Microsoft.EntityFrameworkCore.Migrations;

namespace information_system.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumderReadTicket",
                schema: "Identity",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumderReadTicket",
                schema: "Identity",
                table: "AspNetUsers");
        }
    }
}
