using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Faction : DataObjectBase
    {
        public string Name { get; set; }
        public int BaseAggroLevel { get; set; }
    }
}
