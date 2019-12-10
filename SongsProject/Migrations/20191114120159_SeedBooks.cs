using Microsoft.EntityFrameworkCore.Migrations;

namespace SongsProject.Migrations
{
    public partial class SeedBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "ID", "Author", "Country", "Date", "Name" },
                values: new object[,]
                {
                    { 1, "Neta", "Israel", "#1 Eurovision", "Toy" },
                    { 2, "Neta", "Israel", "#1 Eurovision", "Toy" },
                    { 3, "Neta", "Israel", "#1 Eurovision", "Toy" },
                    { 4, "Neta", "Israel", "#1 Eurovision", "Toy" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "ID",
                keyValue: 4);
        }
    }
}
