using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ArtifactBonus :DataObjectBase
    {
        public int ArtifactID { get; set; }
        public int BonusID { get; set; }
        public int Level { get; set; }

        public virtual Artifact Artifact { get; set; }
    }
}
