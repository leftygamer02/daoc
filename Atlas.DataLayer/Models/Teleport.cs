using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Teleport : DataObjectBase
    {
        public string Type { get; set; }
        public string TeleportID { get; set; }
        public int Realm { get; set; }
        public int RegionID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Heading { get; set; }

        public virtual Region Region { get; set; }
    }
}
