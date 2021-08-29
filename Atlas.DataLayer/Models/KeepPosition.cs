using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class KeepPosition : DataObjectBase
    {
        public int ComponentSkin { get; set; }
        public int ComponentRotation { get; set; }
        public string TemplateID { get; set; }
        public int Height { get; set; }
        public int XOff { get; set; }
        public int YOff { get; set; }
        public int ZOff { get; set; }
        public int HOff { get; set; }
        public string ClassType { get; set; }
        public int TemplateType { get; set; }
        public int KeepType { get; set; }
    }
}
