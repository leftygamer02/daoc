using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Ability : DataObjectBase
    {
        public string KeyName { get; set; }
        public string Name { get; set; }
        public int InternalID { get; set; }
        public string Description { get; set; }
        public int IconID { get; set; }
        public string Implementation { get; set; }

    }
}
