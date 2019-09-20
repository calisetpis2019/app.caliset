using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Caliset.Migrations
{
    public partial class claseOperacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Commodity = table.Column<string>(nullable: true),
                    Package = table.Column<string>(nullable: true),
                    ShipName = table.Column<string>(nullable: true),
                    Destiny = table.Column<string>(nullable: true),
                    Line = table.Column<string>(nullable: true),
                    BookingNumber = table.Column<string>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    OperationTypeId = table.Column<int>(nullable: true),
                    NominadorId = table.Column<int>(nullable: true),
                    CargadorId = table.Column<int>(nullable: true),
                    OperationStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Clients_CargadorId",
                        column: x => x.CargadorId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Clients_NominadorId",
                        column: x => x.NominadorId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_OperationStates_OperationStateId",
                        column: x => x.OperationStateId,
                        principalTable: "OperationStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_OperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CargadorId",
                table: "Operations",
                column: "CargadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_LocationId",
                table: "Operations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_NominadorId",
                table: "Operations",
                column: "NominadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_OperationStateId",
                table: "Operations",
                column: "OperationStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_OperationTypeId",
                table: "Operations",
                column: "OperationTypeId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "Operations");

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
    }
}
