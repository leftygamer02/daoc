using System;
using DOL.Database;
using DOL.Events;
using DOL.GS.PacketHandler;

namespace DOL.GS.Commands
{
    [CmdAttribute(
        // Enter '/summonmerchant' to list all associated subcommands
        "&summonmerchant",
        // Message: '/summonmerchant' - Summons a merchant that sells charged items to summon additional merchants.
        "PLCommands.SummonMerch.CmdList.Description",
        // Message: <----- '/{0}' Command {1}----->
        "AllCommands.Header.General.Commands",
        // Required minimum privilege level to use the command
        ePrivLevel.Admin,
        // Message: Summons a merchant that sells charged items to summon additional merchants.
        "PLCommands.SummonMerch.Description",
        // Syntax: /summonmerchant
        "PLCommands.SummonMerch.Syntax.SummonMerchant",
        // Message: Summons a merchant.
        "PLCommands.SummonMerch.Usage.SummonMerchant")]
    public class SummonMerchantCommandHandler : AbstractCommandHandler, ICommandHandler
    {
        [ScriptLoadedEvent]
        public static void OnScriptLoaded(DOLEvent e, object sender, EventArgs args)
        {
            Spell load;
            load = MerchantSpell;
        }

        public const string SummonMerch = "SummonMerch";

        public void OnCommand(GameClient client, string[] args)
        { 
            var player = client.Player;
            var merchTick = player.TempProperties.getProperty(SummonMerch, 0L);
            var changeTime = GameLoop.GameLoopTime - merchTick;
            
            if (changeTime < 30000 && client.Account.PrivLevel == 1) // Staff can override timer
            {
                // Message: You must wait {0} more seconds before you may use this command again!
                ChatUtil.SendTypeMessage("error", client, "AllCommands.Command.Err.YouMustWaitToUse", null);
                return;
            }
            
            player.TempProperties.setProperty(SummonMerch, GameLoop.GameLoopTime);

            var line = new SpellLine("MerchantCast", "Merchant Cast", "unknown", false);
            var spellHandler = ScriptMgr.CreateSpellHandler(client.Player, MerchantSpell, line);
            if (spellHandler != null)
                spellHandler.StartSpell(client.Player);
            // Message: You have summoned a merchant!
            ChatUtil.SendTypeMessage("success", client, "PLCommands.SummonMerch.Msg.YouSummoned", null);
        }

        protected static Spell MMerchantSpell;

        public static Spell MerchantSpell
        {
            get
            {
                if (MMerchantSpell == null)
                {
                    var spell = new DBSpell {CastTime = 0, ClientEffect = 0, Duration = 30};
                    spell.Description = "Summons a merchant to your location for " + spell.Duration + " seconds.";
                    spell.Name = "Merchant Spell";
                    spell.Type = "SummonMerchant";
                    spell.Range = 0;
                    spell.SpellID = 121232;
                    spell.Target = "Self";
                    spell.Value = MerchantTemplate.TemplateId;
                    MMerchantSpell = new Spell(spell, 1);
                    SkillBase.GetSpellList(GlobalSpellsLines.Item_Effects).Add(MMerchantSpell);
                }
                return MMerchantSpell;
            }
        }

        protected static NpcTemplate MMerchantTemplate;

        public static NpcTemplate MerchantTemplate
        {
            get
            {
                if (MMerchantTemplate == null)
                {
                    MMerchantTemplate = new NpcTemplate();
                    MMerchantTemplate.Flags += (byte) GameNPC.eFlags.GHOST + (byte) GameNPC.eFlags.PEACE;
                    MMerchantTemplate.Name = "Merchant";
                    MMerchantTemplate.ClassType = "DOL.GS.Scripts.SummonedMerchant";
                    MMerchantTemplate.Model = "50";
                    MMerchantTemplate.TemplateId = 93049;
                    NpcTemplateMgr.AddTemplate(MMerchantTemplate);
                }
                return MMerchantTemplate;
            }
        }
    }
}