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
/*
*Author         : Etaew - Fallen Realms
*Editor			: Gandulf
*Source         : http://camelot.allakhazam.com
*Date           : 22 November 2004
*Quest Name     : War Concluded (Level 50)
*Quest Classes  : Hunter, Shadowblade (Rogue)
*Quest Version  : v1
*
*Changes:
*add bonuses to epic items
*
*ToDo:
*
*   Add correct Text
*   Find Helm ModelID for epics..
*/

using System;
using System.Linq;
using System.Reflection;
using Atlas.DataLayer.Models;
using DOL.Events;
using DOL.GS;
using DOL.GS.PacketHandler;
using log4net;

namespace DOL.GS.Quests.Midgard
{
	public class Rogue_50 : BaseQuest
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected const string questTitle = "War Concluded";
		protected const int minimumLevel = 50;
		protected const int maximumLevel = 50;

		private static GameNPC Masrim = null; // Start NPC
		private static GameNPC Oona = null; // Mob to kill
		private static GameNPC MorlinCaan = null; // Trainer for reward

		private static ItemTemplate oona_head = null; //ball of flame
		private static ItemTemplate sealed_pouch = null; //sealed pouch
		private static ItemTemplate HunterEpicBoots = null; //Call of the Hunt Boots 
		private static ItemTemplate HunterEpicHelm = null; //Call of the Hunt Coif 
		private static ItemTemplate HunterEpicGloves = null; //Call of the Hunt Gloves 
		private static ItemTemplate HunterEpicVest = null; //Call of the Hunt Hauberk 
		private static ItemTemplate HunterEpicLegs = null; //Call of the Hunt Legs 
		private static ItemTemplate HunterEpicArms = null; //Call of the Hunt Sleeves 
		private static ItemTemplate ShadowbladeEpicBoots = null; //Shadow Shrouded Boots 
		private static ItemTemplate ShadowbladeEpicHelm = null; //Shadow Shrouded Coif 
		private static ItemTemplate ShadowbladeEpicGloves = null; //Shadow Shrouded Gloves 
		private static ItemTemplate ShadowbladeEpicVest = null; //Shadow Shrouded Hauberk 
		private static ItemTemplate ShadowbladeEpicLegs = null; //Shadow Shrouded Legs 
		private static ItemTemplate ShadowbladeEpicArms = null; //Shadow Shrouded Sleeves         

		// Constructors
		public Rogue_50() : base()
		{
		}

		public Rogue_50(GamePlayer questingPlayer) : base(questingPlayer)
		{
		}

		public Rogue_50(GamePlayer questingPlayer, int step) : base(questingPlayer, step)
		{
		}

		public Rogue_50(GamePlayer questingPlayer, Quest dbQuest) : base(questingPlayer, dbQuest)
		{
		}

		[ScriptLoadedEvent]
		public static void ScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			if (!ServerProperties.Properties.LOAD_QUESTS)
				return;
			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initializing ...");

			#region defineNPCs

