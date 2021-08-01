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
        public int HouseNumber { get; set; }
        public bool CanEnterHouse { get; set; }
        public bool Vault1 { get; set; }
        public bool Vault2 { get; set; }
        public bool Vault3 { get; set; }
        public bool Vault4 { get; set; }
        public bool CanChangeExternalAppearance { get; set; }
        public bool ChangeInterior { get; set; }
        public bool ChangeGarden { get; set; }
        public bool CanBanish { get; set; }
        public bool CanUseMerchants { get; set; }
        public bool CanUseTools { get; set; }
        public bool CanBindInHouse { get; set; }
        public bool ConsignmentMerchant { get; set; }
        public bool CanPayRent { get; set; }

        public virtual DbHouse House { get; set; }
    }
}
