using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class Voting : DataObjectBase
    {
        public string OptionStr { get; set; }
        public string Description { get; set; }
    }
}
