using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZooBuilderBackend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Species = table.Column<string>(type: "text", nullable: false),
                    Costs = table.Column<int>(type: "integer", nullable: false),
                    Hunger = table.Column<int>(type: "integer", nullable: false),
                    Diet = table.Column<string>(type: "text", nullable: false),
                    Attraction = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    SizeHeight = table.Column<int>(type: "integer", nullable: false),
                    SizeWidth = table.Column<int>(type: "integer", nullable: false),
                    Costs = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    MaxRevenue = table.Column<int>(type: "integer", nullable: false),
                    AnimalId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Building_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zoo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Money = table.Column<int>(type: "integer", nullable: false),
                    Vegetables = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zoo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zoo_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GridPlacement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    XCoordinate = table.Column<int>(type: "integer", nullable: false),
                    YCoordinate = table.Column<int>(type: "integer", nullable: false),
                    AnimalCount = table.Column<int>(type: "integer", nullable: false),
                    Connected = table.Column<bool>(type: "boolean", nullable: false),
                    ZooId = table.Column<int>(type: "integer", nullable: false),
                    BuildingId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridPlacement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GridPlacement_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GridPlacement_Zoo_ZooId",
                        column: x => x.ZooId,
                        principalTable: "Zoo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Animal",
                columns: new[] { "Id", "Attraction", "Costs", "Diet", "Hunger", "Species" },
                values: new object[,]
                {
                    { 1, 5, 15, "Meat", 4, "Lion" },
                    { 2, 4, 12, "Vegetables", 3, "Giraffe" },
                    { 3, 5, 20, "Vegetables", 4, "Elephant" },
                    { 4, 2, 8, "Fish", 2, "Penguin" },
                    { 5, 4, 12, "Vegetables", 2, "Kangaroo" },
                    { 6, 3, 10, "Vegetables", 3, "Zebra" },
                    { 7, 5, 18, "Bamboo", 2, "Panda" },
                    { 8, 1, 3, "Meat", 1, "Rat" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Building_AnimalId",
                table: "Building",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_GridPlacement_BuildingId",
                table: "GridPlacement",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_GridPlacement_ZooId",
                table: "GridPlacement",
                column: "ZooId");

            migrationBuilder.CreateIndex(
                name: "IX_Zoo_PlayerId",
                table: "Zoo",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GridPlacement");

            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropTable(
                name: "Zoo");

            migrationBuilder.DropTable(
                name: "Animal");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
