using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class DbHouseCharPerms : DataObjectBase
    {
        public int HouseId { get; set; }
        public int HouseNumber { get; set; }
        public int PermissionType { get; set; }
        public string TargetName { get; set; }
        public string DisplayName { get; set; }
        public int PermissionLevel { get; set; }

        public virtual DbHouse House { get; set; }
    }
}
