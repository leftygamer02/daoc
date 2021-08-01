using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ClassSpecialization : DataObjectBase
    {
        public int ClassID { get; set; }
        public int SpecializationID { get; set; }
        public int LevelAcquired { get; set; }

        public virtual Specialization Specialization { get; set; }
    }
}
