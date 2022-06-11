using System;
using DOL.AI.Brain;
using DOL.Database;
using DOL.GS.Effects;
using DOL.GS.Spells;
using DOL.GS.PacketHandler;

namespace DOL.GS
{
    
    /// <summary>
    /// Spell-Based Effect
    /// </summary>
    public class ECSGameSpellEffect : ECSGameEffect, IConcentrationEffect
    {
        public ISpellHandler SpellHandler;
        string IConcentrationEffect.Name => Name;
        ushort IConcentrationEffect.Icon => Icon;
        byte IConcentrationEffect.Concentration => SpellHandler.Spell.Concentration;

        public override ushort Icon { get { return SpellHandler.Spell.Icon; } }
        public override string Name { get { return SpellHandler.Spell.Name; } }
        public override bool HasPositiveEffect { get { return SpellHandler == null ? false : SpellHandler.HasPositiveEffect; } }

        public ECSGameSpellEffect(ECSGameEffectInitParams initParams) : base(initParams)
        {
            SpellHandler = initParams.SpellHandler;
            //SpellHandler = SpellHandler; // this is the base ECSGameEffect handler , temp during conversion into different classes
            EffectType = MapSpellEffect();
            PulseFreq = SpellHandler.Spell != null ? SpellHandler.Spell.Frequency : 0;

            if (SpellHandler.Spell.SpellType == (byte)eSpellType.SpeedDecrease || SpellHandler.Spell.SpellType == (byte)eSpellType.UnbreakableSpeedDecrease)
            {
                TickInterval = 650;
                NextTick = 1 + (Duration >> 1) + (int)StartTick;
                if(!SpellHandler.Spell.Name.Equals("Prevent Flight"))
					TriggersImmunity = true;
            }
            else if (SpellHandler.Spell.IsConcentration)
            {
                NextTick = StartTick;
                // 60 seconds taken from PropertyChangingSpell
                // Not sure if this is correct
                PulseFreq = 650;
            }

            if (this is not ECSImmunityEffect && this is not ECSPulseEffect)
                EffectService.RequestStartEffect(this);
        }

        private eEffect MapSpellEffect()
        {
            if (SpellHandler.SpellLine.IsBaseLine)
            {
                SpellHandler.Spell.IsSpec = false;
            }
            else
            {
                SpellHandler.Spell.IsSpec = true;
            }

            return EffectService.GetEffectFromSpell(SpellHandler.Spell);
        }

        public override bool IsConcentrationEffect()
        {
            return SpellHandler.Spell.IsConcentration;
        }

        public override bool ShouldBeAddedToConcentrationList()
        {
            return SpellHandler.Spell.IsConcentration || EffectType == eEffect.Pulse;
        }

        public override bool ShouldBeRemovedFromConcentrationList()
        {
            return SpellHandler.Spell.IsConcentration || EffectType == eEffect.Pulse;
        }

        public override void TryApplyImmunity()
        {
            if (TriggersImmunity)
            {
                if (OwnerPlayer != null)
                {
                    if ((EffectType == eEffect.Stun && SpellHandler.Caster is GamePet) || SpellHandler is UnresistableStunSpellHandler)
                        return;

                    new ECSImmunityEffect(Owner, SpellHandler, ImmunityDuration, (int)PulseFreq, Effectiveness, Icon);
                }
                else if (Owner is GameNPC)
                {
                    if (EffectType == eEffect.Stun)
                    {
                        NPCECSStunImmunityEffect npcImmune = (NPCECSStunImmunityEffect)EffectListService.GetEffectOnTarget(Owner, eEffect.NPCStunImmunity);
                        if (npcImmune is null)
                            new NPCECSStunImmunityEffect(new ECSGameEffectInitParams(Owner, ImmunityDuration, Effectiveness));
                    }
                    else if (EffectType == eEffect.Mez)
                    {
                        NPCECSMezImmunityEffect npcImmune = (NPCECSMezImmunityEffect)EffectListService.GetEffectOnTarget(Owner, eEffect.NPCMezImmunity);
                        if (npcImmune is null)
                            new NPCECSMezImmunityEffect(new ECSGameEffectInitParams(Owner, ImmunityDuration, Effectiveness));
                    }
                }
            }
        }

