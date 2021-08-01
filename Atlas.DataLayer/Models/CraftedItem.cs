using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CraftedItem : DataObjectBase
    {
        public int ItemTemplateID { get; set; }
        public int CraftingLevel { get; set; }
        public int CraftingSkillType { get; set; }
        public bool MakeTemplated { get; set; }

        public virtual ItemTemplate ItemTemplate { get; set; }

        public virtual ICollection<CraftedItemComponent> Components { get; set; }

        public CraftedItem()
        {
            Components = new HashSet<CraftedItemComponent>();
        }
    }
}
