using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class ForeignKey_Assignations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignations_AbpUsers_InspectorId",
                table: "Assignations");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignations_Operations_OperationId",
                table: "Assignations");

            migrationBuilder.AlterColumn<int>(
                name: "OperationId",
                table: "Assignations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "InspectorId",
                table: "Assignations",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignations_AbpUsers_InspectorId",
                table: "Assignations",
                column: "InspectorId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignations_Operations_OperationId",
                table: "Assignations",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignations_AbpUsers_InspectorId",
                table: "Assignations");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignations_Operations_OperationId",
                table: "Assignations");

            migrationBuilder.AlterColumn<int>(
                name: "OperationId",
                table: "Assignations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "InspectorId",
                table: "Assignations",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Assignations_AbpUsers_InspectorId",
                table: "Assignations",
                column: "InspectorId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignations_Operations_OperationId",
                table: "Assignations",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
