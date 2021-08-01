using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class FactionAggroLevel : DataObjectBase
    {
        public int CharacterID { get; set; }
        public int FactionID { get; set; }
        public int AggroLevel { get; set; }

        public virtual Character Character { get; set; }
        public virtual Faction Faction { get; set; }
    }
}
