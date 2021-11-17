using System;
using System.Collections;
using System.Reflection;
using DOL.Database;
using DOL.Events;
using DOL.GS;
using DOL.GS.Effects;
using DOL.GS.PacketHandler;

/*
 * Utility Scrolls v2.0
 * Etaew - Dawn of Light
 */

namespace DOL.GS.Spells
{
    [SpellHandlerAttribute("ComfortingFlames")]
    public class CampfireSpellHandler : DoTSpellHandler
    {
        private GameObject m_campfire;

        public CampfireSpellHandler(GameLiving caster, Spell spell, SpellLine line)
            : base(caster, spell, line)
        { }

        public override void OnEffectPulse(GameSpellEffect effect)
        {
            if (m_campfire == null) return;


            foreach (GamePlayer player in m_campfire.GetPlayersInRadius(500))
            {
                if (player.IsAlive == false) continue;
				if (player.CharacterClass.Name == "Vampiir") continue;
				if (player.InCombat == true) continue;

                if ((GameServer.ServerRules.IsSameRealm(Caster, player, true)) && (player.InCombat == false))
                {
                    int mr = player.MaxMana / 20;
                    int hr = player.MaxHealth / 20;
                    int er = player.MaxEndurance / 20;

                    // Don't stack
                    int stack = 0;
                    foreach (GameObject obj in player.GetItemsInRadius(500))
                    {
                        if (obj.Model == 3460) stack++;
                    }

                    if (stack > 1)
                    {
                        // Divide the regs by the number of campfires so that they heal with double frequency and half amount
                        mr = mr / stack;
                        hr = hr / stack;
                        er = er / stack;
                    }

                    if (mr > (player.MaxMana - player.Mana)) mr = player.MaxMana - player.Mana;
                    if (hr > (player.MaxHealth - player.Health)) hr = player.MaxHealth - player.Health;
                    if (er > (player.MaxEndurance - player.Endurance)) er = player.MaxEndurance - player.Endurance;

                    if (hr > 0)
                    {
                        player.Health += hr;
                        player.Out.SendMessage("You regain " + hr.ToString() + " hit points from the campfire!", eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
                    }
                    if (mr > 0)
                    {
                        player.Mana += mr;
                        player.Out.SendMessage("You regain " + mr.ToString() + " power from the campfire!", eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
                    }
                    if (er > 0)
                    {
                        player.Endurance += mr;
                        player.Out.SendMessage("You regain " + er.ToString() + " endurance from the campfire!", eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
                    }
                }
            }
        }

        public override void OnEffectStart(GameSpellEffect effect)
        {
            if (Caster.CurrentRegion.IsRvR == true)
            {
                if (Caster is GamePlayer)
                {
                    GamePlayer player = Caster as GamePlayer;
                    player.Out.SendMessage("After a short blaze up the flames expire. This spell does not work in RvR areas.", eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
                }
                return;
            }
            m_campfire = new GameStaticItem();
            m_campfire.X = Caster.X;
            m_campfire.Y = Caster.Y;
            m_campfire.Z = Caster.Z;
            m_campfire.Heading = Caster.Heading;
            m_campfire.CurrentRegionID = Caster.CurrentRegionID;
            m_campfire.Realm = Caster.Realm;
            m_campfire.Model = 3460;
            m_campfire.Name = "Campfire";
            m_campfire.AddToWorld();
        }

        public override bool IsOverwritable(GameSpellEffect compare)
        {
            return false;
        }

        public override int OnEffectExpires(GameSpellEffect effect, bool noMessages)
        {
            base.OnEffectExpires(effect, noMessages);
            if (m_campfire == null) return 0;
            m_campfire.Delete();
            return 0;
        }
    }
}