using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Salvage : DataObjectBase
    {
        public int ObjectType { get; set; }
        public int SalvageLevel { get; set; }
        public int ItemTemplateID { get; set; }
        public int Realm { get; set; }

        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
