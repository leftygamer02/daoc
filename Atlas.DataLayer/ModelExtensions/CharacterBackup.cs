using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class CharacterBackup
    {
        public CharacterBackup() : base()
        {

        }

        public CharacterBackup(Character source) : base()
        {
            if (source == null)
                return;

            this.OldCharacterId = source.Id;

            //TODO: Populate 'this' from source
        }
    }
}
