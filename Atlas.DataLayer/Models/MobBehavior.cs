using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class MobBehavior: DataObjectBase
    {
        public string Source { get; set; }
        public string Trigger { get; set; }
        public int Emote { get; set; }
        public string Text { get; set; }
        public int Chance { get; set; }
        public string Voice { get; set; }
    }
}
