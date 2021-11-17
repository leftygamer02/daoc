using System;
using System.Collections;
using System.Reflection;
using DOL.Database;
using DOL.Events;
using DOL.GS;
using DOL.GS.Effects;
using DOL.GS.GameEvents;
using DOL.GS.PacketHandler;
using DOL.GS.Commands;

using log4net;

/*
 * Utility Scrolls v2.0
 * Etaew - Dawn of Light
 */

// namespace DOL.GS.GameEvents
// {
//     public class UtilityScrollsEvent
//     {
//         /// <summary>
//         /// Defines a logger for this class.
//         /// </summary>
//         public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
//
//         
//
//         [ScriptLoadedEvent]
//         public static void OnScriptLoaded(DOLEvent e, object sender, EventArgs args)
//         {   
//             Spell load;
// 			load = CampfireSpell;
//             log.Info("Systeme de Feux de camp Activé");
//         }
//
// 		/////////////////////////////////////////////////////// SPELL /////////////////////////////////////////////////////////
//         #region Spells
//         #region Campfire
//         protected static Spell m_CampfireSpell;
//         public static Spell CampfireSpell
//         {
//             get
//             {
//                 if (m_CampfireSpell == null)
//                 {
//                     DBSpell spell = new DBSpell();
// 					spell.AllowAdd = false;
//                     spell.CastTime = 0;
//                     spell.ClientEffect = 14804;
//                     spell.Icon = 14804;
//                     spell.Duration = 60;
//                     spell.Description = "Invoque un Feu de camp guerisseur pendant " + spell.Duration + " secondes.";
//                     spell.Name = "Comforting Flames";
//                     spell.Range = 0;
//                     spell.SpellID = 666675;
//                     spell.Target = "Self";
//                     spell.Type = "ComfortingFlames";
//                     spell.Value = 0;
//                     m_CampfireSpell = new Spell(spell, 1);
//                     SkillBase.GetSpellList(GlobalSpellsLines.Item_Effects).Add(m_CampfireSpell);
//                 }
//                 return m_CampfireSpell;
//             }
//         }
//         #endregion
//         		
//         #endregion
//
// 		/////////////////////////////////////////////////////// ITEMS /////////////////////////////////////////////////////////
// 		
//         #region Items
//         #region Campfire
//         protected static ItemTemplate m_tinderbox;
//         public static ItemTemplate Tinderbox
//         {
//             get
//             {
//                 if (m_tinderbox == null)
//                 {
//                     m_tinderbox = new ItemTemplate();
//                     m_tinderbox.CanDropAsLoot = false;
//                     m_tinderbox.Charges = 1;
//                     m_tinderbox.Id_nb = "tinderbox";
//                     m_tinderbox.IsDropable = true;
//                     m_tinderbox.IsPickable = true;
//                     m_tinderbox.IsTradable = true;
//                     m_tinderbox.Item_Type = 41;
//                     m_tinderbox.Level = 1;
//                     m_tinderbox.MaxCharges = 1;
//                     m_tinderbox.MaxCount = 2;
//                     m_tinderbox.Model = 1347;
//                     m_tinderbox.Name = "Tinderbox";
//                     m_tinderbox.Object_Type = (int)eObjectType.Magical;
//                     m_tinderbox.Realm = 0;
//                     m_tinderbox.SpellID = CampfireSpell.ID;
//                     m_tinderbox.Quality = 99;
//                     m_tinderbox.Price = 200000;
//                 }
//                 return m_tinderbox;
//             }
//         }
//         #endregion
//         		
//         #endregion
//     }
// }



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

// namespace DOL.GS.Commands
// {
//     [CmdAttribute("&scroll", //command to handle
//         ePrivLevel.Admin, //minimum privelege level
//         "creates a test scroll.", //command description
//         "/scroll")] //usage
//     public class ScrollCommandHandler : AbstractCommandHandler, ICommandHandler
//     {
//         public void OnCommand(GameClient client, string[] args)
//         {
//             client.Player.Inventory.AddItem(eInventorySlot.FirstEmptyBackpack, new InventoryItem(UtilityScrollsEvent.Tinderbox));
//             return ;
//         }
//     }
// }