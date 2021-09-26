using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class PlayerInfo : DataObjectBase
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Guild { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public string Alive { get; set; }
        public string Realm { get; set; }
        public string Region { get; set; }
        public int Level { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
