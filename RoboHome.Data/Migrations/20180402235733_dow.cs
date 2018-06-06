using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace projects.Migrations
{
    public partial class dow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Time_DayOfWeek",
                table: "KeyTimes",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Time_DayOfWeek",
                table: "Flips",
                type: "int4",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time_DayOfWeek",
                table: "KeyTimes");

            migrationBuilder.DropColumn(
                name: "Time_DayOfWeek",
                table: "Flips");
        }
    }
}
