using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class AuditEntry :DataObjectBase
    {
        public DateTime? AuditTime { get; set; }
        public int AccountID { get; set; }
        public string RemoteHost { get; set; }
        public int AuditType { get; set; }
        public int AuditSubtype { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public virtual Account Account { get; set; }
    }
}
