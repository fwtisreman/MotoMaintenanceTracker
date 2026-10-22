using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoMaintenanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddMotorcycleToMaintenanceTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MotorcycleId",
                table: "MaintenanceTasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTasks_MotorcycleId",
                table: "MaintenanceTasks",
                column: "MotorcycleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTasks_Motorcycles_MotorcycleId",
                table: "MaintenanceTasks",
                column: "MotorcycleId",
                principalTable: "Motorcycles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTasks_Motorcycles_MotorcycleId",
                table: "MaintenanceTasks");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTasks_MotorcycleId",
                table: "MaintenanceTasks");

            migrationBuilder.DropColumn(
                name: "MotorcycleId",
                table: "MaintenanceTasks");
        }
    }
}
