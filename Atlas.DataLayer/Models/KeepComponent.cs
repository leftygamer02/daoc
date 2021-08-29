using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class KeepComponent :DataObjectBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Heading { get; set; }
        public int Health { get; set; }
        public int Skin { get; set; }
        public int KeepID { get; set; }
        public string CreateInfo { get; set; }

        public virtual Keep Keep { get; set; }
    }
}
