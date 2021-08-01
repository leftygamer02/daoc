using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CraftedItemComponent : DataObjectBase
    {
        public int CraftedItemID { get; set; }
        public int ItemTemplateID { get; set; }
        public int Count { get; set; }

        public virtual CraftedItem CraftedItem { get; set; }
        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
