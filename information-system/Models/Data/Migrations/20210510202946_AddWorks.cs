using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace information_system.Migrations
{
    public partial class AddWorks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(nullable: false),
                    WorkStart = table.Column<DateTime>(nullable: false),
                    WorkEnd = table.Column<DateTime>(nullable: false),
                    PauseStart = table.Column<DateTime>(nullable: false),
                    PauseEnd = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Works");
        }
    }
}
