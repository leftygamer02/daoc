using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Artifact : DataObjectBase
    {
        public string EncounterID { get; set; }
        public string QuestID { get; set; }
        public string Name { get; set; }
        public int Zone { get; set; }
        public int ScholarID { get; set; }
        public int ReuseTimer { get; set; }
        public int XPRate { get; set; }
        public string BookID { get; set; }
        public int BookModel { get; set; }
        public string Scroll1 { get; set; }
        public string Scroll2 { get; set; }
        public string Scroll3 { get; set; }
        public string Scroll12 { get; set; }
        public string Scroll13 { get; set; }
        public string Scroll23 { get; set; }
        public int ScrollModel1 { get; set; }
        public int ScrollModel2 { get; set; }
        public int ScrollLevel { get; set; }
        public string MessageUse { get; set; }
        public string MessageCombineScrolls { get; set; }
        public string MessageCombineBook { get; set; }
        public string MessageReceiveScrolls { get; set; }
        public string MessageReceiveBook { get; set; }
        public string Credit { get; set; }

        public virtual ICollection<ArtifactBonus> ArtifactBonuses { get; set; }

        public Artifact()
        {
            ArtifactBonuses = new HashSet<ArtifactBonus>();
        }
    }
}
