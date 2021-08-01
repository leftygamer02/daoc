using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Door :DataObjectBase
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int Z { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public int Heading { get; set; }
        public int InternalID { get; set; }
        public string Guild { get; set; }
        public int Level { get; set; }
        public int Realm { get; set; }
        public int Flags { get; set; }
        public int Locked { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        

    }
}
