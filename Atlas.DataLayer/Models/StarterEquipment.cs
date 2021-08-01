using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class StarterEquipment : DataObjectBase
    {
        public int ClassID { get; set; }
        public int ItemTemplateID { get; set; }

        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
