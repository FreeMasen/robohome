using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace projects.Migrations
{
    public partial class invers_rel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flip_Switches_SwitchId",
                table: "Flip");

            migrationBuilder.DropForeignKey(
                name: "FK_Switches_Remotes_RemoteId",
                table: "Switches");

            migrationBuilder.AlterColumn<int>(
                name: "RemoteId",
                table: "Switches",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SwitchId",
                table: "Flip",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flip_Switches_SwitchId",
                table: "Flip",
                column: "SwitchId",
                principalTable: "Switches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Switches_Remotes_RemoteId",
                table: "Switches",
                column: "RemoteId",
                principalTable: "Remotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flip_Switches_SwitchId",
                table: "Flip");

            migrationBuilder.DropForeignKey(
                name: "FK_Switches_Remotes_RemoteId",
                table: "Switches");

            migrationBuilder.AlterColumn<int>(
                name: "RemoteId",
                table: "Switches",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<int>(
                name: "SwitchId",
                table: "Flip",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AddForeignKey(
                name: "FK_Flip_Switches_SwitchId",
                table: "Flip",
                column: "SwitchId",
                principalTable: "Switches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Switches_Remotes_RemoteId",
                table: "Switches",
                column: "RemoteId",
                principalTable: "Remotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
