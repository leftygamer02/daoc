using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ArtifactItem : DataObjectBase
    {
        public int ArtifactID { get; set; }
        public int ItemTemplateID { get; set; }
        public int Version { get; set; }
        public int Realm { get; set; }

        public virtual Artifact Artifact { get; set; }
        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
