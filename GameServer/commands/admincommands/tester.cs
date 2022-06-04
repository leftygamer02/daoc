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

/* <--- SendMessage Standardization --->
*  All messages now use translation IDs to both
*  centralize their location and standardize the method
*  of message calls used throughout this project. All messages affected
*  are in English. Other languages are not yet supported.
*
*  To  find a message at its source location, either use
*  the message body contained in the comment above the return
*  (e.g., // Message: "This is a message.") or the
*  translation ID (e.g., "AdminCommands.Account.Description").
*
*  To perform message changes, take note of your server settings.
*  If the `serverproperty` table setting `use_dblanguage`
*  is set to `True`, you must make your changes from the
*  `languagesystem` DB table.
*
*  If the `serverproperty` table setting
*  `update_existing_db_system_sentences_from_files` is set to `True`,
*  perform changes to messages from this file at "GameServer >
*  language > EN > OtherSentences.txt" and "Commands > AdminCommands.txt".
*
*  OPTIONAL: After changing a message, paste the new content
*  into the comment above the affected message return(s). This is
*  done for ease of reference. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using DOL.Database;
using DOL.GS.PacketHandler.Client.v168;
using DOL.Language;

namespace DOL.GS.Commands
{
	// See the comments above 'using' about SendMessage translation IDs
	[CmdAttribute(
		// Enter '/tester' to list all commands of this type
		"&tester",
		// Message: <----- '/tester' Commands (plvl 3) ----->
		"<----- '/tester' Commands ----->",
		ePrivLevel.Admin,
		// Message:
		"Perform various tester things, WIP",
		// Syntax:
		"'/tester speed <speed>' - Change your speed up to a maximum of 750.",
		// Message:
		"'/tester heal' - Heals your end, HP, and power up to full.",
		// Syntax:
		"'/tester godmode' - Prevents mobs from attacking you."
		)]
	public class TesterCommand : AbstractCommandHandler, ICommandHandler
	{
		public void OnCommand(GameClient client, string[] args)
		{
			if (client == null)
				return;

			/*
			if (args.Length < 1)
			{
				// Lists all '/tester' type subcommand syntax
				DisplaySyntax(client);
				return;
			}*/

			switch (args[1].ToLower())
			{
				#region Speed

				case "speed":
				{
					GamePlayer player = client.Player;

					if (player == null)
						return;


					if (args.Length == 1)
					{
						DisplayMessage(player, "Your" + " maximum speed is currently " + player.MaxSpeedBase + ".");
						return;
					}

					short speed;

					if (short.TryParse(args[2], out speed))
					{
						if (speed is <= 750 and >= 1)
						{
							player.MaxSpeedBase = speed;
							DisplayMessage(player, ("Your maximum speed is now " + player.MaxSpeedBase + "."));
						}
					}

					break;
				}
				#endregion Speed

				#region Heal
				case "heal":
				{
					GamePlayer player = client.Player;

					if (player == null)
						return;

					player.Health = player.MaxHealth;
					player.Endurance = player.MaxEndurance;
					player.Mana = player.MaxMana;

					if (player.effectListComponent.ContainsEffectForEffectType(eEffect.ResurrectionIllness))
					{
						EffectService.RequestCancelEffect(player.effectListComponent.GetAllEffects().FirstOrDefault(e => e.EffectType == eEffect.ResurrectionIllness));
					}

					if (player.effectListComponent.ContainsEffectForEffectType(eEffect.RvrResurrectionIllness))
					{
						EffectService.RequestCancelEffect(player.effectListComponent.GetAllEffects().FirstOrDefault(e => e.EffectType == eEffect.RvrResurrectionIllness));
					}
				}
					break;
				#endregion Heal
/*
				#region Invuln
				case "godmode":
				{
					GamePlayer player = client.Player;

					if (player == null)
						return;

					var getProp = player?.TempProperties.getProperty("tester_godmode", false);
					if (getProp == true)
						player?.TempProperties.getProperty("tester_godmode", false);
					else
						player?.TempProperties.getProperty("tester_godmode", true);
				}
					break;
				#endregion Invuln */

            }
		}
	}
}