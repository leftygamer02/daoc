using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class LootOtd : DataObjectBase
    {
        public string MobName { get; set; }
        public int ItemTemplateID { get; set; }
        public int MinLevel { get; set; }

        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
