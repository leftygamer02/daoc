using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class MinotaurRelic : DataObjectBase
    {
        public int RelicSpell { get; set; }
        public bool SpawnLocked { get; set; }
        public string ProtectorClassType { get; set; }
        public string RelicTarget { get; set; }
        public string Name { get; set; }
        public int Model { get; set; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
        public int SpawnZ { get; set; }
        public int SpawnHeading { get; set; }
        public int SpawnRegion { get; set; }
        public int Effect { get; set; }

    }
}
