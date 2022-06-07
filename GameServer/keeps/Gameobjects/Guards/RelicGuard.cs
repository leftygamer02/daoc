using DOL.GS.PlayerClass;
using DOL.GS.ServerProperties;
using DOL.Language;

namespace DOL.GS.Keeps
{
	public class RelicGuard : GuardFighter
	{
		protected override ICharacterClass GetClass()
		{
			return ModelRealm switch
			{
				eRealm.Albion => new ClassArmsman(),
				eRealm.Midgard => new ClassWarrior(),
				eRealm.Hibernia => new ClassHero(),
				_ => new DefaultCharacterClass()
			};
		}

		protected override void SetBlockEvadeParryChance()
		{
			base.SetBlockEvadeParryChance();
			BlockChance = 10;
			ParryChance = 10;
		}

		protected override void SetName()
		{
			Name = "Relic Guard";
		}
	}
}
