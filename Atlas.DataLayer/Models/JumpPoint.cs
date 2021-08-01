using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class JumpPoint : DataObjectBase
    {
        public string Name { get; set; }
        public int RegionID { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public int Zpos { get; set; }
        public int Heading { get; set; }

        public virtual Region Region { get; set; }
    }
}
