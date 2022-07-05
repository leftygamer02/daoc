/*
 *
 * ATLAS - Script to adjust stats of reset characters above level 5
 *
 */

using System;
using System.Reflection;
using DOL.Database;
using DOL.Events;
using DOL.GS.PacketHandler;
using DOL.GS.Scripts;
using log4net;

#region LoginEvent
namespace DOL.GS.GameEvents
{
    public class LaunchRestartStatsScript
    {
        
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [GameServerStartedEvent]
        public static void OnServerStart(DOLEvent e, object sender, EventArgs arguments)
        {
            GameEventMgr.AddHandler(GamePlayerEvent.GameEntered, new DOLEventHandler(PlayerEntered));
        }

        /// <summary>
        /// Event handler fired when server is stopped
        /// </summary>
        [GameServerStoppedEvent]
        public static void OnServerStop(DOLEvent e, object sender, EventArgs arguments)
        {
            GameEventMgr.RemoveHandler(GamePlayerEvent.GameEntered, new DOLEventHandler(PlayerEntered));
        }
        
        /// <summary>
        /// Event handler fired when players enters the game
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        private static void PlayerEntered(DOLEvent e, object sender, EventArgs arguments)
        {
            if (sender is not GamePlayer player) return;
            
            var charID = player.ObjectId;
            
            var stats = DOLDB<PlayerStatsReset>.SelectObject(DB.Column("CharacterID").IsEqualTo(charID));

            if (stats == null) return;
            
            player.Out.SendMessage($"PREVIOUS STATS", eChatType.CT_Important, eChatLoc.CL_SystemWindow);

            var stsMessage = "";
            
            for (eProperty stat = eProperty.Stat_First; stat <= eProperty.Stat_Last; stat++)
            {
                stsMessage += GlobalConstants.PropertyToName(stat);
                stsMessage += player.GetModified(stat);
            }
            
            player.Out.SendMessage(stsMessage, eChatType.CT_Important, eChatLoc.CL_SystemWindow);

            player.Out.SendMessage($"Adjusting stats..", eChatType.CT_Important, eChatLoc.CL_SystemWindow);

            for (var i = 6; i <= stats.PreviousLevel ; i++)
            {
                if (player.CharacterClass.PrimaryStat != eStat.UNDEFINED)
                {
                    player.ChangeBaseStat(player.CharacterClass.PrimaryStat, -1);
                }
                if (player.CharacterClass.SecondaryStat != eStat.UNDEFINED && ((i - 6) % 2 == 0))
                {
                    player.ChangeBaseStat(player.CharacterClass.SecondaryStat, -1);
                }
                if (player.CharacterClass.TertiaryStat != eStat.UNDEFINED && ((i - 6) % 3 == 0))
                {
                    player.ChangeBaseStat(player.CharacterClass.TertiaryStat, -1);
                }
            }
            
            player.Out.SendMessage($"NEW STATS", eChatType.CT_Important, eChatLoc.CL_SystemWindow);

            stsMessage = "";
            
            for (eProperty stat = eProperty.Stat_First; stat <= eProperty.Stat_Last; stat++)
            {
                stsMessage += GlobalConstants.PropertyToName(stat);
                stsMessage += player.GetModified(stat);
            }
            
            player.Out.SendMessage(stsMessage, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
            
            player.Out.SendMessage($"Saving..", eChatType.CT_Important, eChatLoc.CL_SystemWindow);
            player.SaveIntoDatabase();
            
            player.Out.SendMessage($"Removing entry from table..", eChatType.CT_Important, eChatLoc.CL_SystemWindow);
            GameServer.Database.DeleteObject(stats);

        }
    }
}
#endregion