using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class GuildRank : DataObjectBase
    {
        public int GuildID { get; set; }
        public string Title { get; set; }
        public int RankLevel { get; set; }
        public bool Alli { get; set; }
        public bool Emblem { get; set; }
        public bool Buff { get; set; }
        public bool GcHear { get; set; }
        public bool GcSpeak { get; set; }
        public bool OcHear { get; set; }
        public bool OcSpeak { get; set; }
        public bool AcHear { get; set; }
        public bool AcSpeak { get; set; }
        public bool Invite { get; set; }
        public bool Promote { get; set; }
        public bool Remove { get; set; }
        public bool View { get; set; }
        public bool Claim { get; set; }
        public bool Upgrade { get; set; }
        public bool Release { get; set; }
        public bool Dues { get; set; }
        public bool Withdraw { get; set; }

        public virtual Guild Guild { get; set; }
    }
}
