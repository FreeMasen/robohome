using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace projects.Migrations
{
    public partial class fix_kt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KeyTimes",
                table: "KeyTimes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "KeyTimes",
                type: "int4",
                nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_KeyTimes",
                table: "KeyTimes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KeyTimes",
                table: "KeyTimes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "KeyTimes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KeyTimes",
                table: "KeyTimes",
                column: "Date");
        }
    }
}
