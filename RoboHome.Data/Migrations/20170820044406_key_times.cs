using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace projects.Migrations
{
    public partial class key_times : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "TimeOfDay",
                table: "Flip",
                newName: "Time_TimeOfDay");

            migrationBuilder.RenameColumn(
                name: "Minute",
                table: "Flip",
                newName: "Time_Minute");

            migrationBuilder.RenameColumn(
                name: "Hour",
                table: "Flip",
                newName: "Time_Hour");

            migrationBuilder.RenameIndex(
                name: "IX_Flips_SwitchId",
                table: "Flip",
                newName: "IX_Flip_SwitchId");

            migrationBuilder.AddColumn<int>(
                name: "Hour",
                table: "Flip",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Minute",
                table: "Flip",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeOfDay",
                table: "Flip",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Time_TimeType",
                table: "Flip",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flip",
                table: "Flip",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "KeyTimes",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Time_Hour = table.Column<int>(type: "int4", nullable: false),
                    Time_Minute = table.Column<int>(type: "int4", nullable: false),
                    Time_TimeOfDay = table.Column<int>(type: "int4", nullable: false),
                    Time_TimeType = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyTimes", x => x.Date);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Flip_Switches_SwitchId",
                table: "Flip",
                column: "SwitchId",
                principalTable: "Switches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flip_Switches_SwitchId",
                table: "Flip");

            migrationBuilder.DropTable(
                name: "KeyTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flip",
                table: "Flip");

            migrationBuilder.DropColumn(
                name: "Hour",
                table: "Flip");

            migrationBuilder.DropColumn(
                name: "Minute",
                table: "Flip");

            migrationBuilder.DropColumn(
                name: "TimeOfDay",
                table: "Flip");

            migrationBuilder.DropColumn(
                name: "Time_TimeType",
                table: "Flip");

            migrationBuilder.RenameTable(
                name: "Flip",
                newName: "Flips");

            migrationBuilder.RenameColumn(
                name: "Time_TimeOfDay",
                table: "Flips",
                newName: "TimeOfDay");

            migrationBuilder.RenameColumn(
                name: "Time_Minute",
                table: "Flips",
                newName: "Minute");

            migrationBuilder.RenameColumn(
                name: "Time_Hour",
                table: "Flips",
                newName: "Hour");

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
