using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooBuilderBackend.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GridPlacement_Animal_AnimalId",
                table: "GridPlacement");

            migrationBuilder.DropIndex(
                name: "IX_GridPlacement_AnimalId",
                table: "GridPlacement");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "GridPlacement");

            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "GridPlacement");

            migrationBuilder.AddColumn<bool>(
                name: "Connected",
                table: "GridPlacement",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "Building",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Revenue",
                table: "Building",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Building_AnimalId",
                table: "Building",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Building_Animal_AnimalId",
                table: "Building",
                column: "AnimalId",
                principalTable: "Animal",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Building_Animal_AnimalId",
                table: "Building");

            migrationBuilder.DropIndex(
                name: "IX_Building_AnimalId",
                table: "Building");

            migrationBuilder.DropColumn(
                name: "Connected",
                table: "GridPlacement");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Building");

            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "Building");

            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "GridPlacement",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Revenue",
                table: "GridPlacement",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_GridPlacement_AnimalId",
                table: "GridPlacement",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_GridPlacement_Animal_AnimalId",
                table: "GridPlacement",
                column: "AnimalId",
                principalTable: "Animal",
                principalColumn: "Id");
        }
    }
}
