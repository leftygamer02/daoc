using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class SpellLine : DataObjectBase
    {
        public int? SpecializationID { get; set; }
        public string KeyName { get; set; }
        public string Name { get; set; }
        public string SpecKeyName { get; set; }
        public bool IsBaseLine { get; set; }
        public string PackageID { get; set; }
        public int ClassIDHint { get; set; }

        public virtual Specialization Specialization { get; set; }

        public virtual ICollection<SpellLineSpell> SpellLineSpells { get; set; }

        public SpellLine()
        {
            SpellLineSpells = new HashSet<SpellLineSpell>();
        }
    }
}
