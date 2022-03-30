using SmartCharge.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Domain.Entities
{
    public class Group : EntityBase
    {
        public string Name { get; set; }

        public int CapacityInAmps { get; set; }

        public ICollection<ChargeStation> ChargeStations { get; set; } = new List<ChargeStation>();


    }
}
