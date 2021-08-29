using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class Keep
    {
        public Keep() : base()
        {
            Name = string.Empty;
            AlbionDifficultyLevel = 1;
            MidgardDifficultyLevel = 1;
            HiberniaDifficultyLevel = 1;
            KeepType = 0; // Default = Any
            CreateInfo = string.Empty;
        }

        public Keep(string createInfo) : this()
        {
            CreateInfo = createInfo;
        }
    }
}
