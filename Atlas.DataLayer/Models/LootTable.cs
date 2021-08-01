using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class LootTable : DataObjectBase
    {
        public string Name { get; set; }
        public int DropCount { get; set; }

        public virtual ICollection<LootTableItem> Items { get; set; }

        public LootTable()
        {
            Items = new HashSet<LootTableItem>();
        }
    }
}
