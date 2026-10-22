using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MotoMaintenanceTracker.Models
{
    public class ServiceRecord : IValidatableObject
    {
        public int Id { get; set; }
        public int MotorcycleId { get; set; }
        [ValidateNever]

        public Motorcycle Motorcycle { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string ServiceType { get; set; } = string.Empty;


        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; }

        [Required]
        [Range(0, 1000000)]
        public int Mileage { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "100000")]
        public decimal Cost { get; set; }

        [StringLength(500)]

        public string? Notes { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ServiceDate.Date > DateTime.Today)
            {
                yield return new ValidationResult(
                    "Service date cannot be in the future.",
                    new[] { nameof(ServiceDate) });
            }
        }


    }
}
