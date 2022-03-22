/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */

using System;
using DOL.GS;
using DOL.GS.PacketHandler;
using DOL.Database;

namespace DOL.GS.Commands
{
	/// <summary>
	/// Handles all user-based interaction for the '/team' command
	/// </summary>
	[CmdAttribute( 
	// Enter '/team' to see command syntax messages
	"&team",
	new [] { "&te" },
	// Message: '/team' or '/te' - Broadcasts a message to all Atlas server team members (i.e., plvl 2+).
	"GMCommands.CmdList.Team.Description",
	// Message: <----- '/{0}' Command {1}----->
	"AllCommands.Header.General.Commands",
	// Required minimum privilege level to use the command
   ePrivLevel.GM,
	// Message: Broadcasts a message to all Atlas server team members (i.e., plvl 2+).
	"GMCommands.Team.Description",
	// Syntax: '/team <message>' or '/te <message>'
	"GMCommands.Team.Syntax.Team",
	// Message: Broadcasts a message to the [TEAM] channel.
	"GMCommands.Team.Usage.Team")]

	public class TeamCommandHandler : AbstractCommandHandler, ICommandHandler
	{
		public void OnCommand(GameClient client, string[] args)
		{
			// Lists all '/team' command syntax
			if (args.Length < 2)
			{
				// Message: <----- '/{0}' Command {1}----->
				// Message: Use the following syntax for this command:
				// Syntax: '/team <message>' or '/te <message>'
				// Message: Broadcasts a message to the [TEAM] channel.
				DisplayHeadSyntax(client, "team", "", "", 2, false, "GMCommands.Team.Syntax.Team", "GMCommands.Team.Usage.Team");
				return;
			}

			// Identify message body
			string message = string.Join(" ", args, 1, args.Length - 1);

			foreach (GameClient player in WorldMgr.GetAllPlayingClients())
			{
				// Don't send team messages to Players
				if (player.Account.PrivLevel > 1)
				{
					// Message: [TEAM] {0}: {1}
					ChatUtil.SendTypeMessage("team", player, "Social.ReceiveMessage.Staff.Channel", client.Player.Name, message);
				}
			}
		}
	}
}
