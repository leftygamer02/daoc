using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class MerchantItem : DataObjectBase
    {
        public string ItemListName { get; set; }
        public int ItemTemplateID { get; set; }
        public int PageNumber { get; set; }
        public int SlotPosition { get; set; }

        public virtual ItemTemplate ItemTemplate { get; set; }

    }
}
