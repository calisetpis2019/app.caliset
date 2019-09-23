using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class Operationsconpocoscampos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_Operations_OperationId",
                table: "AbpUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Operations_OperationId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Samples_Operations_OperationId",
                table: "Samples");

            migrationBuilder.DropIndex(
                name: "IX_Samples_OperationId",
                table: "Samples");

            migrationBuilder.DropIndex(
                name: "IX_Comments_OperationId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_OperationId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "OperationId",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "OperationId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "OperationId",
                table: "AbpUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OperationId",
                table: "Samples",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperationId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperationId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Samples_OperationId",
                table: "Samples",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OperationId",
                table: "Comments",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_OperationId",
                table: "AbpUsers",
                column: "OperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_Operations_OperationId",
                table: "AbpUsers",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Operations_OperationId",
                table: "Comments",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_Operations_OperationId",
                table: "Samples",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
