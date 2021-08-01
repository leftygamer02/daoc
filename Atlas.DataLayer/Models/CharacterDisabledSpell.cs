using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterDisabledSpell : DataObjectBase
    {
        public int CharacterID { get; set; }
        public int SpellID { get; set; }
        public long CastTime { get; set; }
        public long ReuseTime { get; set; }

        public virtual Character Character { get; set; }
    }
}
