using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterIgnore : DataObjectBase
    {
        public int CharacterID { get; set; }
        public int IgnoreID { get; set; }

        public virtual Character Character { get; set; }
        public virtual Character Ignore { get; set; }
    }
}
