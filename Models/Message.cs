
using System.ComponentModel.DataAnnotations;

namespace MotoMaintenanceTracker.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int? MaintenanceTaskId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsDismissed { get; set; }

        

        public bool IsRead { get; set; }

        [StringLength(50)]
        public string Type { get; set; } = "General";
    }
}