			GameNPC[] npcs = WorldMgr.GetNPCsByName("Masrim", eRealm.Midgard);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 100 && npc.X == 749099 && npc.Y == 813104)
					{
						Masrim = npc;
						break;
					}

			if (Masrim == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Masrim , creating it ...");
				Masrim = new GameNPC();
				Masrim.Model = 177;
				Masrim.Name = "Masrim";
				Masrim.GuildName = "";
				Masrim.Realm = eRealm.Midgard;
				Masrim.CurrentRegionID = 100;
				Masrim.Size = 52;
				Masrim.Level = 40;
				Masrim.X = 749099;
				Masrim.Y = 813104;
				Masrim.Z = 4437;
				Masrim.Heading = 2605;
				Masrim.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					Masrim.SaveIntoDatabase();
				}
			}
			// end npc

			npcs = WorldMgr.GetNPCsByName("Oona", eRealm.None);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 100 && npc.X == 607233 && npc.Y == 786850)
					{
						Oona = npc;
						break;
					}

			if (Oona == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Oona , creating it ...");
				Oona = new GameNPC();
				Oona.Model = 356;
				Oona.Name = "Oona";
				Oona.GuildName = "";
				Oona.Realm = eRealm.None;
				Oona.CurrentRegionID = 100;
				Oona.Size = 50;
				Oona.Level = 65;
				Oona.X = 607233;
				Oona.Y = 786850;
				Oona.Z = 4384;
				Oona.Heading = 3891;
				Oona.Flags ^= GameNPC.eFlags.GHOST;
				Oona.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					Oona.SaveIntoDatabase();
				}
			}
			// end npc

			npcs = WorldMgr.GetNPCsByName("Morlin Caan", eRealm.Midgard);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 101 && npc.X == 33400 && npc.Y == 33620)
					{
						MorlinCaan = npc;
						break;
					}

			if (MorlinCaan == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Morlin Caan , creating it ...");
				MorlinCaan = new GameNPC();
				MorlinCaan.Model = 235;
				MorlinCaan.Name = "Morlin Caan";
				MorlinCaan.GuildName = "Smith";
				MorlinCaan.Realm = eRealm.Midgard;
				MorlinCaan.CurrentRegionID = 101;
				MorlinCaan.Size = 50;
				MorlinCaan.Level = 54;
				MorlinCaan.X = 33400;
				MorlinCaan.Y = 33620;
				MorlinCaan.Z = 8023;
				MorlinCaan.Heading = 523;
				MorlinCaan.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					MorlinCaan.SaveIntoDatabase();
				}
			}
			// end npc

			#endregion

			#region defineItems

			oona_head = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "oona_head");
			if (oona_head == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Oona's Head , creating it ...");
				oona_head = new ItemTemplate();
				oona_head.KeyName = "oona_head";
				oona_head.Name = "Oona's Head";
				oona_head.Level = 8;
				oona_head.ItemType = 29;
				oona_head.Model = 503;
				oona_head.IsDropable = false;
				oona_head.IsPickable = false;
				oona_head.DPS_AF = 0;
				oona_head.SPD_ABS = 0;
				oona_head.ObjectType = 41;
				oona_head.Hand = 0;
				oona_head.TypeDamage = 0;
				oona_head.Quality = 100;
				oona_head.Weight = 12;
				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(oona_head);
				}

			}

			sealed_pouch = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "sealed_pouch");
			if (sealed_pouch == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Sealed Pouch , creating it ...");
				sealed_pouch = new ItemTemplate();
				sealed_pouch.KeyName = "sealed_pouch";
				sealed_pouch.Name = "Sealed Pouch";
				sealed_pouch.Level = 8;
				sealed_pouch.ItemType = 29;
				sealed_pouch.Model = 488;
				sealed_pouch.IsDropable = false;
				sealed_pouch.IsPickable = false;
				sealed_pouch.DPS_AF = 0;
				sealed_pouch.SPD_ABS = 0;
				sealed_pouch.ObjectType = 41;
				sealed_pouch.Hand = 0;
				sealed_pouch.TypeDamage = 0;
				sealed_pouch.Quality = 100;
				sealed_pouch.Weight = 12;
				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(sealed_pouch);
				}

			}
// end item

			HunterEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HunterEpicBoots");
			if (HunterEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Hunters Epic Boots , creating it ...");
				HunterEpicBoots = new ItemTemplate();
				HunterEpicBoots.KeyName = "HunterEpicBoots";
				HunterEpicBoots.Name = "Call of the Hunt Boots";
				HunterEpicBoots.Level = 50;
				HunterEpicBoots.ItemType = 23;
				HunterEpicBoots.Model = 760;
				HunterEpicBoots.IsDropable = true;
				HunterEpicBoots.IsPickable = true;
				HunterEpicBoots.DPS_AF = 100;
				HunterEpicBoots.SPD_ABS = 19;
				HunterEpicBoots.ObjectType = 34;
				HunterEpicBoots.Quality = 100;
				HunterEpicBoots.Weight = 22;
				HunterEpicBoots.ItemBonus = 35;
				HunterEpicBoots.MaxCondition = 50000;
				HunterEpicBoots.MaxDurability = 50000;
				HunterEpicBoots.Condition = 50000;
				HunterEpicBoots.Durability = 50000;

				HunterEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 19 });

				HunterEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 19 });

				HunterEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Thrust, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HunterEpicBoots);
				}

			}
