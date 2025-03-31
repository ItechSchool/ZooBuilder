using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooBuilderBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddStreet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Building",
                columns: new[] { "Id", "AnimalId", "Capacity", "Costs", "MaxRevenue", "Name", "SizeHeight", "SizeWidth", "Type" },
                values: new object[] { 13, null, 0, 1, 0, "Street", 1, 1, "Path" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
