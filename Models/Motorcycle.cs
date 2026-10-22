using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;   

namespace MotoMaintenanceTracker.Models
{
    public class Motorcycle
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;
        [Required]
        [Range(1900,2027)]
        public int Year { get; set; }
        [Required]
        [Range(0,1000000)]
        public int Mileage { get; set; }

        public ICollection<ServiceRecord> ServiceRecords { get; set; } = new List<ServiceRecord>();
    }
}
