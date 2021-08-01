using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class LootGenerator : DataObjectBase
    {
        public string MobName { get; set; }
        public string MobGuild { get; set; }
        public string MobFaction { get; set; }
        public int RegionID { get; set; }
        public string LootGeneratorClass { get; set; }
        public int ExclusivePriority { get; set; }

        public virtual Region Region { get; set; }
    }
}
