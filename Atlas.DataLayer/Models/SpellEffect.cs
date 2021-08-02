using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class SpellEffect : DataObjectBase
    {
        public int SpellID { get; set; }
        public int EffectType { get; set; }
        public int Value { get; set; }
        public int Duration { get; set; }

        public virtual Spell Spell { get; set; }
    }
}
