using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projects.Migrations
{
    public partial class location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemoteId",
                table: "Switches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Remotes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Switches_RemoteId",
                table: "Switches",
                column: "RemoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Switches_Remotes_RemoteId",
                table: "Switches",
                column: "RemoteId",
                principalTable: "Remotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Switches_Remotes_RemoteId",
                table: "Switches");

            migrationBuilder.DropIndex(
                name: "IX_Switches_RemoteId",
                table: "Switches");

            migrationBuilder.DropColumn(
                name: "RemoteId",
                table: "Switches");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Remotes");
        }
    }
}
