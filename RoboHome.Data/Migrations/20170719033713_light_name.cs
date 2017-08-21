using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projects.Migrations
{
    public partial class light_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Lights",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Lights");
        }
    }
}
