using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Battleground : DataObjectBase
    {
        public int RegionID { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int MaxRealmLevel { get; set; }
    }
}
