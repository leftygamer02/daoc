using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class BindPoint : DataObjectBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Radius { get; set; }
        public int RegionID { get; set; }
        public int Realm { get; set; }
    }
}
