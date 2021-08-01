using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Relic : DataObjectBase
    {
        public int RegionID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Heading { get; set; }
        public int Realm { get; set; }
        public int OriginalRealm { get; set; }
        public int LastRealm { get; set; }
        public int RelicType { get; set; }

        public virtual Region Region { get; set; }
    }
}
