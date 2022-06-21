using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class AddFieldPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggerMode",
                table: "QuestItems");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TotalAmount",
                table: "Payments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "TriggerMode",
                table: "QuestItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}