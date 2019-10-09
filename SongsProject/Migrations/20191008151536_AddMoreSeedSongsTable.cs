using Microsoft.EntityFrameworkCore.Migrations;

namespace SongsProject.Migrations
{
    public partial class AddMoreSeedSongsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Artist", "Country", "Description", "ImagePath", "MusicStyle", "Name", "Price", "Rating" },
                values: new object[] { 2, "Jennifer Lopez", "USA", "#1 USA", null, "Pop", "On the floor", 21m, 0 });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Artist", "Country", "Description", "ImagePath", "MusicStyle", "Name", "Price", "Rating" },
                values: new object[] { 3, "Ariana Grande", "USA", "#1 USA", null, "Pop", "7 Rings", 22m, 0 });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Artist", "Country", "Description", "ImagePath", "MusicStyle", "Name", "Price", "Rating" },
                values: new object[] { 4, "Kobi Marimi", "Israel", "#23 Eurovision", null, "Pop", "Home", 11m, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
