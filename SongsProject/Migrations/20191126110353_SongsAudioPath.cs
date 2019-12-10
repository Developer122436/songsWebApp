using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SongsProject.Migrations
{
    public partial class SongsAudioPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.AddColumn<string>(
                name: "AudioPath",
                table: "Songs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioPath",
                table: "Songs");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.ID);
                });

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
    }
}
