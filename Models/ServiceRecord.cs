using System.ComponentModel.DataAnnotations;

namespace MotoMaintenanceTracker.Models
{
    public class ServiceRecord
    {
        public int Id { get; set; }

        public int MotorcycleId { get; set; }

        public Motorcycle? Motorcycle { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Service type")]
        public string ServiceType { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Service date")]
        public DateTime ServiceDate { get; set; }

        [Range(0, 1000000)]
        [Display(Name = "Mileage (km)")]
        public int Mileage { get; set; }

        [Range(0, 100000)]
        public decimal Cost { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}