using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class Approval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsFeedbackApproved",
                table: "CustomerQuests",
                type: "bit",
                nullable: false,
                defaultValueSql: "(CONVERT([bit],(1)))",
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsFeedbackApproved",
                table: "CustomerQuests",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "(CONVERT([bit],(1)))");
        }
    }
}
