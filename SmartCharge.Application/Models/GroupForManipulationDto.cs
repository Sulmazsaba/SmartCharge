using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Models
{
    public class GroupForManipulationDto : IValidatableObject
    {

        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity In Amps should be greater than zero")]
        public int CapacityInAmps { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CapacityInAmps <= 0)
                yield return new ValidationResult("Capacity In Amps should be greater than zero");
        }
    }
}
