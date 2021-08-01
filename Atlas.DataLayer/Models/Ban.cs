using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Ban : DataObjectBase
    {
        public string Author { get; set; }
        public string Type { get; set; }
        public string Ip { get; set; }
        public int AccountID { get; set; }
        public DateTime? DateBan { get; set; }
        public string Reason { get; set; }

        public virtual Account Account { get; set; }
    }
}
