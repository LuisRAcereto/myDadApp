using Microsoft.EntityFrameworkCore.Migrations;

namespace myDadApp.Migrations
{
    public partial class chorecount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChoreCnt",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChoreCnt",
                table: "AspNetUsers");
        }
    }
}
