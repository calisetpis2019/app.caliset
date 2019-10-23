using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class cambioenSample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InspectorId",
                table: "Samples",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2019, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2019, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationTime",
                value: new DateTime(2019, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_Samples_InspectorId",
                table: "Samples",
                column: "InspectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_AbpUsers_InspectorId",
                table: "Samples",
                column: "InspectorId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Samples_AbpUsers_InspectorId",
                table: "Samples");

            migrationBuilder.DropIndex(
                name: "IX_Samples_InspectorId",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "InspectorId",
                table: "Samples");

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2019, 10, 22, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2019, 10, 22, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationTime",
                value: new DateTime(2019, 10, 22, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
