using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace myDadApp.Migrations
{
    public partial class Chores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chore",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Owner = table.Column<string>(nullable: true),
                    IsDone = table.Column<bool>(nullable: false),
                    CreateDt = table.Column<DateTime>(nullable: false),
                    CompleteDt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chore", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chore");
        }
    }
}
