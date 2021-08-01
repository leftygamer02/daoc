using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class KeepCaptureLog : DataObjectBase
    {
        public DateTime? DateTaken { get; set; }
        public string KeepName { get; set; }
        public string KeepType { get; set; }
        public int NumEnemies { get; set; }
        public int CombatTime { get; set; }
        public int RPReward { get; set; }
        public int BPReward { get; set; }
        public long XPReward { get; set; }
        public long MoneyReward { get; set; }
        public string CapturedBy { get; set; }
        public string RPGainerList { get; set; }
    }
}
