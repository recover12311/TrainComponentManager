using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrainComponentManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanAssignQuantity = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainComponents", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TrainComponents",
                columns: new[] { "Id", "CanAssignQuantity", "Name", "Quantity", "UniqueNumber" },
                values: new object[,]
                {
                    { 1, false, "Engine", null, "ENG123" },
                    { 2, false, "Passenger Car", null, "PAS456" },
                    { 3, false, "Freight Car", null, "FRT789" },
                    { 4, true, "Wheel", null, "WHL101" },
                    { 5, true, "Seat", null, "STS234" },
                    { 6, true, "Window", null, "WIN567" },
                    { 7, true, "Door", null, "DR123" },
                    { 8, true, "Control Panel", null, "CTL987" },
                    { 9, true, "Light", null, "LGT456" },
                    { 10, true, "Brake", null, "BRK789" },
                    { 11, true, "Bolt", null, "BLT321" },
                    { 12, true, "Nut", null, "NUT654" },
                    { 13, false, "Engine Hood", null, "EH789" },
                    { 14, false, "Axle", null, "AX456" },
                    { 15, false, "Piston", null, "PST789" },
                    { 16, true, "Handrail", null, "HND234" },
                    { 17, true, "Step", null, "STP567" },
                    { 18, false, "Roof", null, "RF123" },
                    { 19, false, "Air Conditioner", null, "AC789" },
                    { 20, false, "Flooring", null, "FLR456" },
                    { 21, true, "Mirror", null, "MRR789" },
                    { 22, false, "Horn", null, "HRN321" },
                    { 23, false, "Coupler", null, "CPL654" },
                    { 24, true, "Hinge", null, "HNG987" },
                    { 25, true, "Ladder", null, "LDR456" },
                    { 26, false, "Paint", null, "PNT789" },
                    { 27, true, "Decal", null, "DCL321" },
                    { 28, true, "Gauge", null, "GGS654" },
                    { 29, false, "Battery", null, "BTR987" },
                    { 30, false, "Radiator", null, "RDR456" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainComponents");
        }
    }
}
