using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Zone : DataObjectBase
    {
        public int RegionID { get; set; }
        public string Name { get; set; }
        public bool IsLava { get; set; }
        public int DivingFlag { get; set; }
        public int WaterLevel { get; set; }
        public int OffsetY { get; set; }
        public int OffsetX { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Experience { get; set; }
        public int Realmpoints { get; set; }
        public int Bountypoints { get; set; }
        public int Coin { get; set; }
        public int Realm { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Artifact> Artifacts { get; set; }

        public Zone()
        {
            Artifacts = new HashSet<Artifact>();
        }
    }
}
