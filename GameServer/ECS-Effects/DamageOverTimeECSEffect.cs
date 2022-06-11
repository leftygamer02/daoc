using DOL.GS.PacketHandler;
using DOL.GS.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.GS
{
    public class DamageOverTimeECSGameEffect : ECSGameSpellEffect
    {
        public DamageOverTimeECSGameEffect(ECSGameEffectInitParams initParams)
            : base(initParams) 
        {
            NextTick = GameLoop.GameLoopTime;
        }

        public override void OnStartEffect()
        {
            // "Searing pain fills your mind!"
            // "{0} is wracked with pain!"
            OnEffectStartsMsg(Owner, true, true, true);

            // Remove stealth on first application since the code that normally handles removing stealth on
            // attack ignores DoT damage, since only the first tick of a DoT should remove stealth.            
            if (OwnerPlayer != null && !OwnerPlayer.effectListComponent.ContainsEffectForEffectType(eEffect.Vanish))
                OwnerPlayer.Stealth(false);
        }

        public override void OnStopEffect()
        {
            if (EffectType == eEffect.Bleed && !Owner.effectListComponent.ContainsEffectForEffectType(eEffect.Bleed))
                Owner.TempProperties.removeProperty(StyleBleeding.BLEED_VALUE_PROPERTY);
            
            // "Your mental agony fades."
            // "{0}'s mental agony fades."
            OnEffectExpiresMsg(Owner, true, true, true);
        }

        public override void OnEffectPulse()
        {
            if (OwnerPlayer != null)
                return;

            if (Owner.IsAlive == false)
            {
                // "Your mental agony fades."
                // "{0}'s mental agony fades."
                OnEffectExpiresMsg(Owner, true, true, true);
                EffectService.RequestImmediateCancelEffect(this);
            }

            if (Owner.IsAlive)
            {
                if (SpellHandler is DoTSpellHandler handler)
                {
                    // "Searing pain fills your mind!"
                    // "{0} is wracked with pain!"
                    OnEffectStartsMsg(Owner, true, false, true);

                    double postRAEffectiveness = Effectiveness;
                    if (handler.Caster.effectListComponent.ContainsEffectForEffectType(eEffect.Viper) && SpellHandler.Spell.IsPoison)
                        postRAEffectiveness *= 2;
                    
                    handler.OnDirectEffect(Owner, postRAEffectiveness);
                }
                else if (SpellHandler is StyleBleeding bleedHandler)
                {

                    if (Owner.effectListComponent.ContainsEffectForEffectType(eEffect.Bleed)
                        && Owner.TempProperties.getProperty<int>(StyleBleeding.BLEED_VALUE_PROPERTY) > bleedHandler.Spell.Damage)
                    {
                        if (OwnerPlayer != null)
                            bleedHandler.MessageToCaster("A stronger bleed effect already exists on your target.", eChatType.CT_SpellResisted);
                        EffectService.RequestCancelEffect(this);
                        return;
                    }

                    if (StartTick + PulseFreq > GameLoop.GameLoopTime && Owner.TempProperties.getProperty<int>(StyleBleeding.BLEED_VALUE_PROPERTY) < bleedHandler.Spell.Damage)
                    {
                        Owner.TempProperties.setProperty(StyleBleeding.BLEED_VALUE_PROPERTY, (int)bleedHandler.Spell.Damage); 
                    }

                    int bleedValue = Owner.TempProperties.getProperty<int>(StyleBleeding.BLEED_VALUE_PROPERTY);

                    AttackData ad = bleedHandler.CalculateDamageToTarget(Owner, 1.0);
                    bleedHandler.SendDamageMessages(ad);

                    // attacker must be null, attack result is 0x0A
                    foreach (GamePlayer player in ad.Target.GetPlayersInRadius(WorldMgr.VISIBILITY_DISTANCE))
                    {
                        player.Out.SendCombatAnimation(null, ad.Target, 0, 0, 0, 0, 0x0A, ad.Target.HealthPercent);
                    }

                    // "Searing pain fills your mind!"
                    // "{0} is wracked with pain!"
                    OnEffectStartsMsg(Owner, true, false, true);

                    // send animation before dealing damage else dead livings show no animation
                    ad.Target.OnAttackedByEnemy(ad);
                    ad.Attacker.DealDamage(ad);

                    if(bleedValue > 1)
                        bleedValue--;

                    if (!Owner.IsAlive)
                    {
                        // "Your mental agony fades."
                        // "{0}'s mental agony fades."
                        OnEffectExpiresMsg(Owner, true, true, true);
                        EffectService.RequestImmediateCancelEffect(this);
                    }
                    else Owner.TempProperties.setProperty(StyleBleeding.BLEED_VALUE_PROPERTY, bleedValue);
                }

                if (Owner.Realm == 0 || SpellHandler.Caster.Realm == 0)
                    Owner.LastAttackTickPvE = GameLoop.GameLoopTime;
                else
                    Owner.LastAttackTickPvP = GameLoop.GameLoopTime;
            }

            if (LastTick == 0)
            {
                LastTick = GameLoop.GameLoopTime;
                NextTick = LastTick + PulseFreq;
            }
            else
            {
                LastTick += PulseFreq;
                NextTick = LastTick + PulseFreq;
            }
            
            if(SpellHandler.Caster is GamePet)
                Owner.StartInterruptTimer(SpellHandler.Caster.SpellInterruptDuration, AttackData.eAttackType.Spell, SpellHandler.Caster);
                
        }
    }
}
