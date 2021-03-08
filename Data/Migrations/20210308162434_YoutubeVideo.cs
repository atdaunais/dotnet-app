using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_app.Data.Migrations
{
    public partial class YoutubeVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YoutubeVideo",
                table: "Products",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YoutubeVideo",
                table: "Products");
        }
    }
}
