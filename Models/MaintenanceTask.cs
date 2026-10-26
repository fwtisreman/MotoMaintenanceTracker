using System.ComponentModel.DataAnnotations;

namespace MotoMaintenanceTracker.Models
{
    public class MaintenanceTask
    {
        public int Id { get; set; }

        public int? MotorcycleId { get; set; }

        public Motorcycle? Motorcycle { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Due date")]
        public DateTime? DueDate { get; set; }

        [Range(0, 1000000)]
        [Display(Name = "Due mileage (km)")]
        public int? DueMileage { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }

        // Used by the reminder system so each reminder is sent only once
        public bool SevenDayReminderSent { get; set; }
        public bool OneDayReminderSent { get; set; }
        public bool DueDateReminderSent { get; set; }
    }
}