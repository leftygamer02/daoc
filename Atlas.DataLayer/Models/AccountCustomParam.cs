using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class AccountCustomParam : DataObjectBase
    {
        public int AccountID { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }

        public virtual Account Account { get; set; }
    }
}
