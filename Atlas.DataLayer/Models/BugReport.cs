using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class BugReport :DataObjectBase
    {
        public string Message { get; set; }
        public string Submitter { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string ClosedBy { get; set; }
        public DateTime? DateClosed { get; set; }
        public string Category { get; set; }
    }
}
