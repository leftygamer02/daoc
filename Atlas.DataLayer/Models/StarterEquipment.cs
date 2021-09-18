using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.DataLayer.Models
{
    [Table("StarterEquipment")]
    public class StarterEquipment : DataObjectBase
    {
        public string ClassIDs { get; set; }
        public int ItemTemplateID { get; set; }

        public virtual ItemTemplate ItemTemplate { get; set; }
    }
}
