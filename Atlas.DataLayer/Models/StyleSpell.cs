using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class StyleSpell : DataObjectBase
    {
        public int SpellID { get; set; }
        public int ClassID { get; set; }
        public int StyleID { get; set; }
        public int Chance { get; set; }

        public virtual Spell Spell { get; set; }
        public virtual Style Style { get; set; }
    }
}
