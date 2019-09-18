using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AbpUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Document",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Document",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AbpUsers");
        }
    }
}
