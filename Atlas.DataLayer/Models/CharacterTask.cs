using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterTask : DataObjectBase
    {
        public int CharacterID { get; set; }
        public DateTime? TimeOut { get; set; }
        public string TaskType { get; set; }
        public int TasksDone { get; set; }
        public string CustomPropertiesString { get; set; }

        public virtual Character Character { get; set; }
    }
}