        private GameLiving m_caster;
        /// <summary>
        /// Used for 'OnEffectStartMsg' and 'OnEffectExpiresMsg'. Identifies the entity triggering the effect.
        /// </summary>
        public GameLiving Caster => m_caster;

        #region Effect Messages

        #region Effect Start Messages
        /// <summary>
		/// Sends effect messages to all nearby/associated players when an ability/spell/style effect becomes active on a target
		/// </summary>
		/// <param name="target">The owner of the effect.</param>
        /// <param name="msgTarget">If 'true', the system sends a first-person spell message to the target/owner of the effect.</param>
		/// <param name="msgCaster">If 'true', the system sends a third-person spell message to the caster triggering the effect, regardless of their proximity to the target.</param>
		/// <param name="msgArea">If 'true', the system sends a third-person message to all players within range of the target.</param>
		/// <returns>'Message1' and 'Message2' values from the 'spell' table.</returns>
		public void OnEffectStartsMsg(GameLiving target, bool msgTarget, bool msgCaster, bool msgArea)
		{
			// If the target variable is at the start of the string, capitalize their name or article
			var upperCase = SpellHandler.Spell.Message2.StartsWith("{0}");

	        if (target == null)
		        return;

	        Owner = target;
	        m_caster = SpellHandler.Caster;

	        // Sends a first-person message directly to the caster's target, if they are a player
	        // "You feel more dexterous!"
	        if (msgTarget)
	        {
		        if (Owner is GamePlayer targetOwner)
			        ((SpellHandler) SpellHandler).MessageToLiving(targetOwner, Util.MakeSentence(SpellHandler.Spell.Message1), eChatType.CT_Spell);
	        }

	        // Sends a third-person message directly to the caster about the effect and target
	        // "{0} looks more agile!"
	        if (msgCaster)
	        {
		        if (Caster is GamePlayer pCaster && Owner != pCaster)
			        ((SpellHandler) SpellHandler).MessageToLiving(pCaster, Util.MakeSentence(SpellHandler.Spell.Message2, Owner.GetName(0, true)), eChatType.CT_Spell);
		        else if (Caster is GameNPC casterPet && casterPet.ControlledBrain != null && casterPet.ControlledBrain.GetPlayerOwner() != null)
		        {
			        var petCaster = casterPet.ControlledBrain.GetPlayerOwner().Client.Player;
			        if (petCaster != null)
						((SpellHandler) SpellHandler).MessageToLiving(petCaster, Util.MakeSentence(SpellHandler.Spell.Message2, Owner.GetName(0, true)), eChatType.CT_Spell);
		        }
	        }

	        // Sends a third-person message to certain players around the target
	        // "{0} looks more agile!"
	        if (msgArea)
	        {
		        if ((Owner is GameNPC npcOwner && npcOwner.Realm == 0) && (Caster is GameNPC npcCaster && npcCaster.Realm == 0))
		        {
			        foreach (GamePlayer p in Owner.GetPlayersInRadius(WorldMgr.INFO_DISTANCE))
				        if (p != null)
					        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message2, npcOwner.GetName(0, upperCase)), eChatType.CT_Spell);
		        }
		        else
		        {
			        foreach (GamePlayer p in Owner.GetPlayersInRadius(WorldMgr.INFO_DISTANCE))
			        {
				        if (p == null)
					        return;
				        if (Owner is GamePlayer pOwner && p == pOwner)
					        continue;
				        if (p == Caster)
					        continue;

				        // X-realm coordination dissuading among soloers
				        // If you are not a member of the caster or target's group or realm, you cannot see spell messages
				        // If you are solo player, you will only see effects on other solo players

				        // Group player logic
				        if (p.Group != null)
				        {
					        // If the group contains the target player, all members of the group can see the message
					        if (Owner is GamePlayer aePlayer && p.Group.GetPlayersInTheGroup().Contains(aePlayer))
						        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message2, aePlayer.GetName(0, upperCase)), eChatType.CT_Spell);
					        // If the group contains the caster, all members of the group can see the message
					        else if (Caster is GamePlayer gpCaster && p.Group.GetPlayersInTheGroup().Contains(gpCaster))
						        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message2, Owner.GetName(0, upperCase)), eChatType.CT_Spell);
				        }
				        // Solo player logic
				        else if (p.Group == null)
				        {
					        // Solo players can only see other player spell messages if the caster is friendly and the target is from an enemy realm
					        if (p.Realm == Caster.Realm && p.Realm != Owner.Realm)
					        {
						        // If the target is a player and solo, then show messages
						        if (Owner is GamePlayer pSolo && pSolo.Group == null)
							        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message2, pSolo.GetName(0, upperCase)), eChatType.CT_Spell);
					        }
					        // If the player's realm isn't the same as the caster, but is the same as the target and the target is a player running solo, then see messages
					        else if (p.Realm != Caster.Realm && p.Realm == Owner.Realm)
					        {
						        // If the target is a player and solo, then show messages
						        if (Owner is GamePlayer pSolo2 && pSolo2.Group == null)
							        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message2, pSolo2.GetName(0, upperCase)), eChatType.CT_Spell);
					        }
					        else if (p.Realm == Caster.Realm && Caster == Owner)
					        {
						        // If the target is a player and solo, then show messages
						        if (Owner is GamePlayer pSolo2 && pSolo2 != null && pSolo2.Group == null)
							        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message2, pSolo2.GetName(0, upperCase)), eChatType.CT_Spell);
					        }
				        }
			        }
		        }
	        }
	    }
        #endregion Effect Start Messages

        #region Effect End Messages
        /// <summary>
        /// Sends effect messages to all nearby/associated players when an ability/spell/style effect ends on a target
        /// </summary>
        /// <param name="target">The owner of the effect.</param>
        /// <param name="msgTarget">If 'true', the system sends a first-person spell message to the target/owner of the effect.</param>
        /// <param name="msgCaster">If 'true', the system sends a third-person spell message to the caster triggering the effect, regardless of their proximity to the target.</param>
        /// <param name="msgArea">If 'true', the system sends a third-person message to all players within range of the target.</param>
        /// <returns>'Message3' and 'Message4' values from the 'spell' table.</returns>
        public void OnEffectExpiresMsg(GameLiving target, bool msgTarget, bool msgCaster, bool msgArea)
        {
	        // If the target variable is at the start of the string, capitalize their name or article
	        var upperCase = SpellHandler.Spell.Message4.StartsWith("{0}");

	        if (target == null)
		        return;

	        Owner = target;
	        m_caster = SpellHandler.Caster;

	        // Sends a first-person message directly to the caster's target, if they are a player
	        // "Your agility returns to normal."
	        if (msgTarget)
	        {
		        if (Owner is GamePlayer targetOwner)
			        ((SpellHandler) SpellHandler).MessageToLiving(targetOwner, Util.MakeSentence(SpellHandler.Spell.Message3), eChatType.CT_Spell);
	        }

	        // Sends a third-person message directly to the caster about the effect and target
	        // "{0}'s enhanced agility fades."
	        if (msgCaster)
	        {
		        if (Caster is GamePlayer pCaster && Owner != pCaster)
			        ((SpellHandler) SpellHandler).MessageToLiving(pCaster, Util.MakeSentence(SpellHandler.Spell.Message4, Owner.GetName(0, true)), eChatType.CT_Spell);
		        else if (Caster is GameNPC casterPet && casterPet.ControlledBrain != null && casterPet.ControlledBrain.GetPlayerOwner() != null)
		        {
			        var petCaster = casterPet.ControlledBrain.GetPlayerOwner().Client.Player;
			        if (petCaster != null)
						((SpellHandler) SpellHandler).MessageToLiving(petCaster, Util.MakeSentence(SpellHandler.Spell.Message4, Owner.GetName(0, true)), eChatType.CT_Spell);
		        }
	        }

	        // Sends a third-person message to certain players around the target
	        // "{0}'s enhanced agility fades."
	        if (msgArea)
	        {
		        if ((Owner is GameNPC npcOwner && npcOwner.Realm == 0) && (Caster is GameNPC npcCaster && npcCaster.Realm == 0))
		        {
			        foreach (GamePlayer p in Owner.GetPlayersInRadius(WorldMgr.INFO_DISTANCE))
				        if (p != null)
					        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message4, npcOwner.GetName(0, upperCase)), eChatType.CT_Spell);
		        }
		        else
		        {
			        foreach (GamePlayer p in Owner.GetPlayersInRadius(WorldMgr.INFO_DISTANCE))
			        {
				        if (p == null)
					        return;
				        if (Owner is GamePlayer pOwner && p == pOwner)
					        continue;
				        if (p == Caster)
					        continue;

				        // X-realm coordination dissuading among soloers
				        // If you are not a member of the caster or target's group or realm, you cannot see spell messages
				        // If you are solo player, you will only see effects on other solo players

				        // Group player logic
				        if (p.Group != null)
				        {
					        // If the group contains the target player, all members of the group can see the message
					        if (Owner is GamePlayer aePlayer && p.Group.GetPlayersInTheGroup().Contains(aePlayer))
						        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message4, aePlayer.GetName(0, upperCase)), eChatType.CT_Spell);
					        // If the group contains the caster, all members of the group can see the message
					        else if (Caster is GamePlayer gpCaster && p.Group.GetPlayersInTheGroup().Contains(gpCaster))
						        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message4, Owner.GetName(0, upperCase)), eChatType.CT_Spell);
				        }
				        // Solo player logic
				        else if (p.Group == null)
				        {
					        // Solo players can only see other player spell messages if the caster is friendly and the target is from an enemy realm
					        if (p.Realm == Caster.Realm && p.Realm != Owner.Realm)
					        {
						        // If the target is a player and solo, then show messages
						        if (Owner is GamePlayer pSolo && pSolo.Group == null)
							        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message4, pSolo.GetName(0, upperCase)), eChatType.CT_Spell);
					        }
					        // If the player's realm isn't the same as the caster, but is the same as the target and the target is a player running solo, then see messages
					        else if (p.Realm != Caster.Realm && p.Realm == Owner.Realm)
					        {
						        // If the target is a player and solo, then show messages
						        if (Owner is GamePlayer pSolo2 && pSolo2.Group == null)
							        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message4, pSolo2.GetName(0, upperCase)), eChatType.CT_Spell);
					        }
					        else if (p.Realm == Caster.Realm && Caster == Owner)
					        {
						        // If the target is a player and solo, then show messages
						        if (Owner is GamePlayer pSolo2 && pSolo2 != null && pSolo2.Group == null)
							        ((SpellHandler) SpellHandler).MessageToLiving(p, Util.MakeSentence(SpellHandler.Spell.Message4, pSolo2.GetName(0, upperCase)), eChatType.CT_Spell);
					        }
				        }
			        }
		        }
	        }
        }
        #endregion Effect End Messages

        #endregion Effect Messages

		public override PlayerXEffect getSavedEffect()
		{
			if (SpellHandler == null || SpellHandler.Spell == null) return null;
			
			PlayerXEffect eff = new PlayerXEffect();
			eff.Var1 = SpellHandler.Spell.ID;
			eff.Var2 = Effectiveness;
			eff.Var3 = (int)SpellHandler.Spell.Value;
			
			if (Duration > 0)
				eff.Duration = (int)(ExpireTick - GameLoop.GameLoopTime);
			else
				eff.Duration = 30 * 60 * 1000;
			
			eff.IsHandler = true;
			eff.SpellLine = SpellHandler.SpellLine.KeyName;
			return eff;
		}
    }
}