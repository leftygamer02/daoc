using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class ItemUnique
    {
        public ItemUnique() : base()
        {

        }

        public ItemUnique(ItemTemplate template) : base()
        {
            if (template == null)
                return;
            //TODO: Populate 'this' from template
        }
    }
}
