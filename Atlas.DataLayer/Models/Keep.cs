using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class Keep : DataObjectBase
    {
        public string Name { get; set; }
        public int RegionID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Heading { get; set; }
        public int Realm { get; set; }
        public int Level { get; set; }
        public string ClaimedGuildName { get; set; }
        public int AlbionDifficultyLevel { get; set; }
        public int MidgardDifficultyLevel { get; set; }
        public int HiberniaDifficultyLevel { get; set; }
        public int OriginalRealm { get; set; }
        public int KeepType { get; set; }
        public int BaseLevel { get; set; }
        public eKeepSkinType SkinType { get; set; }
        public string CreateInfo { get; set; }
    }
}
