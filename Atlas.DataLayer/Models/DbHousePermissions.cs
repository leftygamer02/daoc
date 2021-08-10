using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class DbHousePermissions : DataObjectBase
    {
        public int HouseId { get; set; }
        public int PermissionLevel { get; set; }
        public int HouseNumber { get; set; }
        public bool CanEnterHouse { get; set; }
        public int Vault1 { get; set; }
        public int Vault2 { get; set; }
        public int Vault3 { get; set; }
        public int Vault4 { get; set; }
        public bool CanChangeExternalAppearance { get; set; }
        public int ChangeInterior { get; set; }
        public int ChangeGarden { get; set; }
        public bool CanBanish { get; set; }
        public bool CanUseMerchants { get; set; }
        public bool CanUseTools { get; set; }
        public bool CanBindInHouse { get; set; }
        public int ConsignmentMerchant { get; set; }
        public bool CanPayRent { get; set; }

        public virtual DbHouse House { get; set; }
    }
}
