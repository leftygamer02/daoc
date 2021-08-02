using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class SpellLineSpell : DataObjectBase
    {
        public int SpellLineID { get; set; }
        public int SpellID { get; set; }
        public int Level { get; set; }

        public virtual SpellLine SpellLine { get; set; }
        public virtual Spell Spell { get; set; }
    }
}
