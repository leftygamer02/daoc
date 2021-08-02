using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public abstract class DataObjectBase : IDataObject, ICloneable
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public object Clone()
		{
			var obj = (DataObjectBase) MemberwiseClone();
			obj.Id = 0;
			return obj;
		}
    }
}
