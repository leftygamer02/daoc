using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class CharacterOneTimeDrop : DataObjectBase
    {
        public int CharacterID { get; set; }
        public int ItemTemplateID { get; set; }

        public virtual Character Character { get; set; }
        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
