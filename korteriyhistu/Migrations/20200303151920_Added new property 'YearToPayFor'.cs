using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace korteriyhistu.Migrations
{
    public partial class AddednewpropertyYearToPayFor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearToPayFor",
                table: "Bill",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearToPayFor",
                table: "Bill");
        }
    }
}
