using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ServerProperty :DataObjectBase
    {
        public string Category { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public string Value { get; set; }
    }
}
