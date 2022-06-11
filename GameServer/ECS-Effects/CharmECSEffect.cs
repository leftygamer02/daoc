using DOL.AI.Brain;
using DOL.Events;
using DOL.GS.PacketHandler;
using DOL.GS.Spells;
using System.Linq;
using DOL.Language;

namespace DOL.GS
{
    public class CharmECSGameEffect : ECSGameSpellEffect
    {
        public CharmECSGameEffect(ECSGameEffectInitParams initParams)
            : base(initParams) { }

        public override void OnStartEffect()
        {
            GamePlayer casterPlayer = SpellHandler.Caster as GamePlayer;
            GameNPC charmMob = Owner as GameNPC;

            if (casterPlayer != null && charmMob != null)
            {
                if (((CharmSpellHandler)SpellHandler).m_controlledBrain == null && !(charmMob.Brain is ControlledNpcBrain))
                {
                    ((CharmSpellHandler)SpellHandler).m_controlledBrain = new ControlledNpcBrain(casterPlayer);
                }
                else
                {
                    ((CharmSpellHandler)SpellHandler).m_controlledBrain = charmMob.Brain as ControlledNpcBrain;
                    ((CharmSpellHandler)SpellHandler).m_isBrainSet = true;
                }

                if (!((CharmSpellHandler)SpellHandler).m_isBrainSet &&
                    !((CharmSpellHandler)SpellHandler).m_controlledBrain.IsActive)
                {

                    charmMob.AddBrain(((CharmSpellHandler)SpellHandler).m_controlledBrain);
                    ((CharmSpellHandler)SpellHandler).m_isBrainSet = true;

                    GameEventMgr.AddHandler(charmMob, GameLivingEvent.PetReleased, (((CharmSpellHandler)SpellHandler).ReleaseEventHandler));
                }

                if (casterPlayer.ControlledBrain != ((CharmSpellHandler)SpellHandler).m_controlledBrain)
                {
                    
                    casterPlayer.SetControlledBrain(((CharmSpellHandler)SpellHandler).m_controlledBrain);
                    // Message: {0} is now under your control.
                    // Message: {0} is now charmed!
                    OnEffectStartsMsg(Owner, true, true, true);

                    foreach (GamePlayer player in charmMob.GetPlayersInRadius(WorldMgr.VISIBILITY_DISTANCE))
                    {
                        player.Out.SendNPCCreate(charmMob);
                        if (charmMob.Inventory != null)
                            player.Out.SendLivingEquipmentUpdate(charmMob);

                        player.Out.SendObjectGuildID(charmMob, casterPlayer.Guild);
                    }
                }
                ((CharmSpellHandler)SpellHandler).SendEffectAnimation(charmMob, 0, false, 1);
            }
        }

        public override void OnStopEffect()
        {
            GamePlayer casterPlayer = SpellHandler.Caster as GamePlayer;
            GameNPC charmMob = Owner as GameNPC;

            if (casterPlayer != null && charmMob != null)
            {
                GameEventMgr.RemoveHandler(charmMob, GameLivingEvent.PetReleased, (((CharmSpellHandler) SpellHandler).ReleaseEventHandler));
                ControlledNpcBrain oldBrain = (ControlledNpcBrain)casterPlayer.ControlledBrain;
                casterPlayer.SetControlledBrain(null);
                
                // Message: You lose control of {0}!
                // Message: {0} is no longer charmed!
                OnEffectExpiresMsg(Owner, true, true, true);

                lock (charmMob.BrainSync)
                {
                    var immunityEffects = charmMob.effectListComponent.GetSpellEffects().Where(e => e.TriggersImmunity).ToArray();
                    for (int i = 0; i < immunityEffects.Length; i++)
                    {
                        EffectService.RequestImmediateCancelEffect(immunityEffects[i]);
                    }

                    charmMob.StopAttack();
                    charmMob.RemoveBrain(oldBrain);

                    charmMob.AddBrain(new StandardMobBrain());
                    ((CharmSpellHandler) SpellHandler).m_isBrainSet = false;

                    if (charmMob.Brain != null && charmMob.Brain is IOldAggressiveBrain)
                    {

                        ((IOldAggressiveBrain)charmMob.Brain).ClearAggroList();

                        if (SpellHandler.Spell.Pulse != 0 && SpellHandler.Caster.ObjectState == GameObject.eObjectState.Active && SpellHandler.Caster.IsAlive
                        && !SpellHandler.Caster.IsStealthed)
                        {
                            ((IOldAggressiveBrain)charmMob.Brain).AddToAggroList(SpellHandler.Caster, SpellHandler.Caster.Level * 10);
                            charmMob.StartAttack(SpellHandler.Caster);
                            charmMob.LastAttackedByEnemyTickPvE = GameLoop.GameLoopTime;
                        }
                        else if (charmMob.IsWithinRadius(charmMob.SpawnPoint, 5000))
                        {
                            charmMob.WalkToSpawn();
                        }
                        else
                        {
                            charmMob.Brain.Stop();
                            casterPlayer.Notify(GameNPCEvent.PetLost);
                            charmMob.Die(null);
                        }
                    }
                }

                // remove NPC with new brain from all attackers aggro list
                lock (charmMob.attackComponent.Attackers)
                    foreach (GameObject attacker in charmMob.attackComponent.Attackers)
                    {
                        if (attacker == null || !(attacker is GameNPC))
                            continue;

                        if (((GameNPC)attacker).Brain != null && ((GameNPC)attacker).Brain is IOldAggressiveBrain)
                        {
                            ((IOldAggressiveBrain) ((GameNPC) attacker).Brain).RemoveFromAggroList(charmMob);
                            ((IOldAggressiveBrain) ((GameNPC) attacker).Brain).AddToAggroList(casterPlayer, casterPlayer.Level * 10);
                            ((GameNPC) attacker).StartAttack(casterPlayer);
                            ((GameNPC) attacker).LastAttackedByEnemyTickPvE = GameLoop.GameLoopTime;
                        }
                    }

                ((CharmSpellHandler) SpellHandler)?.m_controlledBrain?.ClearAggroList();
                charmMob.StopFollowing();

                charmMob.TempProperties.setProperty(GameNPC.CHARMED_TICK_PROP, charmMob.CurrentRegion.Time);


                foreach (GamePlayer ply in charmMob.GetPlayersInRadius(WorldMgr.VISIBILITY_DISTANCE))
                {
                    if (charmMob.IsAlive)
                    {
                        ply.Out.SendNPCCreate(charmMob);

                        if (charmMob.Inventory != null)
                            ply.Out.SendLivingEquipmentUpdate(charmMob);

                        ply.Out.SendObjectGuildID(charmMob, null);
                    }
                }
            }
            ECSPulseEffect song = EffectListService.GetPulseEffectOnTarget(casterPlayer);
            if (charmMob != null && song != null && song.SpellHandler.Spell.InstrumentRequirement == 0 && !charmMob.IsWithinRadius(casterPlayer, SpellHandler.Spell.Range))
            {
                EffectService.RequestImmediateCancelConcEffect(song);
            }
            ((CharmSpellHandler) SpellHandler).m_controlledBrain = null;
        }
    }
}
