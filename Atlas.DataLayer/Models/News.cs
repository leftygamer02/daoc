using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class News : DataObjectBase
    {
        public int Type { get; set; }
        public int Realm { get; set; }
        public string Text { get; set; }
    }
}
