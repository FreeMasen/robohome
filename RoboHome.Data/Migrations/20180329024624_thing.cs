using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace projects.Migrations
{
    public partial class thing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flip_Switches_SwitchId",
                table: "Flip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flip",
                table: "Flip");

            migrationBuilder.RenameTable(
                name: "Flip",
                newName: "Flips");

            migrationBuilder.RenameIndex(
                name: "IX_Flip_SwitchId",
                table: "Flips",
                newName: "IX_Flips_SwitchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flips",
                table: "Flips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flips_Switches_SwitchId",
                table: "Flips",
                column: "SwitchId",
                principalTable: "Switches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flips_Switches_SwitchId",
                table: "Flips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flips",
                table: "Flips");

            migrationBuilder.RenameTable(
                name: "Flips",
                newName: "Flip");

            migrationBuilder.RenameIndex(
                name: "IX_Flips_SwitchId",
                table: "Flip",
                newName: "IX_Flip_SwitchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flip",
                table: "Flip",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flip_Switches_SwitchId",
                table: "Flip",
                column: "SwitchId",
                principalTable: "Switches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
