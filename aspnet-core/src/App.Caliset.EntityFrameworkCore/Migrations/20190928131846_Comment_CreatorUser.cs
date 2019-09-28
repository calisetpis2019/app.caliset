using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class Comment_CreatorUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2019, 9, 28, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2019, 9, 28, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationTime",
                value: new DateTime(2019, 9, 28, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatorUserId",
                table: "Comments",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AbpUsers_CreatorUserId",
                table: "Comments",
                column: "CreatorUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AbpUsers_CreatorUserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CreatorUserId",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2019, 9, 27, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2019, 9, 27, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationTime",
                value: new DateTime(2019, 9, 27, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
