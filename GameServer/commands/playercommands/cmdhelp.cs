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
using DOL.GS.PacketHandler;
using DOL.Language;
using System.Collections.Generic;

namespace DOL.GS.Commands
{
	[CmdAttribute("&cmd", // The main command controlled here
		new[] {"&cmdhelp"}, // Secondary syntax to trigger the main command
		// Message: '/cmd' - Returns a list of all available commands, along with a description of their purpose.
		"PLCommands.Cmd.CmdList.Description",
		// Message: <----- '/{0}' Command {1}----->
		"AllCommands.Header.General.Commands",
		ePrivLevel.Player, // Required minimum privilege level to use the command
		// Message: Returns a list of all available commands, along with a description of their purpose.
		"PLCommands.Cmd.Description",
		// Syntax: /cmd
		"PLCommands.Cmd.Syntax.Cmdhelp",
		// Message: Returns a list of all commands available to the player.
		"PLCommands.Cmd.Usage.Cmdhelp",
		// Syntax: /cmd <plvl>
		"PLCommands.Cmd.Syntax.Plvl",
		// Message: Returns a list of all commands available to the privilege level specified.
		"PLCommands.Cmd.Usage.Plvl",
		// Syntax: /cmd <command>
		"PLCommands.Cmd.Syntax.Command",
		// Message: Returns a list of all subcommands associated with the command type specified.
		"PLCommands.Cmd.Usage.Command")]
	public class CmdHelpCommandHandler : AbstractCommandHandler, ICommandHandler
	{
		public void OnCommand(GameClient client, string[] args)
		{
			// Anti-spamming measure
			if (IsSpammingCommand(client.Player, "cmd", 500))
				return;

			// Players may only view commands associated with their plvl
			ePrivLevel privilegeLevel = (ePrivLevel)client.Account.PrivLevel;
			bool isCommand = true; // Checks to make sure the related command(s) exist

			// Check if client is trying to list commands by plvl (e.g., '/cmd 3') by trying to convert arg[1] into int
			if (args.Length > 1 && client.Account.PrivLevel > 1)
			{
				try
				{
					privilegeLevel = (ePrivLevel)Convert.ToUInt32(args[1]);
				}
				catch (Exception)
				{
					isCommand = false;
				}
			}

			// If converted to int successfully, display commands by plvl association
			if (isCommand && client.Account.PrivLevel > 1)
			{
                String[] commandList = GetCommandList(privilegeLevel); // Get all commands for that plvl
                // Message: <----- {0} Commands ----->
				ChatUtil.SendTypeMessage("cmdHeader", client, "PLCommands.Header.Cmd.AvailableCmds", privilegeLevel.ToString());

				foreach (String command in commandList)
				{
					ChatUtil.SendTypeMessage("cmdUsage", client, command, null);
				}
					
			}
			else // If the player attempts to filter by command type (e.g., '/cmd jump')
			{
				string command = args[1];
				
				if (command[0] != '&')
					command = "&" + command;

				ScriptMgr.GameCommand gameCommand = ScriptMgr.GetCommand(command);

				if (gameCommand == null)
					// Message: The {0} command does not exist.
					ChatUtil.SendTypeMessage("error", client, "PLCommands.Cmd.Err.DoesNotExist", command);
                else
				{
					// Message: <----- '/{0}' Subcommands ----->
					ChatUtil.SendTypeMessage("cmdHeader", client, "PLCommands.Header.Cmd.Subcommands", command);

					foreach (var usage in gameCommand.Usage)
					{
						if (usage.Contains(".Syntax."))
							ChatUtil.SendTypeMessage("cmdSyntax", client, usage, null);
						else
							ChatUtil.SendTypeMessage("cmdUsage", client, usage, null);
					}
				}
			}
		}

        private static IDictionary<ePrivLevel, String[]> m_commandLists = new Dictionary<ePrivLevel, String[]>();
        private static object m_syncObject = new object();

        private String[] GetCommandList(ePrivLevel privilegeLevel)
        {
            lock (m_syncObject)
            {
                if (!m_commandLists.Keys.Contains(privilegeLevel))
                {
                    String[] commandList = ScriptMgr.GetCommandList(privilegeLevel, true);
                    Array.Sort(commandList);
                    m_commandLists.Add(privilegeLevel, commandList);
                }

                return m_commandLists[privilegeLevel];
            }
        }
    }
}