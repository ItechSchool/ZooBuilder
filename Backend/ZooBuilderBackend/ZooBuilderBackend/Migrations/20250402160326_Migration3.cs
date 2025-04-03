using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooBuilderBackend.Migrations
{
    /// <inheritdoc />
    public partial class Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GridPlacement_Building_BuildingId",
                table: "GridPlacement");

            migrationBuilder.AddColumn<int>(
                name: "Meat",
                table: "Zoo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "GridPlacement",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GridPlacement_Building_BuildingId",
                table: "GridPlacement",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GridPlacement_Building_BuildingId",
                table: "GridPlacement");

            migrationBuilder.DropColumn(
                name: "Meat",
                table: "Zoo");

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "GridPlacement",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_GridPlacement_Building_BuildingId",
                table: "GridPlacement",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id");
        }
    }
}