//end item
			//Call of the Hunt Coif 
			HunterEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HunterEpicHelm");
			if (HunterEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Hunters Epic Helm , creating it ...");
				HunterEpicHelm = new ItemTemplate();
				HunterEpicHelm.KeyName = "HunterEpicHelm";
				HunterEpicHelm.Name = "Call of the Hunt Coif";
				HunterEpicHelm.Level = 50;
				HunterEpicHelm.ItemType = 21;
				HunterEpicHelm.Model = 829; //NEED TO WORK ON..
				HunterEpicHelm.IsDropable = true;
				HunterEpicHelm.IsPickable = true;
				HunterEpicHelm.DPS_AF = 100;
				HunterEpicHelm.SPD_ABS = 19;
				HunterEpicHelm.ObjectType = 34;
				HunterEpicHelm.Quality = 100;
				HunterEpicHelm.Weight = 22;
				HunterEpicHelm.ItemBonus = 35;
				HunterEpicHelm.MaxCondition = 50000;
				HunterEpicHelm.MaxDurability = 50000;
				HunterEpicHelm.Condition = 50000;
				HunterEpicHelm.Durability = 50000;

				HunterEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Spear, BonusValue = 3 });

				HunterEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eProperty.Skill_Stealth, BonusValue = 3 });

				HunterEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.Skill_Composite, BonusValue = 3 });

				HunterEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.DEX, BonusValue = 19 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HunterEpicHelm);
				}

			}
