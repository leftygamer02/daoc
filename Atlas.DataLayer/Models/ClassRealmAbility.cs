using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ClassRealmAbility :DataObjectBase
    {
        public int ClassID { get; set; }
        public int AbilityID { get; set; }

        public virtual Ability Ability { get; set; }
    }
}
