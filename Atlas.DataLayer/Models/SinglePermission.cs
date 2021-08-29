using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class SinglePermission : DataObjectBase
    {
        public int? CharacterID { get; set; }
        public int? AccountID { get; set; }
        public string Command { get; set; }

        public virtual Character Character { get; set; }
        public virtual Account Account { get; set; }
    }
}
