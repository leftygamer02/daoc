using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ServerPropertyCategory : DataObjectBase
    {
        public string BaseCategory { get; set; }
        public string ParentCategory { get; set; }
        public string DisplayName { get; set; }
    }
}
