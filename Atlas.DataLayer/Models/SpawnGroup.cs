using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class SpawnGroup : DataObjectBase
    {        
        public string ClassType { get; set; }
        public string Name { get; set; }
        public int DayNightSpawn { get; set; } //0 = any, 1 = day, 2 = night

        //other camp properties here?
        

        public virtual ICollection<NpcSpawnGroup> NpcSpawnGroups { get; set; }
        public virtual ICollection<SpawnPoint> SpawnPoints { get; set; }
        public SpawnGroup()
        {
            NpcSpawnGroups = new HashSet<NpcSpawnGroup>();
            SpawnPoints = new HashSet<SpawnPoint>();
        }
    }
}
