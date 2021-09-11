using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Specialization : DataObjectBase
    {
        public string KeyName { get; set; }
        public string Name { get; set; }
        public int Icon { get; set; }
        public string Description { get; set; }
        public string Implementation { get; set; }

        public ICollection<SpellLine> SpellLines { get; set; }
        public ICollection<SpecializationAbility> Abilities { get; set; }
        public ICollection<Style> Styles { get; set; }
        public ICollection<Character> Characters { get; set; }

        public Specialization()
        {
            SpellLines = new HashSet<SpellLine>();
            Abilities = new HashSet<SpecializationAbility>();
            Styles = new HashSet<Style>();
            Characters = new HashSet<Character>();
        }
    }
}
