using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class Correct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecievedDate",
                table: "Rewards",
                newName: "ReceivedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceivedDate",
                table: "Rewards",
                newName: "RecievedDate");
        }
    }
}