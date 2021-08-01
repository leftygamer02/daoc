using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class GuildAlliance : DataObjectBase
    {
        public string AllianceName { get; set; }
        public string Motd { get; set; }
        public int LeaderGuildID { get; set; }
        
        public virtual ICollection<Guild> Guilds { get; set; }

        public GuildAlliance()
        {
            Guilds = new HashSet<Guild>();
        }
    }
}
