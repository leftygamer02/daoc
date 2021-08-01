using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class PvpKillsLog : DataObjectBase
    {
        public DateTime? DateKilled { get; set; }
        public string KilledName { get; set; }
        public string KillerName { get; set; }
        public string KillerIP { get; set; }
        public string KilledIP { get; set; }
        public string KilledRealm { get; set; }
        public string KillerRealm { get; set; }
        public int RPReward { get; set; }
        public int SameIP { get; set; }
        public bool IsInstance { get; set; }
    }
}
