using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZooBuilderBackend.Migrations
{
    /// <inheritdoc />
    public partial class PopulateBuildings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Building",
                columns: new[] { "Id", "AnimalId", "Capacity", "Costs", "MaxRevenue", "Name", "SizeHeight", "SizeWidth", "Type" },
                values: new object[,]
                {
                    { 1, 1, 5, 30, 0, "Lion Den", 4, 4, "Enclosure" },
                    { 2, 2, 4, 35, 0, "Giraffe Enclosure", 6, 6, "Enclosure" },
                    { 3, 3, 3, 50, 0, "Elephant Habitat", 8, 8, "Enclosure" },
                    { 4, 4, 10, 20, 0, "Penguin Cove", 3, 3, "Enclosure" },
                    { 5, 5, 6, 25, 0, "Kangaroo Pen", 5, 5, "Enclosure" },
                    { 6, 6, 6, 25, 0, "Zebra Zone", 5, 5, "Enclosure" },
                    { 7, 7, 2, 40, 0, "Panda Sanctuary", 6, 6, "Enclosure" },
                    { 8, 8, 1, 5, 0, "Rat Cage", 1, 1, "Enclosure" },
                    { 9, null, 300, 10, 30, "Food Truck", 2, 1, "Snack" },
                    { 10, null, 100, 8, 20, "Ice Cream Stand", 2, 1, "Snack" },
                    { 11, null, 100, 7, 15, "Lemonade Stand", 2, 1, "Snack" },
                    { 12, null, 300, 20, 50, "Jungle Cafe", 6, 4, "Snack" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
