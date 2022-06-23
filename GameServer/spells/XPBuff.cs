using DOL.GS.Effects;

namespace DOL.GS.Spells
{
	/// <summary>
	/// Base class for all resist buffs, needed to set effectiveness
	/// </summary>
	///
	[SpellHandler("XPBuff")]
	public class XPBuff : SpellHandler
	{
		public XPBuff(GameLiving caster, Spell spell, SpellLine spellLine) : base(caster, spell, spellLine) { }


		public override void OnEffectStart(GameSpellEffect effect)
		{
			if (effect.Owner is GamePlayer player)
			{
				player.BaseBuffBonusCategory[(int)eProperty.XpPoints] += (int)Spell.Value;
			}

			base.OnEffectStart(effect);
		}

		public override int OnEffectExpires(GameSpellEffect effect, bool noMessages)
		{
			if (effect.Owner is GamePlayer player)
			{
				player.BaseBuffBonusCategory[(int)eProperty.XpPoints] -= (int)Spell.Value;
			}
			
			return base.OnEffectExpires(effect, noMessages);
		}
	}
}
