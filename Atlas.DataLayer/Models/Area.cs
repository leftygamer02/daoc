using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Area : DataObjectBase
    {
        public string TranslationID { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Radius { get; set; }
        public int RegionID { get; set; }
        public string ClassType { get; set; }
        public bool CanBroadcast { get; set; }
        public short Sound { get; set; }
        public bool CheckLOS { get; set; }
        public string Points { get; set; }

        public virtual Region Region { get; set; }
    }
}
