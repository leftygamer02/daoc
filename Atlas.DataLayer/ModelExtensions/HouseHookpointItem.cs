using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.DataLayer.Models
{    
    public partial class HouseHookpointItem
    {
        [NotMapped]
        public object GameObject { get; set; }
    }
}
