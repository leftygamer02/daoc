using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class HouseConsignmentMerchant : DataObjectBase
    {
        public int OwnerID { get; set; }
        public int HouseID { get; set; }
        public int HouseNumber { get; set; }
        public long Money { get; set; }

        public virtual Character Owner { get; set; }
        public virtual DbHouse House { get; set; }
    }
}
