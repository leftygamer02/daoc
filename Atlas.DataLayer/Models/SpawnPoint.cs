using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class SpawnPoint : DataObjectBase
    {
        public int RegionID { get; set; }
        public int SpawnGroupID { get; set; }
        public int PathID { get; set; }
        public int? OwnerID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Heading { get; set; }
        public int AggroLevel { get; set; }
        public int AggroRange { get; set; }
        public int MaxDistance { get; set; }
        public int RoamingRange { get; set; }
        public int RespawnInterval { get; set; }
        public bool IsCloakHoodUp { get; set; }

        public virtual Region Region { get; set; }
        public virtual SpawnGroup SpawnGroup { get; set; }
        public virtual Path Path { get; set; }
        public virtual Character Character { get; set; }
    }
}
