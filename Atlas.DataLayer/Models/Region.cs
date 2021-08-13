using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Region : DataObjectBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public int Expansion { get; set; }
        public bool HousingEnabled { get; set; }
        public bool DivingEnabled { get; set; }
        public int WaterLevel { get; set; }
        public string ClassType { get; set; }
        public bool IsFrontier { get; set; }

        public virtual ICollection<Zone> Zones { get; set; }
        public virtual ICollection<SpawnGroup> SpawnGroups { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Battleground> Battlegrounds { get; set; }
        public virtual ICollection<BindPoint> BindPoints { get; set; }
        public virtual ICollection<ZonePoint> ZonePointSources { get; set; }
        public virtual ICollection<ZonePoint> ZonePointTargets { get; set; }

        public Region()
        {
            Areas = new HashSet<Area>();
            Zones = new HashSet<Zone>();
            SpawnGroups = new HashSet<SpawnGroup>();
            Characters = new HashSet<Character>();
            Battlegrounds = new HashSet<Battleground>();
            BindPoints = new HashSet<BindPoint>();
            ZonePointSources = new HashSet<ZonePoint>();
            ZonePointTargets = new HashSet<ZonePoint>();
        }
    }
}
