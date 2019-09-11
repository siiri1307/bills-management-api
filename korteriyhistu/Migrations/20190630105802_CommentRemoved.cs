using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace korteriyhistu.Migrations
{
    public partial class CommentRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Bill");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "LogEntry",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "LogEntry");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Bill",
                nullable: true);
        }
    }
}
