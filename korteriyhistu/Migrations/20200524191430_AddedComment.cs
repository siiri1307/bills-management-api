using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace korteriyhistu.Migrations
{
    public partial class AddedComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Bill",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Bill");
        }
    }
}
