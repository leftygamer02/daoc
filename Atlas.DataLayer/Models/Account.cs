using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Account : DataObjectBase
    {
        public string Name { get; set; }
        public DateTime? LastLogin { get; set; }
        public int Realm { get; set; }
        public int PrivLevel { get; set; }
        public int Status { get; set; }
        public string Mail { get; set; }
        public string LastLoginIP { get; set; }
        public string LastClientVersion { get; set; }
        public string Language { get; set; }
        public bool IsMuted { get; set; }

        public virtual ICollection<AccountCustomParam> AccountCustomParams { get; set; }
        public virtual ICollection<Appeal> Appeals { get; set; }
        public virtual ICollection<Ban> Bans { get; set; }
        public virtual ICollection<SinglePermission> SinglePermissions { get; set; }
        public virtual ICollection<Character> Characters { get; set; }

        public Account()
        {
            AccountCustomParams = new HashSet<AccountCustomParam>();
            Appeals = new HashSet<Appeal>();
            Bans = new HashSet<Ban>();
            SinglePermissions = new HashSet<SinglePermission>();
            Characters = new HashSet<Character>();
        }
    }
}
