using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ZonePoint : DataObjectBase
    {
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public int TargetZ { get; set; }
        public int TargetRegionID { get; set; }
        public int TargetHeading { get; set; }
        public int SourceX { get; set; }
        public int SourceY { get; set; }
        public int SourceZ { get; set; }
        public int? SourceRegionID { get; set; }
        public int Realm { get; set; }
        public string ClassType { get; set; }
        public int ClientID { get; set; }

        public virtual Region TargetRegion { get; set; }
        public virtual Region SourceRegion { get; set; }

    }
}
