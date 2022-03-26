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
	/// Handles all user-based interaction for the '/bind' command
	/// </summary>
	[CmdAttribute(
		// Enter '/bind' to activate this command
		"&bind",
		// Message: '/bind' - Sets a respawn point for your character. After you die, '/release' will send you to this location.
		"PLCommands.Bind.CmdList.Description",
		// Message: <----- '/{0}' Command {1}----->
		"AllCommands.Header.General.Commands",
		// Required minimum privilege level to use the command
		ePrivLevel.Player,
		// Message: Sets a respawn point for your character. After you die, '/release' will send you to this location.
		"PLCommands.Bind.Description",
		// Syntax: /bind
		"PLCommands.Bind.Syntax.Bind",
		// Message: Binds your character to a location, which serves as your default respawn location after you die and release.
		"PLCommands.Bind.Usage.Bind")]
	public class BindCommandHandler : AbstractCommandHandler, ICommandHandler
	{
		/// <summary>
		/// Method to handle the command and any arguments
		/// </summary>
		/// <param name="client"></param>
		/// <param name="args"></param>
		public void OnCommand(GameClient client, string[] args)
		{
			if (IsSpammingCommand(client.Player, "bind"))
				return;
			
			client.Player.Bind(false);
		}
	}
}