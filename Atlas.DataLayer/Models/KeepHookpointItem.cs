using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class KeepHookpointItem : DataObjectBase
    {
        public int KeepID { get; set; }
        public int ComponentID { get; set; }
        public int HookPointID { get; set; }
        public string ClassType { get; set; }

        public virtual Keep Keep { get; set; }
        public virtual KeepComponent Component { get; set; }
        
    }
}
