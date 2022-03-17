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

using System.Collections.Generic;
using DOL.GS.PacketHandler;
using DOL.Language;

namespace DOL.GS
{
	/// <summary>
	/// Contains all methods used for sending players messages
	/// </summary>
	public static class ChatUtil
	{
		/// <summary>
		/// Used to send translated messages
		/// </summary>
		/// <param name="type">Determines the eChatType and eChatLoc to use for the message. Options include: "advise", "alliance", "battlegroup", "bgLeader", "broadcast", "centerSys", "chat", "command", "cmdDesc", "cmdHeader","cmdSyntax", "cmdUsage", "damageAdd", "damaged", "debug", "dialog", "emote", "error", "expires", "group", "guild", "help", "important", "killedByAlb", "killedByHib", "killedByMid", "lfg", "loot", "merchant", "missed", "officer", "othersCombat", "playerDied", "pulse", "resisted", "say", "screenCenter", "send", "skill", "spell", "staff", "system", "team", "trade", "yell", "youDied", "youHit", "youWereHit".</param>
		/// <param name="target">The target client receiving the message (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player.Name" (if no args, then use "null")</param>
		/// <note>Please paste the translation ID's associated message (English) into a comment above this method.</note>
		/// <example>ChatUtil.SendTypeMessage("cmdheader", client, "AdminCommands.Account.Description", null);</example>
		/// <comment>To perform message changes, reference the '.txt' files located in 'GameServer > language > EN'. Please remember, if you change the translation ID value, check all other language folders (e.g., DE, FR, ES, etc.) to ensure all translated strings correctly reflect the new ID.</comment>
		public static void SendTypeMessage(string type, GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);

