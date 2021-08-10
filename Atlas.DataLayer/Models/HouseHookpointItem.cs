using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class HouseHookpointItem : DataObjectBase
    {
        
        public int HouseID { get; set; }
        public int HookpointID { get; set; }
        public int ItemTemplateID { get; set; }        
        public int HouseNumber { get; set; }
        public int Heading { get; set; }
        public int Index { get; set; }

        public virtual DbHouse House { get; set; }
        public virtual ItemTemplate ItemTemplate { get; set; }
        
    }
}