//end item
			//Call of the Hunt Gloves 
			HunterEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HunterEpicGloves");
			if (HunterEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Hunters Epic Gloves , creating it ...");
				HunterEpicGloves = new ItemTemplate();
				HunterEpicGloves.KeyName = "HunterEpicGloves";
				HunterEpicGloves.Name = "Call of the Hunt Gloves ";
				HunterEpicGloves.Level = 50;
				HunterEpicGloves.ItemType = 22;
				HunterEpicGloves.Model = 759;
				HunterEpicGloves.IsDropable = true;
				HunterEpicGloves.IsPickable = true;
				HunterEpicGloves.DPS_AF = 100;
				HunterEpicGloves.SPD_ABS = 19;
				HunterEpicGloves.ObjectType = 34;
				HunterEpicGloves.Quality = 100;
				HunterEpicGloves.Weight = 22;
				HunterEpicGloves.ItemBonus = 35;
				HunterEpicGloves.MaxCondition = 50000;
				HunterEpicGloves.MaxDurability = 50000;
				HunterEpicGloves.Condition = 50000;
				HunterEpicGloves.Durability = 50000;

				HunterEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Composite, BonusValue = 5 });

				HunterEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 15 });

				HunterEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 33 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HunterEpicGloves);
				}

			}
			//Call of the Hunt Hauberk 
			HunterEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HunterEpicVest");
			if (HunterEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Hunters Epic Vest , creating it ...");
				HunterEpicVest = new ItemTemplate();
				HunterEpicVest.KeyName = "HunterEpicVest";
				HunterEpicVest.Name = "Call of the Hunt Jerkin";
				HunterEpicVest.Level = 50;
				HunterEpicVest.ItemType = 25;
				HunterEpicVest.Model = 756;
				HunterEpicVest.IsDropable = true;
				HunterEpicVest.IsPickable = true;
				HunterEpicVest.DPS_AF = 100;
				HunterEpicVest.SPD_ABS = 19;
				HunterEpicVest.ObjectType = 34;
				HunterEpicVest.Quality = 100;
				HunterEpicVest.Weight = 22;
				HunterEpicVest.ItemBonus = 35;
				HunterEpicVest.MaxCondition = 50000;
				HunterEpicVest.MaxDurability = 50000;
				HunterEpicVest.Condition = 50000;
				HunterEpicVest.Durability = 50000;

				HunterEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				HunterEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 15 });

				HunterEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 13 });

				HunterEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Cold, BonusValue = 6 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HunterEpicVest);
				}

			}
			//Call of the Hunt Legs 
			HunterEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HunterEpicLegs");
			if (HunterEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Hunters Epic Legs , creating it ...");
				HunterEpicLegs = new ItemTemplate();
				HunterEpicLegs.KeyName = "HunterEpicLegs";
				HunterEpicLegs.Name = "Call of the Hunt Legs";
				HunterEpicLegs.Level = 50;
				HunterEpicLegs.ItemType = 27;
				HunterEpicLegs.Model = 757;
				HunterEpicLegs.IsDropable = true;
				HunterEpicLegs.IsPickable = true;
				HunterEpicLegs.DPS_AF = 100;
				HunterEpicLegs.SPD_ABS = 19;
				HunterEpicLegs.ObjectType = 34;
				HunterEpicLegs.Quality = 100;
				HunterEpicLegs.Weight = 22;
				HunterEpicLegs.ItemBonus = 35;
				HunterEpicLegs.MaxCondition = 50000;
				HunterEpicLegs.MaxDurability = 50000;
				HunterEpicLegs.Condition = 50000;
				HunterEpicLegs.Durability = 50000;

				HunterEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 15 });

				HunterEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 15 });

				HunterEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.QUI, BonusValue = 7 });

				HunterEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Matter, BonusValue = 12 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HunterEpicLegs);
				}

			}
			//Call of the Hunt Sleeves 
			HunterEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HunterEpicArms");
			if (HunterEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Hunter Epic Arms , creating it ...");
				HunterEpicArms = new ItemTemplate();
				HunterEpicArms.KeyName = "HunterEpicArms";
				HunterEpicArms.Name = "Call of the Hunt Sleeves";
				HunterEpicArms.Level = 50;
				HunterEpicArms.ItemType = 28;
				HunterEpicArms.Model = 758;
				HunterEpicArms.IsDropable = true;
				HunterEpicArms.IsPickable = true;
				HunterEpicArms.DPS_AF = 100;
				HunterEpicArms.SPD_ABS = 19;
				HunterEpicArms.ObjectType = 34;
				HunterEpicArms.Quality = 100;
				HunterEpicArms.Weight = 22;
				HunterEpicArms.ItemBonus = 35;
				HunterEpicArms.MaxCondition = 50000;
				HunterEpicArms.MaxDurability = 50000;
				HunterEpicArms.Condition = 50000;
				HunterEpicArms.Durability = 50000;

				HunterEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 15 });

				HunterEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 15 });

				HunterEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Crush, BonusValue = 10 });

				HunterEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Slash, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HunterEpicArms);
				}

			}
			//Shadow Shrouded Boots 
			ShadowbladeEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ShadowbladeEpicBoots");
			if (ShadowbladeEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Shadowblade Epic Boots , creating it ...");
				ShadowbladeEpicBoots = new ItemTemplate();
				ShadowbladeEpicBoots.KeyName = "ShadowbladeEpicBoots";
				ShadowbladeEpicBoots.Name = "Shadow Shrouded Boots";
				ShadowbladeEpicBoots.Level = 50;
				ShadowbladeEpicBoots.ItemType = 23;
				ShadowbladeEpicBoots.Model = 765;
				ShadowbladeEpicBoots.IsDropable = true;
				ShadowbladeEpicBoots.IsPickable = true;
				ShadowbladeEpicBoots.DPS_AF = 100;
				ShadowbladeEpicBoots.SPD_ABS = 10;
				ShadowbladeEpicBoots.ObjectType = 33;
				ShadowbladeEpicBoots.Quality = 100;
				ShadowbladeEpicBoots.Weight = 22;
				ShadowbladeEpicBoots.ItemBonus = 35;
				ShadowbladeEpicBoots.MaxCondition = 50000;
				ShadowbladeEpicBoots.MaxDurability = 50000;
				ShadowbladeEpicBoots.Condition = 50000;
				ShadowbladeEpicBoots.Durability = 50000;

				ShadowbladeEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Stealth, BonusValue = 5 });

				ShadowbladeEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 13 });

				ShadowbladeEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.QUI, BonusValue = 13 });

				ShadowbladeEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Heat, BonusValue = 6 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ShadowbladeEpicBoots);
				}

			}
			//Shadow Shrouded Coif 
			ShadowbladeEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ShadowbladeEpicHelm");
			if (ShadowbladeEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Shadowblade Epic Helm , creating it ...");
				ShadowbladeEpicHelm = new ItemTemplate();
				ShadowbladeEpicHelm.KeyName = "ShadowbladeEpicHelm";
				ShadowbladeEpicHelm.Name = "Shadow Shrouded Coif";
				ShadowbladeEpicHelm.Level = 50;
				ShadowbladeEpicHelm.ItemType = 21;
				ShadowbladeEpicHelm.Model = 335; //NEED TO WORK ON..
				ShadowbladeEpicHelm.IsDropable = true;
				ShadowbladeEpicHelm.IsPickable = true;
				ShadowbladeEpicHelm.DPS_AF = 100;
				ShadowbladeEpicHelm.SPD_ABS = 10;
				ShadowbladeEpicHelm.ObjectType = 33;
				ShadowbladeEpicHelm.Quality = 100;
				ShadowbladeEpicHelm.Weight = 22;
				ShadowbladeEpicHelm.ItemBonus = 35;
				ShadowbladeEpicHelm.MaxCondition = 50000;
				ShadowbladeEpicHelm.MaxDurability = 50000;
				ShadowbladeEpicHelm.Condition = 50000;
				ShadowbladeEpicHelm.Durability = 50000;

				ShadowbladeEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 10 });

				ShadowbladeEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 12 });

				ShadowbladeEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 10 });

				ShadowbladeEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.QUI, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ShadowbladeEpicHelm);
				}

			}
			//Shadow Shrouded Gloves 
			ShadowbladeEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ShadowbladeEpicGloves");
			if (ShadowbladeEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Shadowblade Epic Gloves , creating it ...");
				ShadowbladeEpicGloves = new ItemTemplate();
				ShadowbladeEpicGloves.KeyName = "ShadowbladeEpicGloves";
				ShadowbladeEpicGloves.Name = "Shadow Shrouded Gloves";
				ShadowbladeEpicGloves.Level = 50;
				ShadowbladeEpicGloves.ItemType = 22;
				ShadowbladeEpicGloves.Model = 764;
				ShadowbladeEpicGloves.IsDropable = true;
				ShadowbladeEpicGloves.IsPickable = true;
				ShadowbladeEpicGloves.DPS_AF = 100;
				ShadowbladeEpicGloves.SPD_ABS = 10;
				ShadowbladeEpicGloves.ObjectType = 33;
				ShadowbladeEpicGloves.Quality = 100;
				ShadowbladeEpicGloves.Weight = 22;
				ShadowbladeEpicGloves.ItemBonus = 35;
				ShadowbladeEpicGloves.MaxCondition = 50000;
				ShadowbladeEpicGloves.MaxDurability = 50000;
				ShadowbladeEpicGloves.Condition = 50000;
				ShadowbladeEpicGloves.Durability = 50000;

				ShadowbladeEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Critical_Strike, BonusValue = 2 });

				ShadowbladeEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 12 });

				ShadowbladeEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 33 });

				ShadowbladeEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.Skill_Envenom, BonusValue = 4 });


				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ShadowbladeEpicGloves);
				}

			}
			//Shadow Shrouded Hauberk 
			ShadowbladeEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ShadowbladeEpicVest");
			if (ShadowbladeEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Shadowblade Epic Vest , creating it ...");
				ShadowbladeEpicVest = new ItemTemplate();
				ShadowbladeEpicVest.KeyName = "ShadowbladeEpicVest";
				ShadowbladeEpicVest.Name = "Shadow Shrouded Jerkin";
				ShadowbladeEpicVest.Level = 50;
				ShadowbladeEpicVest.ItemType = 25;
				ShadowbladeEpicVest.Model = 761;
				ShadowbladeEpicVest.IsDropable = true;
				ShadowbladeEpicVest.IsPickable = true;
				ShadowbladeEpicVest.DPS_AF = 100;
				ShadowbladeEpicVest.SPD_ABS = 10;
				ShadowbladeEpicVest.ObjectType = 33;
				ShadowbladeEpicVest.Quality = 100;
				ShadowbladeEpicVest.Weight = 22;
				ShadowbladeEpicVest.ItemBonus = 35;
				ShadowbladeEpicVest.MaxCondition = 50000;
				ShadowbladeEpicVest.MaxDurability = 50000;
				ShadowbladeEpicVest.Condition = 50000;
				ShadowbladeEpicVest.Durability = 50000;

				ShadowbladeEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				ShadowbladeEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 13 });

				ShadowbladeEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				ShadowbladeEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Heat, BonusValue = 6 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ShadowbladeEpicVest);
				}

			}
			//Shadow Shrouded Legs 
			ShadowbladeEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ShadowbladeEpicLegs");
			if (ShadowbladeEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Shadowblade Epic Legs , creating it ...");
				ShadowbladeEpicLegs = new ItemTemplate();
				ShadowbladeEpicLegs.KeyName = "ShadowbladeEpicLegs";
				ShadowbladeEpicLegs.Name = "Shadow Shrouded Legs";
				ShadowbladeEpicLegs.Level = 50;
				ShadowbladeEpicLegs.ItemType = 27;
				ShadowbladeEpicLegs.Model = 762;
				ShadowbladeEpicLegs.IsDropable = true;
				ShadowbladeEpicLegs.IsPickable = true;
				ShadowbladeEpicLegs.DPS_AF = 100;
				ShadowbladeEpicLegs.SPD_ABS = 10;
				ShadowbladeEpicLegs.ObjectType = 33;
				ShadowbladeEpicLegs.Quality = 100;
				ShadowbladeEpicLegs.Weight = 22;
				ShadowbladeEpicLegs.ItemBonus = 35;
				ShadowbladeEpicLegs.MaxCondition = 50000;
				ShadowbladeEpicLegs.MaxDurability = 50000;
				ShadowbladeEpicLegs.Condition = 50000;
				ShadowbladeEpicLegs.Durability = 50000;

				ShadowbladeEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 12 });

				ShadowbladeEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 15 });

				ShadowbladeEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.QUI, BonusValue = 12 });

				ShadowbladeEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Slash, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ShadowbladeEpicLegs);
				}

			}
			//Shadow Shrouded Sleeves 
			ShadowbladeEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ShadowbladeEpicArms");
			if (ShadowbladeEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Shadowblade Epic Arms , creating it ...");
				ShadowbladeEpicArms = new ItemTemplate();
				ShadowbladeEpicArms.KeyName = "ShadowbladeEpicArms";
				ShadowbladeEpicArms.Name = "Shadow Shrouded Sleeves";
				ShadowbladeEpicArms.Level = 50;
				ShadowbladeEpicArms.ItemType = 28;
				ShadowbladeEpicArms.Model = 763;
				ShadowbladeEpicArms.IsDropable = true;
				ShadowbladeEpicArms.IsPickable = true;
				ShadowbladeEpicArms.DPS_AF = 100;
				ShadowbladeEpicArms.SPD_ABS = 10;
				ShadowbladeEpicArms.ObjectType = 33;
				ShadowbladeEpicArms.Quality = 100;
				ShadowbladeEpicArms.Weight = 22;
				ShadowbladeEpicArms.ItemBonus = 35;
				ShadowbladeEpicArms.MaxCondition = 50000;
				ShadowbladeEpicArms.MaxDurability = 50000;
				ShadowbladeEpicArms.Condition = 50000;
				ShadowbladeEpicArms.Durability = 50000;

				ShadowbladeEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 15 });

				ShadowbladeEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 16 });

				ShadowbladeEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Crush, BonusValue = 10 });

				ShadowbladeEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Thrust, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ShadowbladeEpicArms);
				}

			}
