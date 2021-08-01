using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ItemBonus : DataObjectBase
    {
        public int ItemTemplateID { get; set; }
        public int BonusType { get; set; }
        public int BonusValue { get; set; }
        public int BonusOrder { get; set; }

        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
