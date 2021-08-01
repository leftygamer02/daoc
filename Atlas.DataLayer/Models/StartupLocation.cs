using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class StartupLocation : DataObjectBase
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int ZPos { get; set; }
        public int Heading { get; set; }
        public int RegionID { get; set; }
        public int MinVersion { get; set; }
        public int Realm { get; set; }
        public int RaceID { get; set; }
        public int ClassID { get; set; }
        public int ClientRegionID { get; set; }

        public virtual Region Region { get; set; }
        public virtual Region ClientRegion { get; set; }
    }
}
