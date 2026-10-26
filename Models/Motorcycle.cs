using System.ComponentModel.DataAnnotations;

namespace MotoMaintenanceTracker.Models
{
    public class Motorcycle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Range(0, 1000000)]
        [Display(Name = "Mileage (km)")]
        public int Mileage { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<ServiceRecord> ServiceRecords { get; set; } = new List<ServiceRecord>();
    }
}