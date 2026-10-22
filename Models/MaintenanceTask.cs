

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
        public DateTime? DueDate { get; set; }

        [Range(0, 1000000)]
        public int? DueMileage { get; set; }

        public bool IsCompleted { get; set; }
        public bool SevenDayReminderSent { get; set; }
        public bool OneDayReminderSent { get; set; }
        public bool DueDateReminderSent { get; set; }
    }
}

