using System;
using DOL.GS.PropertyCalc;

namespace DOL.GS.RealmAbilities
{
	/// <summary>
	/// Avoidance of Magic RA, reduces magical damage
	/// </summary>
	public class AtlasOF_AvoidanceOfMagicAbility : AvoidanceOfMagicAbility
	{
		public AtlasOF_AvoidanceOfMagicAbility(Atlas.DataLayer.Models.Ability dba, int level) : base(dba, level) { }
		public override int GetAmountForLevel(int level) { return AtlasRAHelpers.GetPropertyEnhancer3AmountForLevel(level); }
		public override int CostForUpgrade(int level) { return AtlasRAHelpers.GetCommonPassivesCostForUpgrade(level); }
	}
}