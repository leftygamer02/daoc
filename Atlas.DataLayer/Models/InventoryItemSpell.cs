using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class InventoryItemSpell : DataObjectBase
    {
        public int InventoryItemID { get; set; }
        public int SpellID { get; set; }
        public int Charges { get; set; }
        public int MaxCharges { get; set; }
        public int ProcChance { get; set; }
        public bool IsPoison { get; set; }

        public virtual InventoryItem InventoryItem { get; set; }
        public virtual Spell Spell { get; set; }
    }
}
