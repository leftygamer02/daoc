using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class InstanceElement : DataObjectBase
    {
        public string InstanceID { get; set; }
        public string ClassType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Heading { get; set; }
        public int NpcTemplateID { get; set; }
    }
}