//Shadowblade Epic Sleeves End
//Item Descriptions End

			#endregion

			GameEventMgr.AddHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.AddHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.AddHandler(Masrim, GameObjectEvent.Interact, new DOLEventHandler(TalkToMasrim));
			GameEventMgr.AddHandler(Masrim, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToMasrim));

			GameEventMgr.AddHandler(MorlinCaan, GameObjectEvent.Interact, new DOLEventHandler(TalkToMorlinCaan));
			GameEventMgr.AddHandler(MorlinCaan, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToMorlinCaan));

			/* Now we bring to Masrim the possibility to give this quest to players */
			Masrim.AddQuestToGive(typeof (Rogue_50));

			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initialized");
		}

		[ScriptUnloadedEvent]
		public static void ScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			//if not loaded, don't worry
			if (Masrim == null || MorlinCaan == null)
				return;
			// remove handlers
			GameEventMgr.RemoveHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.RemoveHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.RemoveHandler(Masrim, GameObjectEvent.Interact, new DOLEventHandler(TalkToMasrim));
			GameEventMgr.RemoveHandler(Masrim, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToMasrim));

			GameEventMgr.RemoveHandler(MorlinCaan, GameObjectEvent.Interact, new DOLEventHandler(TalkToMorlinCaan));
			GameEventMgr.RemoveHandler(MorlinCaan, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToMorlinCaan));

			/* Now we remove to Masrim the possibility to give this quest to players */
			Masrim.RemoveQuestToGive(typeof (Rogue_50));
		}

		protected static void TalkToMasrim(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs) args).Source as GamePlayer;
			if (player == null)
				return;

			if(Masrim.CanGiveQuest(typeof (Rogue_50), player)  <= 0)
				return;

			//We also check if the player is already doing the quest
			Rogue_50 quest = player.IsDoingQuest(typeof (Rogue_50)) as Rogue_50;

			if (e == GameObjectEvent.Interact)
			{
				// Nag to finish quest
				if (quest != null)
				{
					Masrim.SayTo(player, "Check your Journal for instructions!");
				}
				else
				{
					Masrim.SayTo(player, "Midgard needs your [services]");
				}
			}
				// The player whispered to the NPC
			else if (e == GameLivingEvent.WhisperReceive)
			{
				WhisperReceiveEventArgs wArgs = (WhisperReceiveEventArgs) args;
				//Check player is already doing quest
				if (quest == null)
				{
					switch (wArgs.Text)
					{
						case "services":
							player.Out.SendQuestSubscribeCommand(Masrim, QuestMgr.GetIDForQuestType(typeof(Rogue_50)), "Will you help Masrim [Rogue Level 50 Epic]?");
							break;
					}
				}
				else
				{
					switch (wArgs.Text)
					{
						case "abort":
							player.Out.SendCustomDialog("Do you really want to abort this quest, \nall items gained during quest will be lost?", new CustomDialogResponse(CheckPlayerAbortQuest));
							break;
					}
				}
			}
		}

		protected static void TalkToMorlinCaan(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs) args).Source as GamePlayer;
			if (player == null)
				return;

			if(Masrim.CanGiveQuest(typeof (Rogue_50), player)  <= 0)
				return;

			//We also check if the player is already doing the quest
			Rogue_50 quest = player.IsDoingQuest(typeof (Rogue_50)) as Rogue_50;

			if (e == GameObjectEvent.Interact)
			{
				if (quest != null)
				{
					MorlinCaan.SayTo(player, "Check your journal for instructions!");
				}
				return;
			}
		}

		public override bool CheckQuestQualification(GamePlayer player)
		{
			// if the player is already doing the quest his level is no longer of relevance
			if (player.IsDoingQuest(typeof (Rogue_50)) != null)
				return true;

			if (player.CharacterClass.ID != (byte) eCharacterClass.Shadowblade &&
				player.CharacterClass.ID != (byte) eCharacterClass.Hunter)
				return false;

			// This checks below are only performed is player isn't doing quest already

			//if (player.HasFinishedQuest(typeof(Academy_47)) == 0) return false;

			//if (!CheckPartAccessible(player,typeof(CityOfCamelot)))
			//	return false;

			if (player.Level < minimumLevel || player.Level > maximumLevel)
				return false;

			return true;
		}

		/* This is our callback hook that will be called when the player clicks
		 * on any button in the quest offer dialog. We check if he accepts or
		 * declines here...
		 */

		private static void CheckPlayerAbortQuest(GamePlayer player, byte response)
		{
			Rogue_50 quest = player.IsDoingQuest(typeof (Rogue_50)) as Rogue_50;

			if (quest == null)
				return;

			if (response == 0x00)
			{
				SendSystemMessage(player, "Good, no go out there and finish your work!");
			}
			else
			{
				SendSystemMessage(player, "Aborting Quest " + questTitle + ". You can start over again if you want.");
				quest.AbortQuest();
			}
		}

		protected static void SubscribeQuest(DOLEvent e, object sender, EventArgs args)
		{
			QuestEventArgs qargs = args as QuestEventArgs;
			if (qargs == null)
				return;

			if (qargs.QuestID != QuestMgr.GetIDForQuestType(typeof(Rogue_50)))
				return;

			if (e == GamePlayerEvent.AcceptQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x01);
			else if (e == GamePlayerEvent.DeclineQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x00);
		}

		private static void CheckPlayerAcceptQuest(GamePlayer player, byte response)
		{
			if(Masrim.CanGiveQuest(typeof (Rogue_50), player)  <= 0)
				return;

			if (player.IsDoingQuest(typeof (Rogue_50)) != null)
				return;

			if (response == 0x00)
			{
				player.Out.SendMessage("Our God forgives your laziness, just look out for stray lightning bolts.", eChatType.CT_Say, eChatLoc.CL_PopupWindow);
			}
			else
			{
				//Check if we can add the quest!
				if (!Masrim.GiveQuest(typeof (Rogue_50), player, 1))
					return;

				player.Out.SendMessage("Kill Oona in Raumarik loc 20k,51k!", eChatType.CT_System, eChatLoc.CL_PopupWindow);
			}
		}

		//Set quest name
		public override string Name
		{
			get { return "War Concluded (Level 50 Rogue Epic)"; }
		}

		// Define Steps
		public override string Description
		{
			get
			{
				switch (Step)
				{
					case 1:
						return "[Step #1] Seek out Oona in Raumarik Loc 20k,51k kill it!";
					case 2:
						return "[Step #2] Return to Masrim and give her Oona's Head!";
					case 3:
						return "[Step #3] Go to Morlin Caan in Jordheim and give him the Sealed Pouch for your reward!";
				}
				return base.Description;
			}
		}

		public override void Notify(DOLEvent e, object sender, EventArgs args)
		{
			GamePlayer player = sender as GamePlayer;

			if (player==null || player.IsDoingQuest(typeof (Rogue_50)) == null)
				return;

			if (Step == 1 && e == GameLivingEvent.EnemyKilled)
			{
				EnemyKilledEventArgs gArgs = (EnemyKilledEventArgs) args;
				if (gArgs.Target.Name == Oona.Name)
				{
					m_questPlayer.Out.SendMessage("You collect Oona's Head", eChatType.CT_System, eChatLoc.CL_SystemWindow);
					GiveItem(m_questPlayer, oona_head);
					Step = 2;
					return;
				}
			}

			if (Step == 2 && e == GamePlayerEvent.GiveItem)
			{
				GiveItemEventArgs gArgs = (GiveItemEventArgs) args;
				if (gArgs.Target.Name == Masrim.Name && gArgs.Item.ItemTemplate.KeyName == oona_head.KeyName)
				{
					RemoveItem(Masrim, player, oona_head);
					Masrim.SayTo(player, "Take this sealed pouch to Morlin Caan in Jordheim for your reward!");
					GiveItem(player, sealed_pouch);
					Step = 3;
					return;
				}
			}

			if (Step == 3 && e == GamePlayerEvent.GiveItem)
			{
				GiveItemEventArgs gArgs = (GiveItemEventArgs) args;
				if (gArgs.Target.Name == MorlinCaan.Name && gArgs.Item.ItemTemplate.KeyName == sealed_pouch.KeyName)
				{
					MorlinCaan.SayTo(player, "You have earned this Epic Armour!");
					FinishQuest();
					return;
				}
			}
		}

		public override void AbortQuest()
		{
			base.AbortQuest(); //Defined in Quest, changes the state, stores in DB etc ...

			RemoveItem(m_questPlayer, sealed_pouch, false);
		}

		public override void FinishQuest()
		{
			if (m_questPlayer.Inventory.IsSlotsFree(6, eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack))
			{
				RemoveItem(MorlinCaan, m_questPlayer, sealed_pouch);

				base.FinishQuest(); //Defined in Quest, changes the state, stores in DB etc ...

				if (m_questPlayer.CharacterClass.ID == (byte)eCharacterClass.Shadowblade)
				{
					GiveItem(m_questPlayer, ShadowbladeEpicArms);
					GiveItem(m_questPlayer, ShadowbladeEpicBoots);
					GiveItem(m_questPlayer, ShadowbladeEpicGloves);
					GiveItem(m_questPlayer, ShadowbladeEpicHelm);
					GiveItem(m_questPlayer, ShadowbladeEpicLegs);
					GiveItem(m_questPlayer, ShadowbladeEpicVest);
				}
				else if (m_questPlayer.CharacterClass.ID == (byte)eCharacterClass.Hunter)
				{
					GiveItem(m_questPlayer, HunterEpicArms);
					GiveItem(m_questPlayer, HunterEpicBoots);
					GiveItem(m_questPlayer, HunterEpicGloves);
					GiveItem(m_questPlayer, HunterEpicHelm);
					GiveItem(m_questPlayer, HunterEpicLegs);
					GiveItem(m_questPlayer, HunterEpicVest);
				}

				m_questPlayer.GainExperience(eXPSource.Quest, 1937768448, true);
				//m_questPlayer.AddMoney(Money.GetMoney(0,0,0,2,Util.Random(50)), "You recieve {0} as a reward.");		
			}
			else
			{
				m_questPlayer.Out.SendMessage("You do not have enough free space in your inventory!", eChatType.CT_Important, eChatLoc.CL_SystemWindow);
			}
		}

		#region Allakhazam Epic Source

		/*
        *#25 talk to Masrim
        *#26 seek out Loken in Raumarik Loc 47k, 25k, 4k, and kill him purp and 2 blue adds 
        *#27 return to Masrim 
        *#28 give her the ball of flame
        *#29 talk with Masrim about Loken�s demise
        *#30 go to MorlinCaan in Jordheim 
        *#31 give her the sealed pouch
        *#32 you get your epic armor as a reward
        */

		/*
            *Call of the Hunt Boots 
            *Call of the Hunt Coif
            *Call of the Hunt Gloves
            *Call of the Hunt Hauberk
            *Call of the Hunt Legs
            *Call of the Hunt Sleeves
            *Shadow Shrouded Boots
            *Shadow Shrouded Coif
            *Shadow Shrouded Gloves
            *Shadow Shrouded Hauberk
            *Shadow Shrouded Legs
            *Shadow Shrouded Sleeves
        */

		#endregion
	}
}
