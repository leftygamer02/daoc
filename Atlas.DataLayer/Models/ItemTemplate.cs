using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ItemTemplate : DataObjectBase
    {
        public string TranslationId { get; set; }
        public string Name { get; set; }        
        public int Level { get; set; }
        public int Durability { get; set; }
        public int MaxDurability { get; set; }
        public bool IsNotLosingDur { get; set; }
        public int Condition { get; set; }
        public int MaxCondition { get; set; }
        public int Quality { get; set; }
        public int DPS_AF { get; set; }
        public int SPD_ABS { get; set; }
        public int Hand { get; set; }
        public int TypeDamage { get; set; }
        public int ObjectType { get; set; }
        public int ItemType { get; set; }
        public int Color { get; set; }
        public int Emblem { get; set; }
        public int Effect { get; set; }
        public int Weight { get; set; }
        public int Model { get; set; }
        public int Extension { get; set; }
        public int CanUseEvery { get; set; }
        public bool IsPickable { get; set; }
        public bool IsDropable { get; set; }
        public bool CanDropAsLoot { get; set; }
        public bool IsTradable { get; set; }
        public long Price { get; set; }
        public int MaxCount { get; set; }
        public bool IsIndestructible { get; set; }
        public int PackSize { get; set; }
        public int Realm { get; set; }
        public string AllowedClasses { get; set; }
        public int Flags { get; set; }
        public int BonusLevel { get; set; }
        public int LevelRequirement { get; set; }
        public string PackageID { get; set; }
        public string Description { get; set; }
        public string ClassType { get; set; }
        public int SavageYieldID { get; set; }


        public virtual SalvageYield SalvageYield { get; set; }
        public virtual ICollection<ItemBonus> Bonuses { get; set; }
        public virtual ICollection<ItemSpell> Spells { get; set; }

        public ItemTemplate()
        {
            Bonuses = new HashSet<ItemBonus>();
            Spells = new HashSet<ItemSpell>();
        }
    }
}
