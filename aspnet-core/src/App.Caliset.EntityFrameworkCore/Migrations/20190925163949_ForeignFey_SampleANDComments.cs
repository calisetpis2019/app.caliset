using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class ForeignFey_SampleANDComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdSample",
                table: "Samples",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperationId",
                table: "Samples",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OperationId",
                table: "Comments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Samples_OperationId",
                table: "Samples",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OperationId",
                table: "Comments",
                column: "OperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Operations_OperationId",
                table: "Comments",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_Operations_OperationId",
                table: "Samples",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "IdSample",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "OperationId",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "OperationId",
                table: "Comments");
        }
    }
}
