using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterMasterLevel : DataObjectBase
    {
        public int CharacterID { get; set; }
        public int MLLevel { get; set; }
        public int MLStep { get; set; }
        public bool StepCompleted { get; set; }
        public DateTime? ValidationDate { get; set; }

        public virtual Character Character { get; set; }
    }
}
