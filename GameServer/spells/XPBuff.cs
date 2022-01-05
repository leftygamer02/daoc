/*
 * ATLAS
 * XP Buff potion Spell Handler
 *
 */
using System;
using DOL.GS.Effects;
using DOL.GS.PacketHandler;

/*
 * /cast spell 31100
 * INSERT INTO `spell` (`Spell_ID`, `SpellID`, `ClientEffect`, `Icon`, `Name`, `Description`, `Target`, `Range`, `Power`, `CastTime`, `Damage`, `DamageType`, `Type`, `Duration`, `Frequency`, `Pulse`, `PulsePower`, `Radius`, `RecastDelay`, `ResurrectHealth`, `ResurrectMana`, `Value`, `Concentration`, `LifeDrainReturn`, `AmnesiaChance`, `Message1`, `Message2`, `Message3`, `Message4`, `InstrumentRequirement`, `SpellGroup`, `EffectGroup`, `SubSpellID`, `MoveCast`, `Uninterruptible`, `IsPrimary`, `IsSecondary`, `AllowBolt`, `SharedTimerGroup`, `PackageID`, `IsFocus`, `TooltipId`, `LastTimeRowUpdated`) VALUES ('31100', 31100, 0, 0, 'Atlas XP', 'Target gains 10% additional XP for the effect duration.', 'Self', 0, 0, 0, 10, 0, 'XPBuff', 600, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 'You feel a calmness come over you.', '{0} looks calmer.', 'Your inner peace leaves you.', '{0} calmness leaves.', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 'XPBuff', 0, 31100, '2015-01-01 00:00:00');
 * 
 */

namespace DOL.GS.Spells
{
	/// <summary>
	/// Base class for all resist buffs, needed to set effectiveness
	/// </summary>
	///
	[SpellHandler("XPBuff")]
	public class XPBuff : PropertyChangingSpell
	{
		
        public override void CreateECSEffect(ECSGameEffectInitParams initParams)
        {
			new StatBuffECSEffect(initParams);
        }

        public override void ApplyEffectOnTarget(GameLiving target, double effectiveness)
		{
			if (target is GamePlayer && (((GamePlayer)target).CharacterClass.ID == (int)eCharacterClass.Vampiir
			                             || ((GamePlayer)target).CharacterClass.ID == (int)eCharacterClass.MaulerAlb
			                             || ((GamePlayer)target).CharacterClass.ID == (int)eCharacterClass.MaulerMid
			                             || ((GamePlayer)target).CharacterClass.ID == (int)eCharacterClass.MaulerHib)) { MessageToCaster("This spell has no effect on this class!", eChatType.CT_Spell); return; }
			base.ApplyEffectOnTarget(target, effectiveness);
		}
		public override eBuffBonusCategory BonusCategory1 { get { return eBuffBonusCategory.BaseBuff; } }
		public override eProperty Property1 { get { return eProperty.XpPoints; } }
		
		public XPBuff(GameLiving caster, Spell spell, SpellLine spellLine) : base(caster, spell, spellLine) { }
	}



}
