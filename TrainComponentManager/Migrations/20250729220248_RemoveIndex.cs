using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainComponentManager.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainComponents_CanAssignQuantity",
                table: "TrainComponents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TrainComponents_CanAssignQuantity",
                table: "TrainComponents",
                column: "CanAssignQuantity");
        }
    }
}
