using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class DbIndoorItem : DataObjectBase
    {
        public int HouseID { get; set; }
        public int HouseNumber { get; set; }
        public int Model { get; set; }
        public int Position { get; set; }
        public int Placemode { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int BaseItemID { get; set; }
        public int Color { get; set; }
        public int Emblem { get; set; }
        public int Rotation { get; set; }
        public int Size { get; set; }

        public virtual DbHouse House { get; set; }
        public virtual ItemTemplate BaseItem { get; set; }
    }
}
