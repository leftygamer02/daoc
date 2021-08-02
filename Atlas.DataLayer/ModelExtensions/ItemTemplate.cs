using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.DataLayer.Models
{
    public partial class ItemTemplate 
    {
        [NotMapped]
        public bool AllowUpdate { get; set; }

        [NotMapped]
        public bool IsStackable
        {
            get { return this.MaxCount > 1; }
        }
    }
}
