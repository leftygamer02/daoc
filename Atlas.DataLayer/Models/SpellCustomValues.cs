using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class SpellCustomValues : DataObjectBase
    {
        public int SpellID { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }
        
        public virtual Spell Spell { get; set; }
    }
}
