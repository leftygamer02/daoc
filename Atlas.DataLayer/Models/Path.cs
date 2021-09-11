using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Path : DataObjectBase
    {
        public string PathName { get; set; }
        public int PathType { get; set; }
        public int? RegionID { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<PathPoints> PathPoints { get; set; }
        public virtual ICollection<SpawnPoint> SpawnPoints { get; set; }
        public Path()
        {
            PathPoints = new HashSet<PathPoints>();
            SpawnPoints = new HashSet<SpawnPoint>();
        }
    }
}
