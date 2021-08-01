using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class LootTableItem : DataObjectBase
    {
        public int LootTableID { get; set; }
        public int ItemTemplateID { get; set; }
        public int Chance { get; set; }
        public int Count { get; set; }

        public virtual LootTable LootTable { get; set; }
        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
