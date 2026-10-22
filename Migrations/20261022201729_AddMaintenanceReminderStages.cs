using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoMaintenanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceReminderStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DueDateReminderSent",
                table: "MaintenanceTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OneDayReminderSent",
                table: "MaintenanceTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SevenDayReminderSent",
                table: "MaintenanceTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDateReminderSent",
                table: "MaintenanceTasks");

            migrationBuilder.DropColumn(
                name: "OneDayReminderSent",
                table: "MaintenanceTasks");

            migrationBuilder.DropColumn(
                name: "SevenDayReminderSent",
                table: "MaintenanceTasks");
        }
    }
}
