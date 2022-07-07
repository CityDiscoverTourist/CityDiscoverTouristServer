using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscoverTourist.Data.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rewards_RewardId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RewardId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "RewardId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RewardId",
                table: "Payments",
                column: "RewardId",
                unique: true,
                filter: "[RewardId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rewards_RewardId",
                table: "Payments",
                column: "RewardId",
                principalTable: "Rewards",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rewards_RewardId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RewardId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "RewardId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RewardId",
                table: "Payments",
                column: "RewardId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rewards_RewardId",
                table: "Payments",
                column: "RewardId",
                principalTable: "Rewards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
