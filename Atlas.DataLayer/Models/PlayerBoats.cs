using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class PlayerBoat : DataObjectBase
    {
        public int BoatOwnerID { get; set; }
        public string BoatName { get; set; }
        public int BoatModel { get; set; }
        public int BoatMaxSpeed { get; set; }

        public virtual Character BoatOwner { get; set; }
    }
}
