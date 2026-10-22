using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoMaintenanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class LinkMessagesToMaintenanceTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaintenanceTaskId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MaintenanceTaskId",
                table: "Messages",
                column: "MaintenanceTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MaintenanceTasks_MaintenanceTaskId",
                table: "Messages",
                column: "MaintenanceTaskId",
                principalTable: "MaintenanceTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MaintenanceTasks_MaintenanceTaskId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MaintenanceTaskId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MaintenanceTaskId",
                table: "Messages");
        }
    }
}
