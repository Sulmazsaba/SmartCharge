using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Application.Models
{
    public class ConnectorForManipulationDto
    {
        public int MaxCurrentInAmps { get; set; }

        public int ConnectorId { get; set; }
    }
}
