using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class SalvageYield : DataObjectBase
    {
        public int ObjectType { get; set; }
        public int SalvageLevel { get; set; }
        public int ItemTemplateID { get; set; }
        public int Count { get; set; }
        public int Realm { get; set; }
        public string PackageID { get; set; }

        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