			switch (type)
			{
				case "":
					return;
				case "advise": // Advice channel
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Advise, eChatLoc.CL_ChatWindow);
					break;
				case "alliance": // Guild alliance
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Alliance, eChatLoc.CL_ChatWindow);
					break;
				case "battlegroup":
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_BattleGroup, eChatLoc.CL_ChatWindow);
					break;
				case "bgLeader": // Battlegroup leader
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_BattleGroupLeader, eChatLoc.CL_ChatWindow);
					break;
				case "broadcast":
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Broadcast, eChatLoc.CL_ChatWindow);
					break;
				case "centerSys": // Center screen and system window
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_ScreenCenter_And_CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "chat":
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Chat, eChatLoc.CL_ChatWindow);
					break;
				case "command": // Command general information, "It is recommender..."
				case "cmdDesc": // Description of command type, "Manually executes a custom code."
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "cmdHeader": // Command type header, "<----- '/account' Commands (plvl 3) ----->"
				case "cmdSyntax": // Command syntax, "/account accountname <characterName>"
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "cmdUsage": // Command description, "Deletes the specified account, along with any associated characters."
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "damageAdd": // Damage add (DA) & damage shield (DS)
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_DamageAdd, eChatLoc.CL_SystemWindow);
					break;
				case "damaged": // You damage target
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Damaged, eChatLoc.CL_SystemWindow);
					break;
				case "debug" when target.Client.Account.PrivLevel > (int)ePrivLevel.Player: // Admin debug
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Staff, eChatLoc.CL_SystemWindow);
					break;
				case "dialogWarn": // Incorrect outputs or problems with command syntax
					target.Client.Out.SendDialogBox(eDialogCode.SimpleWarning, 0, 0, 0, 0, eDialogType.Ok, true, translatedMsg);
					break;
				case "emote":
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Emote, eChatLoc.CL_SystemWindow);
					break;
				case "error": // Incorrect outputs or problems with command syntax
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "expires": // Spell expires
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_SpellExpires, eChatLoc.CL_SystemWindow);
					break;
				case "group":
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Group, eChatLoc.CL_ChatWindow);
					break;
				case "guild":
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Guild, eChatLoc.CL_ChatWindow);
					break;
				case "help": // System help
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
					break;
				case "important":
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "killedByAlb": // Alb killspam
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_KilledByAlb, eChatLoc.CL_SystemWindow);
					break;
				case "killedByHib": // Hib killspam
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_KilledByHib, eChatLoc.CL_SystemWindow);
					break;
				case "killedByMid": // Mid killspam
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_KilledByMid, eChatLoc.CL_SystemWindow);
					break;
				case "lfg": // LFG channel
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_LFG, eChatLoc.CL_ChatWindow);
					break;
				case "loot": // Item drops 
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Loot, eChatLoc.CL_SystemWindow);
					break;
				case "merchant":
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Merchant, eChatLoc.CL_SystemWindow);
					break;
				case "message": // System messages conveying information
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "missed": // Missed combat
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Missed, eChatLoc.CL_SystemWindow);
					break;
				case "officer": // Officer channel
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Officer, eChatLoc.CL_ChatWindow);
					break;
				case "othersCombat": // Other's combat
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_OthersCombat, eChatLoc.CL_SystemWindow);
					break;
				case "playerDied": // Player died
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_PlayerDied, eChatLoc.CL_SystemWindow);
					break;
				case "pulse": // Spell pulse
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_SpellPulse, eChatLoc.CL_SystemWindow);
					break;
				case "resisted": // Spell resisted
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_SpellResisted, eChatLoc.CL_SystemWindow);
					break;
				case "say": // GameLiving say
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Say, eChatLoc.CL_ChatWindow);
					break;
				case "screenCenter": // Center of screen
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_ScreenCenter, eChatLoc.CL_SystemWindow);
					break;
				case "send": // Private messages
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Send, eChatLoc.CL_ChatWindow);
					break;
				case "skill": // Skill
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Skill, eChatLoc.CL_SystemWindow);
					break;
				case "spell": // Spell casting and effects
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
					break;
				case "staff": // Atlas Admin
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Staff, eChatLoc.CL_ChatWindow);
					break;
				case "success": // Important command execution messages
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "system": // Standard system messages
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "team": // Atlas Team channel
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_SystemWindow);
					break;
				case "trade": // Trade channel
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Trade, eChatLoc.CL_ChatWindow);
					break;
				case "yell": // Yelling communication
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
					break;
				case "youDied": // You died messages
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_YouDied, eChatLoc.CL_SystemWindow);
					break;
				case "youHit": // You hit a target messages
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_YouHit, eChatLoc.CL_SystemWindow);
					break;
				case "youWereHit": // GameLiving hit you messages
					target.Client.Out.SendMessage(translatedMsg, eChatType.CT_YouWereHit, eChatLoc.CL_SystemWindow);
					break;
			}
		}

		/// <summary>
		/// Used to send translated messages
		/// </summary>
		/// <param name="type">Determines the eChatType and eChatLoc to use for the message. Options include: "advise", "alliance", "battlegroup", "bgLeader", "broadcast", "centerSys", "chat", "command", "cmdDesc", "cmdHeader","cmdSyntax", "cmdUsage", "damageAdd", "damaged", "debug", "dialog", "emote", "error", "expires", "group", "guild", "help", "important", "killedByAlb", "killedByHib", "killedByMid", "lfg", "loot", "merchant", "missed", "officer", "othersCombat", "playerDied", "pulse", "resisted", "say", "screenCenter", "send", "skill", "spell", "staff", "system", "team", "trade", "yell", "youDied", "youHit", "youWereHit".</param>
		/// <param name="target">The target client receiving the message (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player.Name" (if no args, then use "null")</param>
		/// <note>Please paste the translation ID's associated message (English) into a comment above this method.</note>
		/// <example>ChatUtil.SendTypeMessage("cmdheader", client, "AdminCommands.Account.Description", null);</example>
		/// <comment>To perform message changes, reference the '.txt' files located in 'GameServer > language > EN'. Please remember, if you change the translation ID value, check all other language folders (e.g., DE, FR, ES, etc.) to ensure all translated strings correctly reflect the new ID.</comment>
		public static void SendTypeMessage(string type, GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Account.Language, translationID, args);

			switch (type)
			{
				case "":
					return;
				case "advise": // Advice channel
					target.Out.SendMessage(translatedMsg, eChatType.CT_Advise, eChatLoc.CL_ChatWindow);
					break;
				case "alliance": // Guild alliance
					target.Out.SendMessage(translatedMsg, eChatType.CT_Alliance, eChatLoc.CL_ChatWindow);
					break;
				case "battlegroup":
					target.Out.SendMessage(translatedMsg, eChatType.CT_BattleGroup, eChatLoc.CL_ChatWindow);
					break;
				case "bgLeader": // Battlegroup leader
					target.Out.SendMessage(translatedMsg, eChatType.CT_BattleGroupLeader, eChatLoc.CL_ChatWindow);
					break;
				case "broadcast":
					target.Out.SendMessage(translatedMsg, eChatType.CT_Broadcast, eChatLoc.CL_ChatWindow);
					break;
				case "centerSys": // Center screen and system window
					target.Out.SendMessage(translatedMsg, eChatType.CT_ScreenCenter_And_CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "chat":
					target.Out.SendMessage(translatedMsg, eChatType.CT_Chat, eChatLoc.CL_ChatWindow);
					break;
				case "command": // Command general information, "It is recommender..."
				case "cmdDesc": // Description of command type, "Manually executes a custom code."
					target.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "cmdHeader": // Command type header, "<----- '/account' Commands (plvl 3) ----->"
				case "cmdSyntax": // Command syntax, "/account accountname <characterName>"
					target.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "cmdUsage": // Command description, "Deletes the specified account, along with any associated characters."
					target.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "damageAdd": // Damage add (DA) & damage shield (DS)
					target.Out.SendMessage(translatedMsg, eChatType.CT_DamageAdd, eChatLoc.CL_SystemWindow);
					break;
				case "damaged": // You damage target
					target.Out.SendMessage(translatedMsg, eChatType.CT_Damaged, eChatLoc.CL_SystemWindow);
					break;
				case "debug" when target.Account.PrivLevel > (int)ePrivLevel.Player: // Admin debug
					target.Out.SendMessage(translatedMsg, eChatType.CT_Staff, eChatLoc.CL_SystemWindow);
					break;
				case "dialogWarn": // Incorrect outputs or problems with command syntax
					target.Out.SendDialogBox(eDialogCode.SimpleWarning, 0, 0, 0, 0, eDialogType.Ok, true, translatedMsg);
					break;
				case "emote":
					target.Out.SendMessage(translatedMsg, eChatType.CT_Emote, eChatLoc.CL_SystemWindow);
					break;
				case "error": // Incorrect outputs or problems with command syntax
					target.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "expires": // Spell expires
					target.Out.SendMessage(translatedMsg, eChatType.CT_SpellExpires, eChatLoc.CL_SystemWindow);
					break;
				case "group":
					target.Out.SendMessage(translatedMsg, eChatType.CT_Group, eChatLoc.CL_ChatWindow);
					break;
				case "guild":
					target.Out.SendMessage(translatedMsg, eChatType.CT_Guild, eChatLoc.CL_ChatWindow);
					break;
				case "help": // System help
					target.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
					break;
				case "important":
					target.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "killedByAlb": // Alb killspam
					target.Out.SendMessage(translatedMsg, eChatType.CT_KilledByAlb, eChatLoc.CL_SystemWindow);
					break;
				case "killedByHib": // Hib killspam
					target.Out.SendMessage(translatedMsg, eChatType.CT_KilledByHib, eChatLoc.CL_SystemWindow);
					break;
				case "killedByMid": // Mid killspam
					target.Out.SendMessage(translatedMsg, eChatType.CT_KilledByMid, eChatLoc.CL_SystemWindow);
					break;
				case "lfg": // LFG channel
					target.Out.SendMessage(translatedMsg, eChatType.CT_LFG, eChatLoc.CL_ChatWindow);
					break;
				case "loot": // Item drops 
					target.Out.SendMessage(translatedMsg, eChatType.CT_Loot, eChatLoc.CL_SystemWindow);
					break;
				case "merchant":
					target.Out.SendMessage(translatedMsg, eChatType.CT_Merchant, eChatLoc.CL_SystemWindow);
					break;
				case "message": // System messages conveying information
					target.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "missed": // Missed combat
					target.Out.SendMessage(translatedMsg, eChatType.CT_Missed, eChatLoc.CL_SystemWindow);
					break;
				case "officer": // Officer channel
					target.Out.SendMessage(translatedMsg, eChatType.CT_Officer, eChatLoc.CL_ChatWindow);
					break;
				case "othersCombat": // Other's combat
					target.Out.SendMessage(translatedMsg, eChatType.CT_OthersCombat, eChatLoc.CL_SystemWindow);
					break;
				case "playerDied": // Player died
					target.Out.SendMessage(translatedMsg, eChatType.CT_PlayerDied, eChatLoc.CL_SystemWindow);
					break;
				case "pulse": // Spell pulse
					target.Out.SendMessage(translatedMsg, eChatType.CT_SpellPulse, eChatLoc.CL_SystemWindow);
					break;
				case "resisted": // Spell resisted
					target.Out.SendMessage(translatedMsg, eChatType.CT_SpellResisted, eChatLoc.CL_SystemWindow);
					break;
				case "say": // GameLiving say
					target.Out.SendMessage(translatedMsg, eChatType.CT_Say, eChatLoc.CL_ChatWindow);
					break;
				case "screenCenter": // Center of screen
					target.Out.SendMessage(translatedMsg, eChatType.CT_ScreenCenter, eChatLoc.CL_SystemWindow);
					break;
				case "send": // Private messages
					target.Out.SendMessage(translatedMsg, eChatType.CT_Send, eChatLoc.CL_ChatWindow);
					break;
				case "skill": // Skill
					target.Out.SendMessage(translatedMsg, eChatType.CT_Skill, eChatLoc.CL_SystemWindow);
					break;
				case "spell": // Spell casting and effects
					target.Out.SendMessage(translatedMsg, eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
					break;
				case "staff": // Atlas Admin
					target.Out.SendMessage(translatedMsg, eChatType.CT_Staff, eChatLoc.CL_ChatWindow);
					break;
				case "success": // Important command execution messages
					target.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "system": // Standard system messages
					target.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "team": // Atlas Team channel
					target.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_SystemWindow);
					break;
				case "trade": // Trade channel
					target.Out.SendMessage(translatedMsg, eChatType.CT_Trade, eChatLoc.CL_ChatWindow);
					break;
				case "yell": // Yelling communication
					target.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
					break;
				case "youDied": // You died messages
					target.Out.SendMessage(translatedMsg, eChatType.CT_YouDied, eChatLoc.CL_SystemWindow);
					break;
				case "youHit": // You hit a target messages
					target.Out.SendMessage(translatedMsg, eChatType.CT_YouHit, eChatLoc.CL_SystemWindow);
					break;
				case "youWereHit": // GameLiving hit you messages
					target.Out.SendMessage(translatedMsg, eChatType.CT_YouWereHit, eChatLoc.CL_SystemWindow);
					break;
			}
		}
		
		/// <summary>
		/// Used to send translated messages
		/// </summary>
		/// <param name="type">Determines the eChatType and eChatLoc to use for the message. Options include: "advise", "alliance", "battlegroup", "bgLeader", "broadcast", "centerSys", "chat", "command", "cmdDesc", "cmdHeader","cmdSyntax", "cmdUsage", "damageAdd", "damaged", "debug", "dialog", "emote", "error", "expires", "group", "guild", "help", "important", "killedByAlb", "killedByHib", "killedByMid", "lfg", "loot", "merchant", "missed", "officer", "othersCombat", "playerDied", "pulse", "resisted", "say", "screenCenter", "send", "skill", "spell", "staff", "system", "team", "trade", "yell", "youDied", "youHit", "youWereHit".</param>
		/// <param name="target">The target client receiving the message (e.g., "player")</param>
		/// <param name="message">The string message (e.g., "You died!")</param>
		/// <returns>The identified string message</returns>
		/// <example>ChatUtil.SendTypeMessage("system", client, "This is a message.");</example>
		public static void SendTypeMessage(string type, GamePlayer target, string message)
		{
			switch (type)
			{
				case "":
					return;
				case "advise": // Advice channel
					target.Client.Out.SendMessage(message, eChatType.CT_Advise, eChatLoc.CL_ChatWindow);
					break;
				case "alliance": // Guild alliance
					target.Client.Out.SendMessage(message, eChatType.CT_Alliance, eChatLoc.CL_ChatWindow);
					break;
				case "battlegroup":
					target.Client.Out.SendMessage(message, eChatType.CT_BattleGroup, eChatLoc.CL_ChatWindow);
					break;
				case "bgLeader": // Battlegroup leader
					target.Client.Out.SendMessage(message, eChatType.CT_BattleGroupLeader, eChatLoc.CL_ChatWindow);
					break;
				case "broadcast":
					target.Client.Out.SendMessage(message, eChatType.CT_Broadcast, eChatLoc.CL_ChatWindow);
					break;
				case "centerSys": // Center screen and system window
					target.Client.Out.SendMessage(message, eChatType.CT_ScreenCenter_And_CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "chat":
					target.Client.Out.SendMessage(message, eChatType.CT_Chat, eChatLoc.CL_ChatWindow);
					break;
				case "command": // Command general information, "It is recommender..."
				case "cmdDesc": // Description of command type, "Manually executes a custom code."
					target.Client.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "cmdHeader": // Command type header, "<----- '/account' Commands (plvl 3) ----->"
				case "cmdSyntax": // Command syntax, "/account accountname <characterName>"
					target.Client.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "cmdUsage": // Command description, "Deletes the specified account, along with any associated characters."
					target.Client.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "damageAdd": // Damage add (DA) & damage shield (DS)
					target.Client.Out.SendMessage(message, eChatType.CT_DamageAdd, eChatLoc.CL_SystemWindow);
					break;
				case "damaged": // You damage target
					target.Client.Out.SendMessage(message, eChatType.CT_Damaged, eChatLoc.CL_SystemWindow);
					break;
				case "debug" when target.Client.Account.PrivLevel > (int)ePrivLevel.Player: // Admin debug
					target.Client.Out.SendMessage(message, eChatType.CT_Staff, eChatLoc.CL_SystemWindow);
					break;
				case "dialogWarn": // Incorrect outputs or problems with command syntax
					target.Client.Out.SendDialogBox(eDialogCode.SimpleWarning, 0, 0, 0, 0, eDialogType.Ok, true, message);
					break;
				case "emote":
					target.Client.Out.SendMessage(message, eChatType.CT_Emote, eChatLoc.CL_SystemWindow);
					break;
				case "error": // Incorrect outputs or problems with command syntax
					target.Client.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "expires": // Spell expires
					target.Client.Out.SendMessage(message, eChatType.CT_SpellExpires, eChatLoc.CL_SystemWindow);
					break;
				case "group":
					target.Client.Out.SendMessage(message, eChatType.CT_Group, eChatLoc.CL_ChatWindow);
					break;
				case "guild":
					target.Client.Out.SendMessage(message, eChatType.CT_Guild, eChatLoc.CL_ChatWindow);
					break;
				case "help": // System help
					target.Client.Out.SendMessage(message, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
					break;
				case "important":
					target.Client.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "killedByAlb": // Alb killspam
					target.Client.Out.SendMessage(message, eChatType.CT_KilledByAlb, eChatLoc.CL_SystemWindow);
					break;
				case "killedByHib": // Hib killspam
					target.Client.Out.SendMessage(message, eChatType.CT_KilledByHib, eChatLoc.CL_SystemWindow);
					break;
				case "killedByMid": // Mid killspam
					target.Client.Out.SendMessage(message, eChatType.CT_KilledByMid, eChatLoc.CL_SystemWindow);
					break;
				case "lfg": // LFG channel
					target.Client.Out.SendMessage(message, eChatType.CT_LFG, eChatLoc.CL_ChatWindow);
					break;
				case "loot": // Item drops 
					target.Client.Out.SendMessage(message, eChatType.CT_Loot, eChatLoc.CL_SystemWindow);
					break;
				case "merchant":
					target.Client.Out.SendMessage(message, eChatType.CT_Merchant, eChatLoc.CL_SystemWindow);
					break;
				case "message": // System messages conveying information
					target.Client.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "missed": // Missed combat
					target.Client.Out.SendMessage(message, eChatType.CT_Missed, eChatLoc.CL_SystemWindow);
					break;
				case "officer": // Officer channel
					target.Client.Out.SendMessage(message, eChatType.CT_Officer, eChatLoc.CL_ChatWindow);
					break;
				case "othersCombat": // Other's combat
					target.Client.Out.SendMessage(message, eChatType.CT_OthersCombat, eChatLoc.CL_SystemWindow);
					break;
				case "playerDied": // Player died
					target.Client.Out.SendMessage(message, eChatType.CT_PlayerDied, eChatLoc.CL_SystemWindow);
					break;
				case "pulse": // Spell pulse
					target.Client.Out.SendMessage(message, eChatType.CT_SpellPulse, eChatLoc.CL_SystemWindow);
					break;
				case "resisted": // Spell resisted
					target.Client.Out.SendMessage(message, eChatType.CT_SpellResisted, eChatLoc.CL_SystemWindow);
					break;
				case "say": // GameLiving say
					target.Client.Out.SendMessage(message, eChatType.CT_Say, eChatLoc.CL_ChatWindow);
					break;
				case "screenCenter": // Center of screen
					target.Client.Out.SendMessage(message, eChatType.CT_ScreenCenter, eChatLoc.CL_SystemWindow);
					break;
				case "send": // Private messages
					target.Client.Out.SendMessage(message, eChatType.CT_Send, eChatLoc.CL_ChatWindow);
					break;
				case "skill": // Skill
					target.Client.Out.SendMessage(message, eChatType.CT_Skill, eChatLoc.CL_SystemWindow);
					break;
				case "spell": // Spell casting and effects
					target.Client.Out.SendMessage(message, eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
					break;
				case "staff": // Atlas Admin
					target.Client.Out.SendMessage(message, eChatType.CT_Staff, eChatLoc.CL_ChatWindow);
					break;
				case "success": // Important command execution messages
					target.Client.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "system": // Standard system messages
					target.Client.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "team": // Atlas Team channel
					target.Client.Out.SendMessage(message, eChatType.CT_Help, eChatLoc.CL_SystemWindow);
					break;
				case "trade": // Trade channel
					target.Client.Out.SendMessage(message, eChatType.CT_Trade, eChatLoc.CL_ChatWindow);
					break;
				case "yell": // Yelling communication
					target.Client.Out.SendMessage(message, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
					break;
				case "youDied": // You died messages
					target.Client.Out.SendMessage(message, eChatType.CT_YouDied, eChatLoc.CL_SystemWindow);
					break;
				case "youHit": // You hit a target messages
					target.Client.Out.SendMessage(message, eChatType.CT_YouHit, eChatLoc.CL_SystemWindow);
					break;
				case "youWereHit": // GameLiving hit you messages
					target.Client.Out.SendMessage(message, eChatType.CT_YouWereHit, eChatLoc.CL_SystemWindow);
					break;
			}
		}
		
		/// <summary>
		/// Used to send translated messages
		/// </summary>
		/// <param name="type">Determines the eChatType and eChatLoc to use for the message. Options include: "advise", "alliance", "battlegroup", "bgLeader", "broadcast", "centerSys", "chat", "command", "cmdDesc", "cmdHeader","cmdSyntax", "cmdUsage", "damageAdd", "damaged", "debug", "dialog", "emote", "error", "expires", "group", "guild", "help", "important", "killedByAlb", "killedByHib", "killedByMid", "lfg", "loot", "merchant", "missed", "officer", "othersCombat", "playerDied", "pulse", "resisted", "say", "screenCenter", "send", "skill", "spell", "staff", "system", "team", "trade", "yell", "youDied", "youHit", "youWereHit".</param>
		/// <param name="target">The target client receiving the message (e.g., "client")</param>
		/// <param name="message">The string message (e.g., "You died!")</param>
		/// <returns>The identified string message</returns>
		/// <example>ChatUtil.SendTypeMessage("system", client, "This is a message.");</example>
		public static void SendTypeMessage(string type, GameClient target, string message)
		{
			switch (type)
			{
				case "":
					return;
				case "advise": // Advice channel
					target.Out.SendMessage(message, eChatType.CT_Advise, eChatLoc.CL_ChatWindow);
					break;
				case "alliance": // Guild alliance
					target.Out.SendMessage(message, eChatType.CT_Alliance, eChatLoc.CL_ChatWindow);
					break;
				case "battlegroup":
					target.Out.SendMessage(message, eChatType.CT_BattleGroup, eChatLoc.CL_ChatWindow);
					break;
				case "bgLeader": // Battlegroup leader
					target.Out.SendMessage(message, eChatType.CT_BattleGroupLeader, eChatLoc.CL_ChatWindow);
					break;
				case "broadcast":
					target.Out.SendMessage(message, eChatType.CT_Broadcast, eChatLoc.CL_ChatWindow);
					break;
				case "centerSys": // Center screen and system window
					target.Out.SendMessage(message, eChatType.CT_ScreenCenter_And_CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "chat":
					target.Out.SendMessage(message, eChatType.CT_Chat, eChatLoc.CL_ChatWindow);
					break;
				case "command": // Command general information, "It is recommender..."
				case "cmdDesc": // Description of command type, "Manually executes a custom code."
					target.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "cmdHeader": // Command type header, "<----- '/account' Commands (plvl 3) ----->"
				case "cmdSyntax": // Command syntax, "/account accountname <characterName>"
					target.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "cmdUsage": // Command description, "Deletes the specified account, along with any associated characters."
					target.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "damageAdd": // Damage add (DA) & damage shield (DS)
					target.Out.SendMessage(message, eChatType.CT_DamageAdd, eChatLoc.CL_SystemWindow);
					break;
				case "damaged": // You damage target
					target.Out.SendMessage(message, eChatType.CT_Damaged, eChatLoc.CL_SystemWindow);
					break;
				case "debug" when target.Account.PrivLevel > (int)ePrivLevel.Player: // Admin debug
					target.Out.SendMessage(message, eChatType.CT_Staff, eChatLoc.CL_SystemWindow);
					break;
				case "dialogWarn": // Incorrect outputs or problems with command syntax
					target.Out.SendDialogBox(eDialogCode.SimpleWarning, 0, 0, 0, 0, eDialogType.Ok, true, message);
					break;
				case "emote":
					target.Out.SendMessage(message, eChatType.CT_Emote, eChatLoc.CL_SystemWindow);
					break;
				case "error": // Incorrect outputs or problems with command syntax
					target.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "expires": // Spell expires
					target.Out.SendMessage(message, eChatType.CT_SpellExpires, eChatLoc.CL_SystemWindow);
					break;
				case "group":
					target.Out.SendMessage(message, eChatType.CT_Group, eChatLoc.CL_ChatWindow);
					break;
				case "guild":
					target.Out.SendMessage(message, eChatType.CT_Guild, eChatLoc.CL_ChatWindow);
					break;
				case "help": // System help
					target.Out.SendMessage(message, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
					break;
				case "important":
					target.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "killedByAlb": // Alb killspam
					target.Out.SendMessage(message, eChatType.CT_KilledByAlb, eChatLoc.CL_SystemWindow);
					break;
				case "killedByHib": // Hib killspam
					target.Out.SendMessage(message, eChatType.CT_KilledByHib, eChatLoc.CL_SystemWindow);
					break;
				case "killedByMid": // Mid killspam
					target.Out.SendMessage(message, eChatType.CT_KilledByMid, eChatLoc.CL_SystemWindow);
					break;
				case "lfg": // LFG channel
					target.Out.SendMessage(message, eChatType.CT_LFG, eChatLoc.CL_ChatWindow);
					break;
				case "loot": // Item drops 
					target.Out.SendMessage(message, eChatType.CT_Loot, eChatLoc.CL_SystemWindow);
					break;
				case "merchant":
					target.Out.SendMessage(message, eChatType.CT_Merchant, eChatLoc.CL_SystemWindow);
					break;
				case "message": // System messages conveying information
					target.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "missed": // Missed combat
					target.Out.SendMessage(message, eChatType.CT_Missed, eChatLoc.CL_SystemWindow);
					break;
				case "officer": // Officer channel
					target.Out.SendMessage(message, eChatType.CT_Officer, eChatLoc.CL_ChatWindow);
					break;
				case "othersCombat": // Other's combat
					target.Out.SendMessage(message, eChatType.CT_OthersCombat, eChatLoc.CL_SystemWindow);
					break;
				case "playerDied": // Player died
					target.Out.SendMessage(message, eChatType.CT_PlayerDied, eChatLoc.CL_SystemWindow);
					break;
				case "pulse": // Spell pulse
					target.Out.SendMessage(message, eChatType.CT_SpellPulse, eChatLoc.CL_SystemWindow);
					break;
				case "resisted": // Spell resisted
					target.Out.SendMessage(message, eChatType.CT_SpellResisted, eChatLoc.CL_SystemWindow);
					break;
				case "say": // GameLiving say
					target.Out.SendMessage(message, eChatType.CT_Say, eChatLoc.CL_ChatWindow);
					break;
				case "screenCenter": // Center of screen
					target.Out.SendMessage(message, eChatType.CT_ScreenCenter, eChatLoc.CL_SystemWindow);
					break;
				case "send": // Private messages
					target.Out.SendMessage(message, eChatType.CT_Send, eChatLoc.CL_ChatWindow);
					break;
				case "skill": // Skill
					target.Out.SendMessage(message, eChatType.CT_Skill, eChatLoc.CL_SystemWindow);
					break;
				case "spell": // Spell casting and effects
					target.Out.SendMessage(message, eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
					break;
				case "staff": // Atlas Admin
					target.Out.SendMessage(message, eChatType.CT_Staff, eChatLoc.CL_ChatWindow);
					break;
				case "success": // Important command execution messages
					target.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
					break;
				case "system": // Standard system messages
					target.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
					break;
				case "team": // Atlas Team channel
					target.Out.SendMessage(message, eChatType.CT_Help, eChatLoc.CL_SystemWindow);
					break;
				case "trade": // Trade channel
					target.Out.SendMessage(message, eChatType.CT_Trade, eChatLoc.CL_ChatWindow);
					break;
				case "yell": // Yelling communication
					target.Out.SendMessage(message, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
					break;
				case "youDied": // You died messages
					target.Out.SendMessage(message, eChatType.CT_YouDied, eChatLoc.CL_SystemWindow);
					break;
				case "youHit": // You hit a target messages
					target.Out.SendMessage(message, eChatType.CT_YouHit, eChatLoc.CL_SystemWindow);
					break;
				case "youWereHit": // GameLiving hit you messages
					target.Out.SendMessage(message, eChatType.CT_YouWereHit, eChatLoc.CL_SystemWindow);
					break;
			}
		}
		
		/// <summary>
		/// Used to send translated messages contained in a text window
		/// </summary>
		/// /// <param name="target">The player triggering/receiving the message (i.e., typically "client").</param>
		/// /// <param name="title">The string to appear along the top border of the text window.</param>
		/// <param name="args">Additional translation IDs or strings to include in the body ot the text window.</param>
		/// <note>To include empty spaces between paragraphs, input a space between apostrophies (e.g., " ").</note>
		public static void SendWindowMessage(GameClient target, string title, params string[] args)
		{
			var info = new List<string>();
			foreach (var translation in args)
				info.Add(LanguageMgr.GetTranslation(target.Account.Language, translation));
			
			target.Out.SendCustomTextWindow(title, info);
		}
		
		/// <summary>
		/// Used to send translated messages contained in a text window
		/// </summary>
		/// /// <param name="target">The player triggering/receiving the message (i.e., typically "client").</param>
		/// /// <param name="title">The string to appear along the top border of the text window.</param>
		/// <param name="args">Additional translation IDs or strings to include in the body ot the text window.</param>
		/// <note>To include empty spaces between paragraphs, input a space between apostrophies (e.g., " ").</note>
		public static void SendWindowMessage(GamePlayer target, string title, params string[] args)
		{
			var info = new List<string>();
			foreach (var translation in args)
				info.Add(LanguageMgr.GetTranslation(target.Client.Account.Language, translation));
			
			target.Client.Out.SendCustomTextWindow(title, info);
		}

		public static void SendSystemMessage(GamePlayer target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
		}

		public static void SendSystemMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
		}

		public static void SendSystemMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
		}

		public static void SendSystemMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
		}

		public static void SendMerchantMessage(GamePlayer target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Merchant, eChatLoc.CL_SystemWindow);
		}

		public static void SendMerchantMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_Merchant, eChatLoc.CL_SystemWindow);
		}

		public static void SendMerchantMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Merchant, eChatLoc.CL_SystemWindow);
		}

		public static void SendMerchantMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_Merchant, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send translated messages to a player, which displays as a dialog (pop-up) window.
		/// </summary>
		/// <param name="target">The player triggering/receiving the message (i.e., typically "client").</param>
		/// <param name="translationID">The translation string associated with the message (e.g., "Scripts.Blacksmith.Say").</param>
		/// <param name="args">Any arguments to include in the message in place of placeholders like "{0}", or else "null".</param>
		public static void SendDialogMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_PopupWindow);
		}

		/// <summary>
		/// Used to send translated messages to a player, which displays as a "/say" message in the chat window.
		/// </summary>
		/// <param name="target">The player triggering/receiving the message (i.e., typically "client").</param>
		/// <param name="translationID">The translation string associated with the message (e.g., "Scripts.Blacksmith.Say").</param>
		/// <param name="args">Any arguments to include in the message in place of placeholders like "{0}", or else "null".</param>
		public static void SendSayMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_Say, eChatLoc.CL_ChatWindow);
		}
		
		/// <summary>
		/// Used to send translated messages to a player, which displays as a "/say" message in the chat window.
		/// </summary>
		/// <param name="target">The player triggering/receiving the message (i.e., typically "client").</param>
		/// <param name="translationID">The translation string associated with the message (e.g., "Scripts.Blacksmith.Say").</param>
		/// <param name="args">Any arguments to include in the message in place of placeholders like "{0}", or else "null".</param>
		public static void SendSayMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_Say, eChatLoc.CL_ChatWindow);
		}
		
		/// <summary>
		/// Used to send translated messages containing slash command descriptions and related information
		/// </summary>
		/// <param name="target">The player triggering/receiving the message (typically "client")</param>
		/// <param name="translationID">The translation string associated with the message (e.g., "AdminCommands.Account.Usage.Create")</param>
		/// <param name="args">Any arguments to include in the message in place of values like "{0}" (or else use "null")</param>
		public static void SendCommMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_System, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send in-line messages containing slash command descriptions and related information
		/// </summary>
		/// <param name="target">The player triggering/receiving the message (typically "client")</param>
		/// <param name="message">The message itself (translation IDs recommended instead)</param>
		public static void SendCommMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_System, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send translated messages containing slash command syntax (e.g., /account accountname)
		/// </summary>
		/// <param name="target">The player triggering/receiving the command type list (typically "client")</param>
		/// <param name="translationID">The translation string associated with the message (e.g., "AdminCommands.Account.Syntax.Create")</param>
		/// <param name="args">Any arguments to include in the message in place of values like "{0}" (or else use "null")</param>
		public static void SendSyntaxMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send in-line messages containing slash command syntax (e.g., '/account accountname')
		/// </summary>
		/// <param name="target">The player triggering/receiving the command type list (typically "client")</param>
		/// <param name="message">The message itself (translation IDs recommended instead)</param>
		public static void SendSyntaxMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send translated header/separator at head of command list (e.g., ----- '/account' Commands (plvl 3) -----)
		/// </summary>
		/// <param name="target">The player triggering/receiving the command type list (typically "client")</param>
		/// <param name="translationID">The translation string associated with the message (e.g., "AdminCommands.Header.Syntax.Account")</param>
		/// <param name="args">Any arguments to include in the message (typically "null")</param>
		public static void SendHeaderMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
		}

		/// <summary>
		/// Used to send in-line header/separator at head of command list (e.g., ----- '/account' Commands (plvl 3) -----)
		/// </summary>
		/// <param name="target">The player triggering/receiving the command type list (typically "client")</param>
		/// <param name="message">The message itself (translation IDs recommended instead)</param>
		public static void SendHeaderMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
		}
		
		public static void SendHelpMessage(GamePlayer target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
		}

		/// <summary>
		/// Used to send translated help/alert messages
		/// </summary>
		/// <param name="target">The client receiving the help/alert message (e.g., "player.Client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "target.Client" (if no args, then use "null")</param>
		public static void SendHelpMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
		}

		public static void SendHelpMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
		}

		/// <summary>
		/// Used to send translated help/alert messages
		/// </summary>
		/// <param name="target">The player receiving the help/alert message (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player" (if no args, then use "null")</param>
		public static void SendHelpMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_ChatWindow);
		}
		
		/// <summary>
		/// Used to send translated error/alert messages
		/// </summary>
		/// <param name="target">The client receiving the error/alert (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player" (if no args, then use "null")</param>
		public static void SendErrorMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send translated error/alert messages
		/// </summary>
		/// <param name="target">The player client receiving the error/alert (e.g., "player.Client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player" (if no args, then use "null")</param>
		public static void SendErrorMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
		}

		public static void SendErrorMessage(GamePlayer target, string message)
		{
			SendErrorMessage(target.Client, message);
		}

		public static void SendErrorMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Important, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send translated '/send' messages
		/// </summary>
		/// <param name="target">The client receiving the message (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player.Name" (if no args, then use "null")</param>
		public static void SendSendMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Send, eChatLoc.CL_ChatWindow);
		}
		
		/// <summary>
		/// Used to send translated '/send' messages
		/// </summary>
		/// <param name="target">The player client receiving the message (e.g., "player.Client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player" (if no args, then use "null")</param>
		public static void SendSendMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Send, eChatLoc.CL_ChatWindow);
		}

		/// <summary>
		/// Used to send translated '/send' messages
		/// </summary>
		/// <param name="target">The player client receiving the message (e.g., "player.Client")</param>
		/// <param name="message">The message string (e.g., "This is a message.")</param>
		public static void SendSendMessage(GamePlayer target, string message)
		{
			SendSendMessage(target.Client, message);
		}

		/// <summary>
		/// Used to send translated '/send' messages
		/// </summary>
		/// <param name="target">The player client receiving the message (e.g., "client")</param>
		/// <param name="message">The message string (e.g., "This is a message.")</param>
		public static void SendSendMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Send, eChatLoc.CL_ChatWindow);
		}
		
		/// <summary>
		/// Used to send translated '/adv' messages
		/// </summary>
		/// <param name="target">The client receiving the message (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player.Name" (if no args, then use "null")</param>
		public static void SendAdviceMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Advise, eChatLoc.CL_ChatWindow);
		}
		
		/// <summary>
		/// Used to send translated '/adv' messages
		/// </summary>
		/// <param name="target">The player client receiving the message (e.g., "player.Client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player" (if no args, then use "null")</param>
		public static void SendAdviceMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Advise, eChatLoc.CL_ChatWindow);
		}

		/// <summary>
		/// Used to send translated '/adv' messages
		/// </summary>
		/// <param name="target">The player client receiving the message (e.g., "player.Client")</param>
		/// <param name="message">The message string (e.g., "This is a message.")</param>
		public static void SendAdviceMessage(GamePlayer target, string message)
		{
			SendAdviceMessage(target.Client, message);
		}

		/// <summary>
		/// Used to send translated '/adv' messages
		/// </summary>
		/// <param name="target">The player client receiving the message (e.g., "client")</param>
		/// <param name="message">The message string (e.g., "This is a message.")</param>
		public static void SendAdviceMessage(GameClient target, string message)
		{
			target.Out.SendMessage(message, eChatType.CT_Advise, eChatLoc.CL_ChatWindow);
		}
		
		/// <summary>
		/// Used to send translated staff messages
		/// </summary>
		/// <param name="target">The client receiving the message (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player.Name" (if no args, then use "null")</param>
		public static void SendGMMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Staff, eChatLoc.CL_ChatWindow);
		}
		
		/// <summary>
		/// Used to send translated team messages
		/// </summary>
		/// <param name="target">The client receiving the message (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player.Name" (if no args, then use "null")</param>
		public static void SendTeamMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Help, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send translated staff messages
		/// </summary>
		/// <param name="target">The player client receiving the message (e.g., "player.Client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player" (if no args, then use "null")</param>
		public static void SendGMMessage(GamePlayer target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target.Client, translationID, args);
			
			target.Out.SendMessage(translatedMsg, eChatType.CT_Staff, eChatLoc.CL_ChatWindow);
		}

		public static void SendDebugMessage(GamePlayer target, string message)
		{
			SendDebugMessage(target.Client, message);
		}

		public static void SendDebugMessage(GameClient target, string message)
		{
			if (target.Account.PrivLevel > (int)ePrivLevel.Player)
				target.Out.SendMessage(message, eChatType.CT_Staff, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send translated debug messages
		/// </summary>
		/// <param name="target">The player receiving the debug message (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player" (if no args, then use "null")</param>
		public static void SendDebugMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);
			
			if (target.Account.PrivLevel > (int)ePrivLevel.Player)
				target.Out.SendMessage(translatedMsg, eChatType.CT_Staff, eChatLoc.CL_SystemWindow);
		}
		
		/// <summary>
		/// Used to send translated messages to all clients on a server
		/// </summary>
		/// <param name="target">The player receiving the error/alert (e.g., "client")</param>
		/// <param name="translationID">The translation ID for the message (e.g., "AdminCommands.Command.Err.NoPlayerFound")</param>
		/// <param name="args">Any argument values to include in the message, such as "client.Player" (if no args, then use "null")</param>
		public static void SendServerMessage(GameClient target, string translationID, params object[] args)
		{
			var translatedMsg = LanguageMgr.GetTranslation(target, translationID, args);

			target.Out.SendMessage(translatedMsg, eChatType.CT_Staff, eChatLoc.CL_ChatWindow);
		}
	}
}