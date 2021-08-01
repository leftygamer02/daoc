using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Guild : DataObjectBase
    {
        public int GuildAllianceID { get; set; }
        public string GuildName { get; set; }
        public int Realm { get; set; }
        public bool GuildBanner { get; set; }
        public DateTime? GuildBannerLostTime { get; set; }
        public string Motd { get; set; }
        public string oMotd { get; set; }
        public int Emblem { get; set; }
        public long RealmPoints { get; set; }
        public long BountyPoints { get; set; }
        public string Webpage { get; set; }
        public string Email { get; set; }
        public bool Dues { get; set; }
        public double Bank { get; set; }
        public long DuesPercent { get; set; }
        public bool HaveGuildHouse { get; set; }
        public int GuildHouseNumber { get; set; }
        public long GuildLevel { get; set; }
        public int BonusType { get; set; }
        public DateTime? BonusStartTime { get; set; }
        public long MeritPoints { get; set; }

        public virtual GuildAlliance GuildAlliance { get; set; }

        public virtual ICollection<Character> Characters { get; set; }

        public Guild()
        {
            Characters = new HashSet<Character>();
        }
        

    }
}
