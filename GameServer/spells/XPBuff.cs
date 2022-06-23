using DOL.GS.Effects;

namespace DOL.GS.Spells
{
	/// <summary>
	/// Base class for all resist buffs, needed to set effectiveness
	/// </summary>
	///
	[SpellHandler("XPBuff")]
	public class XPBuff : PropertyChangingSpell
	{
		public XPBuff(GameLiving caster, Spell spell, SpellLine spellLine) : base(caster, spell, spellLine) { }


		public override eBuffBonusCategory BonusCategory1 { get { return eBuffBonusCategory.BaseBuff; } }
		public override eProperty Property1 { get { return eProperty.XpPoints; } }
	}
}
