using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class CharacterBackup : Character
    {
        public int OldCharacterId { get; set; }
        public bool Rearranged { get; set; }
    }
}
