using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Spell : DataObjectBase
    {
        public int ClientEffect { get; set; }
        public int Icon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Target { get; set; }
        public int Range { get; set; }
        public int Power { get; set; }
        public double CastTime { get; set; }
        public double Damage { get; set; }
        public int DamageType { get; set; }
        public string Type { get; set; }
        public int Duration { get; set; }
        public int Frequency { get; set; }
        public int Pulse { get; set; }
        public int PulsePower { get; set; }
        public int Radius { get; set; }
        public int RecastDelay { get; set; }
        public int ResurrectHealth { get; set; }
        public int ResurrectMana { get; set; }
        public double Value { get; set; }
        public int Concentration { get; set; }
        public int LifeDrainReturn { get; set; }
        public int AmnesiaChance { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public string Message3 { get; set; }
        public string Message4 { get; set; }
        public int InstrumentRequirement { get; set; }
        public int SpellGroup { get; set; }
        public int EffectGroup { get; set; }
        public int? SubSpellID { get; set; }
        public bool MoveCast { get; set; }
        public bool Uninterruptible { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsSecondary { get; set; }
        public bool AllowBolt { get; set; }
        public int SharedTimerGroup { get; set; }
        public string PackageID { get; set; }
        public bool IsFocus { get; set; }
        public int ToolTipId { get; set; }

        public virtual Spell SubSpell { get; set; }
        public virtual ICollection<SpellCustomValues> CustomValues { get; set; }

        public virtual ICollection<SpellEffect> SpellEffects { get; set; }

        public virtual ICollection<SpellLineSpell> SpellLineSpells { get; set; }
        public virtual ICollection<ItemSpell> ItemSpells { get; set; }
        public virtual ICollection<InventoryItemSpell> InventoryItemSpells { get; set; }
        public Spell()
        {
            CustomValues = new HashSet<SpellCustomValues>();
            SpellEffects = new HashSet<SpellEffect>();
            SpellLineSpells = new HashSet<SpellLineSpell>();
            ItemSpells = new HashSet<ItemSpell>();
            InventoryItemSpells = new HashSet<InventoryItemSpell>();
        }
    }
}
