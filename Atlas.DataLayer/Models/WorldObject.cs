using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class WorldObject : DataObjectBase
    {
        public string ClassType { get; set; }
        public string TranslationID { get; set; }
        public string Name { get; set; }
        public string ExamineArticle { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Heading { get; set; }
        public int RegionID { get; set; }
        public int Model { get; set; }
        public int Emblem { get; set; }
        public int Realm { get; set; }
        public int RespawnInterval { get; set; }

        public virtual Region Region { get; set; }
    }
}
