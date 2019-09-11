using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace korteriyhistu.Migrations.LogEntries
{
    public partial class RemovedBillReferenceNavProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntry_Bill_BillId",
                table: "LogEntry");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_LogEntry_BillId",
                table: "LogEntry");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    BillId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Apartment = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    MonthToPayFor = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    PartialPayAmount = table.Column<double>(nullable: false),
                    PaymentDeadline = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SumToPay = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.BillId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogEntry_BillId",
                table: "LogEntry",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntry_Bill_BillId",
                table: "LogEntry",
                column: "BillId",
                principalTable: "Bill",
                principalColumn: "BillId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
