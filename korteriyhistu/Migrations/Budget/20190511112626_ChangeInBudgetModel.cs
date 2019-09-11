using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace korteriyhistu.Migrations.Budget
{
    public partial class ChangeInBudgetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "size",
                table: "Budget",
                newName: "sum");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sum",
                table: "Budget",
                newName: "size");
        }
    }
}
