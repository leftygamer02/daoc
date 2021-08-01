using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class DbHouse : DataObjectBase
    {
        public int HouseNumber { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int RegionID { get; set; }
        public int Heading { get; set; }
        public string Name { get; set; }
        public int Model { get; set; }
        public int Emblem { get; set; }
        public int PorchRoofColor { get; set; }
        public int PorchMaterial { get; set; }
        public int RoofMaterial { get; set; }
        public int DoorMaterial { get; set; }
        public int WallMaterial { get; set; }
        public int TrussMaterial { get; set; }
        public int WindowMaterial { get; set; }
        public int Rug1Color { get; set; }
        public int Rug2Color { get; set; }
        public int Rug3Color { get; set; }
        public int Rug4Color { get; set; }
        public bool IndoorGuildBanner { get; set; }
        public bool IndoorGuildShield { get; set; }
        public bool OutdoorGuildBanner { get; set; }
        public bool OutdoorGuildShield { get; set; }
        public bool Porch { get; set; }
        public int OwnerID { get; set; }
        public DateTime? LastPaid { get; set; }
        public long KeptMoney { get; set; }
        public bool NoPurge { get; set; }
        public bool GuildHouse { get; set; }
        public string GuildName { get; set; }
        public bool HasConsignment { get; set; }

        public virtual Character Owner { get; set; }
        
    }
}
