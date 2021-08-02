using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Style : DataObjectBase
    {
        public int ClassID { get; set; }
        public int SpecializationID { get; set; }
        public string Name { get; set; }
        public string SpecKeyName { get; set; }
        public int SpecLevelRequirement { get; set; }
        public int Icon { get; set; }
        public int EnduranceCost { get; set; }
        public bool StealthRequirement { get; set; }
        public int OpeningRequirementType { get; set; }
        public int OpeningRequirementValue { get; set; }
        public int AttackResultRequirement { get; set; }
        public int WeaponTypeRequirement { get; set; }
        public double GrowthRate { get; set; }
        public int BonusToHit { get; set; }
        public int BonusToDefense { get; set; }
        public int TwoHandAnimation { get; set; }
        public bool RandomProc { get; set; }
        public int ArmorHitLocation { get; set; }
        public double GrowthOffset { get; set; }

        public virtual Specialization Specialization { get; set; }

        public virtual ICollection<StyleSpell> Spells { get; set; }
        
        public Style()
        {
            Spells = new HashSet<StyleSpell>();
        }
    }
}
