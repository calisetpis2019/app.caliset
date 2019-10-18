using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class Assignation_DateFin_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFin",
                table: "Assignations",
                nullable: true,
                oldClrType: typeof(DateTime));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFin",
                table: "Assignations",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

        }
    }
}
