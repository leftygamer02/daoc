using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class Race : DataObjectBase
    {
        public string Name { get; set; }
        public int ResistBody { get; set; }
        public int ResistCold { get; set; }
        public int ResistCrush { get; set; }
        public int ResistEnergy { get; set; }
        public int ResistHeat { get; set; }
        public int ResistMatter { get; set; }
        public int ResistSlash { get; set; }
        public int ResistSpirit { get; set; }
        public int ResistThrust { get; set; }
    }
}
