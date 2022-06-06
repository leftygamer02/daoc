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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using DOL.AI.Brain;
using DOL.Database;
using DOL.Events;
using DOL.GS.PacketHandler;
using DOL.GS.PacketHandler.Client.v168;
using DOL.Language;
using DOL.GS.ServerProperties;
using static DOL.GS.PropertyCollection;

namespace DOL.GS.Commands
{
	// See the comments above 'using' about SendMessage translation IDs
	[CmdAttribute(
		// Enter '/tester' to list all commands of this type
		"&tester",
		// Message: <----- '/tester' Commands (TESTER ONLY) ----->
		"PLCommands.Header.Syntax.Tester",
		ePrivLevel.Player,
		// Message: Allows testers to perform expanded research and information gathering while on the Atlas test server.
		"PLCommands.Tester.Description",
		// Syntax: /tester speed
		"PLCommands.Tester.Syntax.Speed",
		// Message: Substantially increases your movement speed. Use again to return your speed to normal.
		"PLCommands.Tester.Usage.Speed",
		// Syntax: /tester heal
		"PLCommands.Tester.Syntax.Heal",
		// Message: Fully replenishes your HP, endurance, and power.
		"PLCommands.Tester.Usage.Heal",
		// Syntax: /tester godmode on|off
		"PLCommands.Tester.Syntax.GodMode",
		// Message: Prevents mobs from engaging you in combat, but you may still attack them. Does not work against other players or realm-specific mobs/NPCs.
		"PLCommands.Tester.Usage.GodMode",
		// Syntax: /tester level <newLevel>
		"PLCommands.Tester.Syntax.Level",
		// Message: Changes your level to the specified value. All skill lines are reset upon changing levels.
		"PLCommands.Tester.Usage.Level",
		// Syntax: /tester mobloc
		"PLCommands.Tester.Syntax.MobLoc",
		// Message: Inserts a targeted mob's physical location onto your chat/combat window.
		"PLCommands.Tester.Usage.MobLoc",
		// Syntax: /tester info
		"PLCommands.Tester.Syntax.Info",
		// Message: Provides detailed information about a targeted mob.
		"PLCommands.Tester.Usage.Info"
		)]
	public class TesterCommand : AbstractCommandHandler, ICommandHandler
	{
		public void OnCommand(GameClient client, string[] args)
		{
			if (client == null)
				return;

			var player = client.Player;
			var modifier = Util.Random(1, 10);

			if (player == null)
				return;
			
			if (!player.Client.Account.IsTester)
				return;

			if (!Properties.TESTER_LOGIN)
				return;

			if (args.Length < 2)
			{
				// Lists all '/tester' type subcommand syntax
				DisplaySyntax(client);

				/*
				// Lists the '/account create' command's full syntax
				// Message: "<----- '/account' Commands (plvl 3) ----->"
				ChatUtil.SendSyntaxMessage(client, "AdminCommands.Header.Syntax.Account", null);
				// Message: "Use the following syntax for this command:"
				ChatUtil.SendCommMessage(client, "AdminCommands.Command.SyntaxDesc", null);
				// Message: "/account create <accountName> <password>"
				ChatUtil.SendSyntaxMessage(client, "AdminCommands.Account.Syntax.Create", null);
				// Message: "Creates a new account with the specified login credentials."
				ChatUtil.SendCommMessage(client, "AdminCommands.Account.Usage.Create", null); */
				return;
			}

			if (player.TesterModifier == 0)
				player.TesterModifier = modifier;

			switch (args[1].ToLower())
			{
				#region Speed
				case "speed":
				{

					var testerSpeed = player.TempProperties.getProperty("tester_speed", false);

					if (!testerSpeed)
					{
						player.TempProperties.setProperty("tester_speed", true);
						player.MaxSpeedBase = 750;
						ChatUtil.SendErrorMessage(player, "You move significantly faster.");
					}
					else
					{	
						player.TempProperties.setProperty("tester_speed", false);
						player.MaxSpeedBase = 191; // Default speed
						ChatUtil.SendErrorMessage(player, "You resume moving at your normal speed.");
					}
					
					player.Out.SendUpdateMaxSpeed();
				}
					break;
				#endregion Speed

				#region Heal
				case "heal":
				{
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

					ChatUtil.SendErrorMessage(player, "You heal yourself.");
					player.Out.SendUpdatePlayer();
				}
					break;
				#endregion Heal

				#region Invuln
				case "godmode":
				{
					if (args[2] == "on")
					{
						player.TempProperties.setProperty("tester_godMode", true);
						ChatUtil.SendErrorMessage(player, "The power of Atlas protects you from harm!");
					}
					if (args[2] == "off")
					{
						player.TempProperties.setProperty("tester_godMode", false);
						ChatUtil.SendErrorMessage(player, "Atlas' protection withdraws from you!");
					}

					player.Out.SendUpdatePlayer();
					player.RefreshWorld();
					// Refresh equipment for player so they don't appear naked after changing plvl
					player.UpdateEquipmentAppearance();
				}
					break;
				case "level":
				{
					if (args.Length < 2)
					{
						ChatUtil.SendErrorMessage(player, "You must specify a level with this command. Try '/tester level <newLevel>'.");
						return;
					}

					var level = player.Level;
					var newLevel = Convert.ToByte(args[2]);

					if (newLevel is <= 0 or > 50)
	                {
		                ChatUtil.SendErrorMessage(player, "You may only specify a value between 1 and 50!");
	                    return;
	                }

					if (newLevel <= level)
					{
						player.Reset();
						if (level > 1)
							player.GainExperience(eXPSource.Other, player.GetExperienceValueForLevel(++level));
						player.Level = newLevel;
					}

	                if (newLevel > level)
	                {
	                    var curSecondStage = player.IsLevelSecondStage;

	                    if (newLevel > level && curSecondStage)
		                    player.GainExperience(eXPSource.Other, player.GetExperienceValueForLevel(++level));

	                    if (newLevel != level || !curSecondStage)
	                        player.Level = newLevel;

	                    // If new level is more than 40, then we have
	                    // to add the skill points from half-levels
	                    if (newLevel > 40)
	                    {
	                        if (level < 40)
		                        level = 40;
	                        for (int i = level; i < newLevel; i++)
	                        {
	                            // we skip the first add if was in level 2nd stage
	                            if (curSecondStage)
	                                curSecondStage = false;
	                        }
	                    }
	                }

	                ChatUtil.SendErrorMessage(player, "You changed your level successfully to " + newLevel + "!");

	                player.Out.SendUpdatePlayer();
	                player.Out.SendUpdatePoints();
	                player.Out.SendCharStatsUpdate();
	                player.UpdatePlayerStatus();
	                player.SaveIntoDatabase();
				}
					break;
				case "mobloc":
				{
					var mob = client.Player.TargetObject as GameNPC;

					if (mob == null)
					{
						ChatUtil.SendErrorMessage(player, "You must target a mob or NPC to use this command!");
						return;
					}

					if (mob?.Realm == 0)
						ChatUtil.SendSystemMessage(player, mob.Name + "-- Loc: " +
						                                   mob.CurrentZone.Description + ", " +
						                                   mob.X + ", " +
						                                   mob.Y + ", " +
						                                   mob.Z + ", Heading: " +
						                                   mob.Heading);
				}
					break;
				case "info":
				{
					var target = client.Player.TargetObject;

					if (target == null)
					{
						ChatUtil.SendErrorMessage(player, "You must target a mob to use this command!");
						return;
					}
					else
					{
						//if (client.Player.TargetObject is GameNPC)
							target = client.Player.TargetObject as GameNPC;
						//else if (client.Player.TargetObject is GameDoor)
						//	target = (GameDoor)client.Player.TargetObject;
						//else if (client.Player.TargetObject is GamePlayer)
						//	target = (GamePlayer)client.Player.TargetObject;
						//else if (client.Player.TargetObject is GameStaticItem)
						//	target = (GameStaticItem)client.Player.TargetObject;
					}

					var info = new List<string>();

					info.Add("---------- ATTENTION ----------");
					info.Add ("The following information is made available solely for gathering test data and is not intended for distribution to non-tester players. Some original values have been masked or presented as ranges for security purposes.");
					info.Add(" ");

					if (target != null)
					{
						var targetMob = target as GameNPC;

						info.Add("Tester Information for: " + targetMob.Name);

						info.Add(" ");
						if (targetMob is GameEpicNPC)
							info.Add("Type: Epic Mob");
						else if (targetMob.ScalingFactor > 15)
							info.Add("Type: Epic Boss");
						else
							info.Add("Type: Standard Mob");
						info.Add("HP: " + targetMob.HealthPercent + "%");

						info.Add(" ");
						info.Add("General");
						info.Add(" - Realm: " + GlobalConstants.RealmToName(targetMob.Realm));

						if (!string.IsNullOrEmpty(targetMob.GuildName))
							info.Add(" - Guild: " + targetMob.GuildName);
						info.Add(" - Level: " + targetMob.Level);
						if (targetMob.MaxSpeedBase > player.MaxSpeedBase)
							info.Add(" - Speed: Faster than you");
						else
							info.Add(" - Speed: Slower than you");

						var bodyType = "None";
						switch (targetMob.BodyType)
						{
							case 1:
								bodyType = "Animal";
								break;
							case 2:
								bodyType = "Demon";
								break;
							case 3:
								bodyType = "Dragon";
								break;
							case 4:
								bodyType = "Elemental";
								break;
							case 5:
								bodyType = "Giant";
								break;
							case 6:
								bodyType = "Humanoid";
								break;
							case 7:
								bodyType = "Insect";
								break;
							case 8:
								bodyType = "Magical";
								break;
							case 9:
								bodyType = "Reptile";
								break;
							case 10:
								bodyType = "Plant";
								break;
							case 11:
								bodyType = "Undead";
								break;
						}
						info.Add(" - Body Type: " + bodyType);

						info.Add(" ");
						info.Add("Templates");
						if (targetMob.NPCTemplate != null && targetMob.NPCTemplate.ReplaceMobValues)
							info.Add(" - NPCTemplate Active: YES");
						else
							info.Add(" - NPCTemplate Active: NO");
						if (targetMob.EquipmentTemplateID != null && targetMob.EquipmentTemplateID.Length > 0 && targetMob.Inventory != null)
							info.Add(" - Inventory: Equipment assigned");
						else
							info.Add(" - Inventory: No equipment found");

						info.Add(" ");
						info.Add("Aggro");
						if (targetMob.Faction != null)
							info.Add(" - Faction: " + targetMob.Faction.Name);
						else
							info.Add(" - Faction: No faction");
						if (targetMob.Faction != null)
							info.Add(" - Faction Friends: " + targetMob.Faction.FriendFactions.Count);
						else
							info.Add(" - Faction Friends: 0");
						if (targetMob.Faction != null)
							info.Add(" - Faction Enemies: " + targetMob.Faction.FriendFactions.Count);
						else
							info.Add(" - Faction Enemies: 0");
						var aggroRange = "Near (< 500)";
						if (targetMob.AggroRange > 1000)
							aggroRange = "Far (1000+)";
						else if (targetMob.AggroRange > 500)
							aggroRange = "Medium (500+)";
						else if (targetMob.AggroRange == 0)
							aggroRange = "0";
						info.Add(" - Aggro Level: " + targetMob.GetAggroLevelString(player, true));
						info.Add(" - Aggro Range: " + aggroRange);

						var roaming = "";
						if (targetMob.RoamingRange == -1)
							roaming = "Default (Yes)";
						else if (targetMob.RoamingRange == 0)
							roaming = "No";
						else if (targetMob.RoamingRange > 0)
							roaming = "Yes";
						info.Add(" - Roaming: " + roaming);

						info.Add(" ");
						info.Add("Spawn");
						var respawn = TimeSpan.FromMilliseconds(targetMob.RespawnInterval);
						if (targetMob.RespawnInterval <= 0)
							info.Add(" - Respawn: No respawn");
						else
						{
							string days = "";
							string hours = "";

							if (respawn.Days > 0)
								days = respawn.Days + " days, ";
							if (respawn.Hours > 0)
								hours = respawn.Hours + " hours, ";

							info.Add(" - Respawn: " + days + hours + respawn.Minutes + " minutes, " + respawn.Seconds + " seconds");
						}
						if (targetMob.ClassType == "DOL.GS.NightSpawn")
							info.Add(" - Night Spawn only: Yes");
						else
							info.Add(" - Night Spawn only: No");
						if (targetMob.ClassType == "DOL.GS.DaySpawn")
							info.Add(" - Day Spawn only: Yes");
						else
							info.Add(" - Day Spawn only: No");

						info.Add(" ");
						info.Add("Stats");
						if (targetMob.NPCTemplate != null)
						{
							var template = targetMob.NPCTemplate;
							var minLevel = targetMob.Level;
							var maxLevel = targetMob.Level;

							if (template.Level.Contains(';') || template.Level.Contains('-'))
							{
								var split = Util.SplitCSV(template.Level, true); // Create list and remove separators or express ranges
								var levelRange = new List<byte>(); // Store list of levels from which to grab minimum int value
								if (levelRange.Count > 0)
									levelRange.Clear(); // Pre-usage cleanup

								// If we've added level entries to the split list
								if (split.Count > 0)
								{
									foreach (var level in split)
										if (byte.TryParse(level, out var levelEntry) && !levelRange.Contains(levelEntry))
											levelRange.Add(levelEntry);

									if (levelRange.Count > 0)
									{
										minLevel = levelRange.AsQueryable().Min();
										maxLevel = levelRange.AsQueryable().Max();
									}
								}
								// Try one more time to parse NPCTemplate.Level
								else if (byte.TryParse(template.Level, out var tryLevel))
									{
										minLevel = tryLevel; // Grab the lowest value
										maxLevel = tryLevel;
									}
							}
							// No separators or level ranges detected, so just treat as a single level entry
							else if (byte.TryParse(template.Level, out var onlyLevel))
								minLevel = onlyLevel; // Grab the lowest value

							var baseStr = template.Strength + modifier;
							var baseCon = template.Constitution + modifier;
							var baseDex = template.Dexterity + modifier;
							var baseQui = template.Quickness + modifier;
							var baseInt = template.Intelligence + modifier;
							var baseEmp = template.Empathy + modifier;
							var baseCha = template.Charisma + modifier;
							var basePie = template.Piety + modifier;

							var diff = maxLevel - minLevel;
							var strMax = baseStr + (Properties.MOB_AUTOSET_STR_MULTIPLIER * diff);
							var conMax = baseCon + (Properties.MOB_AUTOSET_CON_MULTIPLIER * diff);
							var dexMax = baseDex + (Properties.MOB_AUTOSET_DEX_MULTIPLIER * diff);
							var quiMax = baseQui + (Properties.MOB_AUTOSET_QUI_MULTIPLIER * diff);
							var intMax = baseInt + (Properties.MOB_AUTOSET_INT_MULTIPLIER * diff);
							var empMax = baseEmp + (Properties.MOB_AUTOSET_EMP_MULTIPLIER * diff);
							var chaMax = baseCha + (Properties.MOB_AUTOSET_CHA_MULTIPLIER * diff);
							var pieMax = basePie + (Properties.MOB_AUTOSET_PIE_MULTIPLIER * diff);

							info.Add(" - Level Range: " + minLevel + "-" + maxLevel);
							info.Add(" - STR: " + (template.Strength) + "-" + strMax + " (" + targetMob.GetModified(eProperty.Strength) + ")");
							info.Add(" - CON: " + (template.Constitution) + "-" + conMax + " (" + targetMob.GetModified(eProperty.Constitution) + ")");
							info.Add(" - DEX: " + (template.Dexterity) + "-" + dexMax + " (" + targetMob.GetModified(eProperty.Dexterity) + ")");
							info.Add(" - QUI: " + (template.Quickness) + "-" + quiMax + " (" + targetMob.GetModified(eProperty.Quickness) + ")");
							info.Add(" - INT: " + (template.Intelligence) + "-" + intMax + " (" + targetMob.GetModified(eProperty.Intelligence) + ")");
							info.Add(" - EMP: " + (template.Empathy) + "-" + empMax + " (" + targetMob.GetModified(eProperty.Empathy) + ")");
							info.Add(" - CHA: " + (template.Charisma) + "-" + chaMax + " (" + targetMob.GetModified(eProperty.Charisma) + ")");
							info.Add(" - PIE: " + (template.Piety) + "-" + pieMax + " (" + targetMob.GetModified(eProperty.Piety) + ")");
						}
						else
						{
							info.Add(" - STR: " + (targetMob.Strength - modifier) + "-" + (targetMob.Strength + modifier) + " (" + (targetMob.GetModified(eProperty.Strength) + modifier) + ")");
							info.Add(" - CON: " + (targetMob.Constitution - modifier) + "-" + (targetMob.Constitution + modifier) + " (" + (targetMob.GetModified(eProperty.Constitution) + modifier) + ")");
							info.Add(" - DEX: " + (targetMob.Dexterity - modifier) + "-" + (targetMob.Dexterity + modifier) + " (" + (targetMob.GetModified(eProperty.Dexterity) + modifier) + ")");
							info.Add(" - QUI: " + (targetMob.Quickness - modifier) + "-" + (targetMob.Quickness + modifier) + " (" + (targetMob.GetModified(eProperty.Quickness) + modifier) + ")");
							info.Add(" - INT: " + (targetMob.Intelligence - modifier) + "-" + (targetMob.Intelligence + modifier) + " (" + (targetMob.GetModified(eProperty.Intelligence) + modifier) + ")");
							info.Add(" - EMP: " + (targetMob.Empathy - modifier) + "-" + (targetMob.Empathy + modifier) + " (" + (targetMob.GetModified(eProperty.Empathy) + modifier) + ")");
							info.Add(" - CHA: " + (targetMob.Charisma - modifier) + "-" + (targetMob.Charisma + modifier) + " (" + (targetMob.GetModified(eProperty.Charisma) + modifier) + ")");
							info.Add(" - PIE: " + (targetMob.Piety - modifier) + "-" + (targetMob.Piety + modifier) + " (" + (targetMob.GetModified(eProperty.Piety) + modifier) + ")");
						}

						info.Add(" ");
						info.Add("Combat");
						if (targetMob.InCombat)
							info.Add(" - In Combat: Yes");
						else
							info.Add(" - In Combat: No");
						info.Add(" - Armor Factor (AF): " + (targetMob.GetModified(eProperty.ArmorFactor) * .9) + "-" + (targetMob.GetModified(eProperty.ArmorFactor) * 1.1));
						info.Add(" - Absorption (ABS): " + (targetMob.GetModified(eProperty.ArmorAbsorption) * .75) + "-" + (targetMob.GetModified(eProperty.ArmorAbsorption) * 1.25));
						if (targetMob.BlockChance > 0)
							info.Add(" - Block: This mob can block attacks");
						else
							info.Add(" - Block: 0%");
						if (targetMob.ParryChance > 0)
							info.Add(" - Parry: This mob can parry attacks");
						else
							info.Add(" - Parry: 0%");
						if (targetMob.EvadeChance > 0)
							info.Add(" - Evade: This mob can evade attacks");
						else
							info.Add(" - Evade: 0%");
						if (targetMob.LeftHandSwingChance > 0)
							info.Add(" - Left Swing: This mob can attack with a left-handed weapon");
						else
							info.Add(" - Left Swing: 0%");
						if (targetMob.MeleeDamageType != 0)
							info.Add(" - Damage Type: " + targetMob.MeleeDamageType);
						info.Add(" - Active Weapon: " + targetMob.ActiveWeaponSlot.ToString());
						if (targetMob.Spells != null && targetMob.Spells.Count > 0)
							info.Add(" - Spells: " + targetMob.Spells.Count);
						else
							info.Add(" - Spells: 0");
						if (targetMob.Styles != null && targetMob.Styles.Count > 0)
							info.Add(" - Combat Styles: " + targetMob.Styles.Count);
						else
							info.Add(" - Combat Styles: 0");

						info.Add(" ");
						info.Add("Resists");
						info.Add(" - Crush:  " + targetMob.GetDamageResist(eProperty.Resist_Crush) + " (" + targetMob.GetModified(eProperty.Resist_Crush) + ")");
						info.Add(" - Slash:  " + targetMob.GetDamageResist(eProperty.Resist_Slash) + " (" + targetMob.GetModified(eProperty.Resist_Slash) + ")");
						info.Add(" - Thrust:  " + targetMob.GetDamageResist(eProperty.Resist_Thrust) + " (" + targetMob.GetModified(eProperty.Resist_Thrust) + ")");
						info.Add(" - Heat:  " + targetMob.GetDamageResist(eProperty.Resist_Heat) + " (" + targetMob.GetModified(eProperty.Resist_Heat) + ")");
						info.Add(" - Cold:  " + targetMob.GetDamageResist(eProperty.Resist_Cold) + " (" + targetMob.GetModified(eProperty.Resist_Cold) + ")");
						info.Add(" - Matter:  " + targetMob.GetDamageResist(eProperty.Resist_Matter) + " (" + targetMob.GetModified(eProperty.Resist_Matter) + ")");
						info.Add(" - Body:  " + targetMob.GetDamageResist(eProperty.Resist_Body) + " (" + targetMob.GetModified(eProperty.Resist_Body) + ")");
						info.Add(" - Spirit:  " + targetMob.GetDamageResist(eProperty.Resist_Spirit) + " (" + targetMob.GetModified(eProperty.Resist_Spirit) + ")");
						info.Add(" - Energy:  " + targetMob.GetDamageResist(eProperty.Resist_Energy) + " (" + targetMob.GetModified(eProperty.Resist_Energy) + ")");

						AttackData ad = (AttackData)targetMob.TempProperties.getProperty<object>(GameLiving.LAST_ATTACK_DATA, null);
						if (ad != null)
						{
							info.Add(" ");
							info.Add("Attack Data");

							if (targetMob.TargetObject != null)
								info.Add(" - Target: " + targetMob.TargetObject.Name);
							else
								info.Add(" - Target: None");

							if (targetMob.Brain != null && targetMob.Brain is StandardMobBrain)
							{
								Dictionary<GameLiving, long> aggroList = (targetMob.Brain as StandardMobBrain).AggroTable;

								if (aggroList.Count > 0)
								{
									info.Add(" - Aggro List");

									foreach (GameLiving living in aggroList.Keys)
									{
										info.Add(" -- " + living.Name + ": " + aggroList[living]);
									}
								}
							}

							if (targetMob.attackComponent.Attackers != null && targetMob.attackComponent.Attackers.Count > 0)
							{
								info.Add(" - Attacker List");

								foreach (var o in targetMob.attackComponent.Attackers)
								{
									var attacker = (GameLiving) o;
									info.Add(" -- " + attacker.Name);
								}
							}

							ArrayList effectsToList = new ArrayList();
							if (effectsToList.Count > 0)
								effectsToList.Clear();

							if (targetMob.effectListComponent != null)
							{
								info.Add(" - Active Effects");
								foreach (ECSGameSpellEffect e in targetMob.effectListComponent.GetSpellEffects())
								{
									effectsToList.Add(e);
								}

								// List active spell effects
								foreach (ECSGameSpellEffect e in effectsToList)
								{
									var caster = "NONE";
									if (e.SpellHandler.Caster.Name != null)
									{
										caster = e.SpellHandler.Caster.Name;
										if (e.SpellHandler.Caster.Name == targetMob.Name)
											caster = "SELF";
									}

									info.Add(" -- " + e.SpellHandler.Spell.Name + " (" + e.EffectType.ToString() + ", level " + e.SpellHandler.Spell.Level + "): " + caster + " (Caster), " + (e.GetRemainingTimeForClient() / 1000) + " seconds remaining");
								}
							}
						}

						if (targetMob.ambientTexts != null && targetMob.ambientTexts.Count > 0)
						{
							info.Add (" ");
							info.Add ("Ambient Texts");
							foreach(var txt in targetMob.ambientTexts)
							{
								info.Add(" - " + txt.Trigger + ", " + txt.Chance);
								info.Add(" -- " + txt.Text);
							}
						}
					}

					client.Out.SendCustomTextWindow("Tester Information", info);
				}
					break;
				#endregion Mob Info

            }

			return;
		}
	}
}