using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public interface IDataObject
    {
        int Id { get; set; }
        DateTime? CreateDate { get; set; }
        DateTime? ModifyDate { get; set; }
    }
}
