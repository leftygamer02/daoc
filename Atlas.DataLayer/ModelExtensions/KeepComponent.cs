using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class KeepComponent
    {
        public KeepComponent() 
            : base()
        {
            Skin = 0;
            X = 0;
            Y = 0;
            Heading = 0;
            Health = 0;
            KeepID = 0;            
            CreateInfo = string.Empty;
        }

        public KeepComponent(int componentID, int componentSkinID, int componentX, int componentY, int componentHead, int componentHeight, int componentHealth, int keepid, string createInfo) 
            : this()
        {
            Skin = componentSkinID;
            X = componentX;
            Y = componentY;
            Heading = componentHead;
            Health = componentHealth;
            KeepID = keepid;
            Id = componentID;
            CreateInfo = createInfo;
        }
    }
}
