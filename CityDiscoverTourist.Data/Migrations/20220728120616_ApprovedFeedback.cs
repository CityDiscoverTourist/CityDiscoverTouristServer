using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class ApprovedFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeedbackApproved",
                table: "CustomerQuests",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeedbackApproved",
                table: "CustomerQuests");
        }
    }
}