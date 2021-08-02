using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public partial class CharacterDataQuest
    {
		/// <summary>
		/// Create a new entry for this quest
		/// </summary>
		/// <param name="characterID"></param>
		/// <param name="dataQuestID"></param>
		public CharacterDataQuest(int characterID, int dataQuestID)
			: base()
		{
			this.CharacterID = characterID;
			this.Id = dataQuestID;
			this.Step = 1;
			this.Count = 0;
		}
	}
}
