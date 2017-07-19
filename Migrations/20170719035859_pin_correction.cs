using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projects.Migrations
{
    public partial class pin_correction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PinId",
                table: "Lights",
                newName: "OnPin");

            migrationBuilder.AddColumn<int>(
                name: "OffPin",
                table: "Lights",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OffPin",
                table: "Lights");

            migrationBuilder.RenameColumn(
                name: "OnPin",
                table: "Lights",
                newName: "PinId");
        }
    }
}
