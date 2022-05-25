using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class UpdateEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerQuests_Competitions_CompetitionId",
                table: "CustomerQuests");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerQuests_CompetitionId",
                table: "CustomerQuests");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "CustomerQuests");

            migrationBuilder.DropColumn(
                name: "TeamCode",
                table: "CustomerQuests");

            migrationBuilder.AddColumn<int>(
                name: "CountSuggestion",
                table: "CustomerTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountSuggestion",
                table: "CustomerTasks");

            migrationBuilder.AddColumn<int>(
                name: "CompetitionId",
                table: "CustomerQuests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TeamCode",
                table: "CustomerQuests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    CompetitionCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitions_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerQuests_CompetitionId",
                table: "CustomerQuests",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_QuestId",
                table: "Competitions",
                column: "QuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerQuests_Competitions_CompetitionId",
                table: "CustomerQuests",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
