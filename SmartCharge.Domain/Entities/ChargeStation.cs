using SmartCharge.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Domain.Entities
{
    public class ChargeStation : EntityBase
    {
        public string Name { get; set; }
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        [Required]
        public Group Group { get; set; }
        public ICollection<Connector> Connectors { get; set; } = new List<Connector>();


    }
}
