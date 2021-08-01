using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class PathPoints : DataObjectBase
    {
        public int PathID { get; set; }
        public int Step { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int MaxSpeed { get; set; }
        public int WaitTime { get; set; }

        public virtual Path Path { get; set; }
    }
}
