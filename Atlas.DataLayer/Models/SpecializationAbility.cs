using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class SpecializationAbility : DataObjectBase
    {
        public int SpecializationID { get; set; }
        public int AbilityID { get; set; }
        public int SpecLevel { get; set; }
        public int AbilityLevel { get; set; }
        public int ClassID { get; set; }

        public virtual Specialization Specialization { get; set; }
        public virtual Ability Ability { get; set; }
    }
}
