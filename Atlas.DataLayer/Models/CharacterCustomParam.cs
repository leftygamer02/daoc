using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterCustomParam : DataObjectBase
    {
        public int CharacterID { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }

        public virtual Character Character { get; set; }
    }
}
