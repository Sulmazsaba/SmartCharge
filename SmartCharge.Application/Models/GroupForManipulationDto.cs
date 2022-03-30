using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Models
{
    public class GroupForManipulationDto
    {

        public string Name { get; set; }

        public int CapacityInAmps { get; set; }
    }
}
