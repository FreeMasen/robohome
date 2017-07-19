using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace projects.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PinId = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Direction = table.Column<int>(nullable: false),
                    Hour = table.Column<int>(nullable: false),
                    LightId = table.Column<int>(nullable: true),
                    Minute = table.Column<int>(nullable: false),
                    TimeOfDay = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flips_Lights_LightId",
                        column: x => x.LightId,
                        principalTable: "Lights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flips_LightId",
                table: "Flips",
                column: "LightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flips");

            migrationBuilder.DropTable(
                name: "Lights");
        }
    }
}
