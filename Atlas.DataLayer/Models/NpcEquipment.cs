using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class NpcEquipment : DataObjectBase
    {
        public int Slot { get; set; }
        public int Model { get; set; }
        public int Color { get; set; }
        public int Effect { get; set; }
        public int Extension { get; set; }
        public int Emblem { get; set; }
        public string EquipmentTemplateName { get; set; }
    }
}
