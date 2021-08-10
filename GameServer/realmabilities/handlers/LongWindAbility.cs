using System;
using Atlas.DataLayer.Models;
using DOL.GS.PacketHandler;
using DOL.GS.Effects;
using DOL.GS.SkillHandler;

namespace DOL.GS.RealmAbilities
{
	/// <summary>
	/// Long Wind
	/// </summary>
	public class LongWindAbility : RAPropertyEnhancer
	{
		public LongWindAbility(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level, eProperty.Undefined) { }

        protected override string ValueUnit { get { return "%"; } }

		public override int GetAmountForLevel(int level)
		{
            return level;
            switch (level)
            {
                case 1: return 20;
                case 2: return 40;
                case 3: return 60;
                case 4: return 80;
                case 5: return 100;
                default: return 0;
            }
        }
	}
}