using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class InventoryItem : DataObjectBase
    {
        public int? CharacterID { get; set; }
        public int ItemTemplateID { get; set; }
        public int ItemUniqueID { get; set; }
        public int OwnerLot { get; set; }
        public bool IsCrafted { get; set; }
        public string Creator { get; set; }
        public int SlotPosition { get; set; }
        public int Count { get; set; }
        public int SellPrice { get; set; }
        public long Experience { get; set; }
        public int Color { get; set; }
        public int Emblem { get; set; }
        public int Extension { get; set; }
        public int Condition { get; set; }
        public int Durability { get; set; }
        public int Cooldown { get; set; }
        public int ItemBonus { get; set; }

        public virtual Character Character { get; set; }
        public virtual ItemTemplate ItemTemplate { get; set; }
        public virtual ItemUnique ItemUnique { get; set; }
        public virtual ICollection<InventoryItemSpell> Spells { get; set; }

        public InventoryItem()
        {
            Spells = new HashSet<InventoryItemSpell>();
        }

    }
}
