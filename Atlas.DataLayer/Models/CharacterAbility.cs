using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterAbility : DataObjectBase
    {
        public int CharacterID { get; set; }
        public int AbilityID { get; set; }
        public bool IsRealmAbility { get; set; }
        public bool IsDisabled { get; set; }
        public int Level { get; set; }
        public long LastUsedTime { get; set; }
        public long ReuseTime { get; set; }

        public virtual Character Character { get; set; }
        public virtual Ability Ability { get; set; }
    }
}
