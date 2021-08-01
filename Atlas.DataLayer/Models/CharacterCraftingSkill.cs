using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterCraftingSkill : DataObjectBase
    {
        public int CharacterID { get; set; }
        public int CraftingSkill { get; set; }
        public int SkillLevel { get; set; }

        public virtual Character Character { get; set; }
    }
}
