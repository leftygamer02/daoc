using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class CharacterDataQuest : DataObjectBase
    {
        public int CharacterID { get; set; }
        public int DataQuestID { get; set; }
        public int Step { get; set; }
        public int Count { get; set; }

        public virtual Character Character { get; set; }
    }
}
