using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OperationStates",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name" },
                values: new object[] { 1, new DateTime(2019, 9, 27, 0, 0, 0, 0, DateTimeKind.Local), null, null, null, false, null, null, "Futura" });

            migrationBuilder.InsertData(
                table: "OperationStates",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name" },
                values: new object[] { 2, new DateTime(2019, 9, 27, 0, 0, 0, 0, DateTimeKind.Local), null, null, null, false, null, null, "Activa" });

            migrationBuilder.InsertData(
                table: "OperationStates",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name" },
                values: new object[] { 3, new DateTime(2019, 9, 27, 0, 0, 0, 0, DateTimeKind.Local), null, null, null, false, null, null, "Finalizada" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OperationStates",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
