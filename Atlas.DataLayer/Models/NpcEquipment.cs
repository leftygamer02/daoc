using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class NpcEquipment : DataObjectBase
    {
        public int Slot { get; set; }
        public int Model { get; set; }
        public int Color { get; set; }
        public int Effect { get; set; }
        public int Expansion { get; set; }
        public int Emblem { get; set; }

        public virtual ICollection<NpcTemplate> Npcs { get; set; }

        public NpcEquipment()
        {
            Npcs = new HashSet<NpcTemplate>();
        }
    }
}
