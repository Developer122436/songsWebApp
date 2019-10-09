using Microsoft.EntityFrameworkCore.Migrations;

namespace SongsProject.Migrations
{
    public partial class SeedSongsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Artist", "Country", "Description", "ImagePath", "MusicStyle", "Name", "Price", "Rating" },
                values: new object[] { 1, "Neta", "Israel", "#1 Eurovision", null, "Pop", "Toy", 18m, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
