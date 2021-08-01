using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class LinkedFaction : DataObjectBase
    {
        public int FactionID { get; set; }
        public int RelatedFactionID { get; set; }
        public bool IsFriend { get; set; }

        public virtual Faction Faction { get; set; }
        public virtual Faction RelatedFaction { get; set; }
    }
}
