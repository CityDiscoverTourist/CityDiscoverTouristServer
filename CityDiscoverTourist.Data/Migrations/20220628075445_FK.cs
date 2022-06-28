using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerAnswers",
                table: "CustomerAnswers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CustomerAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerAnswers",
                table: "CustomerAnswers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAnswers_QuestItemId",
                table: "CustomerAnswers",
                column: "QuestItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerAnswers",
                table: "CustomerAnswers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAnswers_QuestItemId",
                table: "CustomerAnswers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomerAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerAnswers",
                table: "CustomerAnswers",
                columns: new[] { "QuestItemId", "CustomerTaskId" });
        }
    }
}
