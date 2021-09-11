using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterSpec : DataObjectBase
    {
        public int CharacterID { get; set; }
        public string SpecLine { get; set; }
        public int SpecLevel { get; set; }
        public int SpecializationID { get; set; }

        public virtual Character Character { get; set; }
        public virtual Specialization Specialization { get; set; }
    }
}
