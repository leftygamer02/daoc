using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class NpcSpawnGroup : DataObjectBase
    {
        public int NpcTemplateID { get; set; }
        public int SpawnGroupID { get; set; }
        public int SpawnChance { get; set; } //integer percent, 0-100

        public virtual NpcTemplate NpcTemplate { get; set; }
        public virtual SpawnGroup SpawnGroup { get; set; }

    }
}
