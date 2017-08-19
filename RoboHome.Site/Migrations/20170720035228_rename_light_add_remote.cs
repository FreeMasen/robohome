using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace projects.Migrations
{
    public partial class rename_light_add_remote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flips_Lights_LightId",
                table: "Flips");

            migrationBuilder.DropTable(
                name: "Lights");

            migrationBuilder.RenameColumn(
                name: "LightId",
                table: "Flips",
                newName: "SwitchId");

            migrationBuilder.RenameIndex(
                name: "IX_Flips_LightId",
                table: "Flips",
                newName: "IX_Flips_SwitchId");

            migrationBuilder.CreateTable(
                name: "Remotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Switches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    OffPin = table.Column<int>(nullable: false),
                    OnPin = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Switches", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Flips_Switches_SwitchId",
                table: "Flips",
                column: "SwitchId",
                principalTable: "Switches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flips_Switches_SwitchId",
                table: "Flips");

            migrationBuilder.DropTable(
                name: "Remotes");

            migrationBuilder.DropTable(
                name: "Switches");

            migrationBuilder.RenameColumn(
                name: "SwitchId",
                table: "Flips",
                newName: "LightId");

            migrationBuilder.RenameIndex(
                name: "IX_Flips_SwitchId",
                table: "Flips",
                newName: "IX_Flips_LightId");

            migrationBuilder.CreateTable(
                name: "Lights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    OffPin = table.Column<int>(nullable: false),
                    OnPin = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lights", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Flips_Lights_LightId",
                table: "Flips",
                column: "LightId",
                principalTable: "Lights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
