using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Quest : DataObjectBase
    {
        public string Name { get; set; }
        public int Step { get; set; }
        public int CharacterID { get; set; }
        public string CustomPropertiesString { get; set; }

        public virtual Character Character { get; set; }
    }
}
