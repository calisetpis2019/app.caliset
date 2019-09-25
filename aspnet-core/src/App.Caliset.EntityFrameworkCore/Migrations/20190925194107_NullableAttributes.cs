using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class NullableAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Commentary",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Aware",
                table: "Assignations",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Commentary",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<bool>(
                name: "Aware",
                table: "Assignations",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
