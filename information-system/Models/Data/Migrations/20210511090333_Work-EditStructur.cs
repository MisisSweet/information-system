using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace information_system.Migrations
{
    public partial class WorkEditStructur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "PauseEnd",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "PauseStart",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "WorkEnd",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "WorkStart",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "isWork",
                table: "Works");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Works",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Works",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Works");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "Works",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PauseEnd",
                table: "Works",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PauseStart",
                table: "Works",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkEnd",
                table: "Works",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkStart",
                table: "Works",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isWork",
                table: "Works",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
