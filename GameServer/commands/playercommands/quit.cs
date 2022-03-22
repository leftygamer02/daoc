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

namespace DOL.GS.Commands
{
	/// <summary>
	/// Handles all user-based interaction for the '/quit' command
	/// </summary>
	[CmdAttribute(
		"&quit",
		new[] { "&q" },
		// Message: '/quit' or '/q' - Removes the player from the world to the character selection screen.
		"PLCommands.Quit.CmdList.Description",
		// Message: <----- '/{0}' Command {1}----->
		"AllCommands.Header.General.Commands",
		// Required minimum privilege level to use the command
		ePrivLevel.Player,
		// Message: Removes the player from the world to the character selection screen, after a wait of 20 seconds.
		"PLCommands.Quit.Description",
		// Syntax: /quit
		"PLCommands.Quit.Syntax.Quit",
		// Message: Removes the player from the world after 20 seconds.
		"PLCommands.Quit.Usage.Quit")]
	public class QuitCommandHandler : AbstractCommandHandler, ICommandHandler
	{
		public void OnCommand(GameClient client, string[] args)
		{
			if (IsSpammingCommand(client.Player, "quit"))
				return;

			client.Player.Quit(false);
		}
	}
}