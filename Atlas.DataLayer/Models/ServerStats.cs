using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ServerStats : DataObjectBase
    {
        public DateTime? StatDate { get; set; }
        public int Clients { get; set; }
        public double CPU { get; set; }
        public int Upload { get; set; }
        public int Download { get; set; }
        public long Memory { get; set; }
    }
}
