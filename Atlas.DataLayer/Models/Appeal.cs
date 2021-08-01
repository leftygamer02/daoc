using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Appeal : DataObjectBase
    {
        public int AccountID { get; set; }
        public int CharacterID { get; set; }
        public string Name { get; set; }
        public int Severity { get; set; }
        public string Status { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Text { get; set; }

        public virtual Account Account { get; set; }
    }
}
