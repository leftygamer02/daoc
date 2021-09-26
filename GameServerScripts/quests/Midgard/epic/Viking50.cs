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
*Quest Name     : An End to the Daggers (level 50)
*Quest Classes  : Warrior, Berserker, Thane, Skald, Savage (Viking)
*Quest Version  : v1
*
*Done:
*Bonuses to epic items
*
*ToDo:   
*   Find Helm ModelID for epics..
*   checks for all other epics done
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
	public class Viking_50 : BaseQuest
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected const string questTitle = "An End to the Daggers";
		protected const int minimumLevel = 50;
		protected const int maximumLevel = 50;

		private static GameNPC Lynnleigh = null; // Start NPC
		private static GameNPC Ydenia = null; // Mob to kill
		private static GameNPC Elizabeth = null; // reward NPC

		private static ItemTemplate tome_enchantments = null;
		private static ItemTemplate sealed_pouch = null;
		private static ItemTemplate WarriorEpicBoots = null;
		private static ItemTemplate WarriorEpicHelm = null;
		private static ItemTemplate WarriorEpicGloves = null;
		private static ItemTemplate WarriorEpicLegs = null;
		private static ItemTemplate WarriorEpicArms = null;
		private static ItemTemplate WarriorEpicVest = null;
		private static ItemTemplate BerserkerEpicBoots = null;
		private static ItemTemplate BerserkerEpicHelm = null;
		private static ItemTemplate BerserkerEpicGloves = null;
		private static ItemTemplate BerserkerEpicLegs = null;
		private static ItemTemplate BerserkerEpicArms = null;
		private static ItemTemplate BerserkerEpicVest = null;
		private static ItemTemplate ThaneEpicBoots = null;
		private static ItemTemplate ThaneEpicHelm = null;
		private static ItemTemplate ThaneEpicGloves = null;
		private static ItemTemplate ThaneEpicLegs = null;
		private static ItemTemplate ThaneEpicArms = null;
		private static ItemTemplate ThaneEpicVest = null;
		private static ItemTemplate SkaldEpicBoots = null;
		private static ItemTemplate SkaldEpicHelm = null;
		private static ItemTemplate SkaldEpicGloves = null;
		private static ItemTemplate SkaldEpicVest = null;
		private static ItemTemplate SkaldEpicLegs = null;
		private static ItemTemplate SkaldEpicArms = null;
		private static ItemTemplate SavageEpicBoots = null;
		private static ItemTemplate SavageEpicHelm = null;
		private static ItemTemplate SavageEpicGloves = null;
		private static ItemTemplate SavageEpicVest = null;
		private static ItemTemplate SavageEpicLegs = null;
		private static ItemTemplate SavageEpicArms = null;
		private static ItemTemplate ValkyrieEpicBoots = null;
		private static ItemTemplate ValkyrieEpicHelm = null;
		private static ItemTemplate ValkyrieEpicGloves = null;
		private static ItemTemplate ValkyrieEpicVest = null;
		private static ItemTemplate ValkyrieEpicLegs = null;
		private static ItemTemplate ValkyrieEpicArms = null;
        private static ItemTemplate MaulerMidEpicBoots = null;
        private static ItemTemplate MaulerMidEpicHelm = null;
        private static ItemTemplate MaulerMidEpicGloves = null;
        private static ItemTemplate MaulerMidEpicVest = null;
        private static ItemTemplate MaulerMidEpicLegs = null;
        private static ItemTemplate MaulerMidEpicArms = null; 


		// Constructors
		public Viking_50() : base()
		{
		}

		public Viking_50(GamePlayer questingPlayer) : base(questingPlayer)
		{
		}

		public Viking_50(GamePlayer questingPlayer, int step) : base(questingPlayer, step)
		{
		}

		public Viking_50(GamePlayer questingPlayer, Quest dbQuest) : base(questingPlayer, dbQuest)
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

			GameNPC[] npcs = WorldMgr.GetNPCsByName("Lynnleigh", eRealm.Midgard);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 100 && npc.X == 760118 && npc.Y == 758453)
					{
						Lynnleigh = npc;
						break;
					}

			if (Lynnleigh == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Lynnleigh , creating it ...");
				Lynnleigh = new GameNPC();
				Lynnleigh.Model = 217;
				Lynnleigh.Name = "Lynnleigh";
				Lynnleigh.GuildName = "";
				Lynnleigh.Realm = eRealm.Midgard;
				Lynnleigh.CurrentRegionID = 100;
				Lynnleigh.Size = 51;
				Lynnleigh.Level = 50;
				Lynnleigh.X = 760118;
				Lynnleigh.Y = 758453;
				Lynnleigh.Z = 4737;
				Lynnleigh.Heading = 2197;
				Lynnleigh.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					Lynnleigh.SaveIntoDatabase();
				}
			}
			// end npc
			npcs = WorldMgr.GetNPCsByName("Elizabeth", eRealm.Midgard);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 100 && npc.X == 802597 && npc.Y == 727896)
					{
						Elizabeth = npc;
						break;
					}

			if (Elizabeth == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Elizabeth , creating it ...");
				Elizabeth = new GameNPC();
				Elizabeth.Model = 217;
				Elizabeth.Name = "Elizabeth";
				Elizabeth.Realm = eRealm.Midgard;
				Elizabeth.CurrentRegionID = 100;
				Elizabeth.Size = 51;
				Elizabeth.Level = 41;
				Elizabeth.X = 802597;
				Elizabeth.Y = 727896;
				Elizabeth.Z = 4760;
				Elizabeth.Heading = 2480;
				Elizabeth.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					Elizabeth.SaveIntoDatabase();
				}

			}
			// end npc

			npcs = WorldMgr.GetNPCsByName("Ydenia of Seithkona", eRealm.None);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 100 && npc.X == 637680 && npc.Y == 767189)
					{
						Ydenia = npc;
						break;
					}

			if (Ydenia == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Ydenia , creating it ...");
				Ydenia = new GameNPC();
				Ydenia.Model = 217;
				Ydenia.Name = "Ydenia of Seithkona";
				Ydenia.GuildName = "";
				Ydenia.Realm = eRealm.None;
				Ydenia.CurrentRegionID = 100;
				Ydenia.Size = 100;
				Ydenia.Level = 65;
				Ydenia.X = 637680;
				Ydenia.Y = 767189;
				Ydenia.Z = 4480;
				Ydenia.Heading = 2156;
				Ydenia.Flags ^= GameNPC.eFlags.GHOST;
				Ydenia.MaxSpeedBase = 200;
				Ydenia.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					Ydenia.SaveIntoDatabase();
				}
			}
			// end npc

			#endregion

			#region defineItems

			tome_enchantments = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "tome_enchantments");
			if (tome_enchantments == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Tome of Enchantments , creating it ...");
				tome_enchantments = new ItemTemplate();
				tome_enchantments.KeyName = "tome_enchantments";
				tome_enchantments.Name = "Tome of Enchantments";
				tome_enchantments.Level = 8;
				tome_enchantments.ItemType = 0;
				tome_enchantments.Model = 500;
				tome_enchantments.IsDropable = false;
				tome_enchantments.IsPickable = false;
				tome_enchantments.DPS_AF = 0;
				tome_enchantments.SPD_ABS = 0;
				tome_enchantments.ObjectType = 0;
				tome_enchantments.Hand = 0;
				tome_enchantments.TypeDamage = 0;
				tome_enchantments.Quality = 100;
				tome_enchantments.Weight = 12;
				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(tome_enchantments);
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

			WarriorEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WarriorEpicBoots");
			if (WarriorEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warrior Epic Boots , creating it ...");
				WarriorEpicBoots = new ItemTemplate();
				WarriorEpicBoots.KeyName = "WarriorEpicBoots";
				WarriorEpicBoots.Name = "Tyr's Might Boots";
				WarriorEpicBoots.Level = 50;
				WarriorEpicBoots.ItemType = 23;
				WarriorEpicBoots.Model = 780;
				WarriorEpicBoots.IsDropable = true;
				WarriorEpicBoots.IsPickable = true;
				WarriorEpicBoots.DPS_AF = 100;
				WarriorEpicBoots.SPD_ABS = 27;
				WarriorEpicBoots.ObjectType = 35;
				WarriorEpicBoots.Quality = 100;
				WarriorEpicBoots.Weight = 22;
				WarriorEpicBoots.ItemBonus = 35;
				WarriorEpicBoots.MaxCondition = 50000;
				WarriorEpicBoots.MaxDurability = 50000;
				WarriorEpicBoots.Durability = 50000;
				WarriorEpicBoots.Condition = 50000;

				WarriorEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 16 });

				WarriorEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 15 });

				WarriorEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Heat, BonusValue = 10 });

				WarriorEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Energy, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WarriorEpicBoots);
				}

			}
//end item
			WarriorEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WarriorEpicHelm");
			if (WarriorEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warrior Epic Helm , creating it ...");
				WarriorEpicHelm = new ItemTemplate();
				WarriorEpicHelm.KeyName = "WarriorEpicHelm";
				WarriorEpicHelm.Name = "Tyr's Might Coif";
				WarriorEpicHelm.Level = 50;
				WarriorEpicHelm.ItemType = 21;
				WarriorEpicHelm.Model = 832; //NEED TO WORK ON..
				WarriorEpicHelm.IsDropable = true;
				WarriorEpicHelm.IsPickable = true;
				WarriorEpicHelm.DPS_AF = 100;
				WarriorEpicHelm.SPD_ABS = 27;
				WarriorEpicHelm.ObjectType = 35;
				WarriorEpicHelm.Quality = 100;
				WarriorEpicHelm.Weight = 22;
				WarriorEpicHelm.ItemBonus = 35;
				WarriorEpicHelm.MaxCondition = 50000;
				WarriorEpicHelm.MaxDurability = 50000;
				WarriorEpicHelm.Condition = 50000;
				WarriorEpicHelm.Durability = 50000;

				WarriorEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 12 });

				WarriorEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 12 });

				WarriorEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 12 });

				WarriorEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Crush, BonusValue = 11 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WarriorEpicHelm);
				}

			}
//end item
			WarriorEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WarriorEpicGloves");
			if (WarriorEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warrior Epic Gloves , creating it ...");
				WarriorEpicGloves = new ItemTemplate();
				WarriorEpicGloves.KeyName = "WarriorEpicGloves";
				WarriorEpicGloves.Name = "Tyr's Might Gloves";
				WarriorEpicGloves.Level = 50;
				WarriorEpicGloves.ItemType = 22;
				WarriorEpicGloves.Model = 779;
				WarriorEpicGloves.IsDropable = true;
				WarriorEpicGloves.IsPickable = true;
				WarriorEpicGloves.DPS_AF = 100;
				WarriorEpicGloves.SPD_ABS = 27;
				WarriorEpicGloves.ObjectType = 35;
				WarriorEpicGloves.Quality = 100;
				WarriorEpicGloves.Weight = 22;
				WarriorEpicGloves.ItemBonus = 35;
				WarriorEpicGloves.MaxCondition = 50000;
				WarriorEpicGloves.MaxDurability = 50000;
				WarriorEpicGloves.Condition = 50000;
				WarriorEpicGloves.Durability = 50000;

				WarriorEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Shields, BonusValue = 3 });

				WarriorEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eProperty.Skill_Parry, BonusValue = 3 });

				WarriorEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.STR, BonusValue = 15 });

				WarriorEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.DEX, BonusValue = 13 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WarriorEpicGloves);
				}

			}

			WarriorEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WarriorEpicVest");
			if (WarriorEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warrior Epic Vest , creating it ...");
				WarriorEpicVest = new ItemTemplate();
				WarriorEpicVest.KeyName = "WarriorEpicVest";
				WarriorEpicVest.Name = "Tyr's Might Hauberk";
				WarriorEpicVest.Level = 50;
				WarriorEpicVest.ItemType = 25;
				WarriorEpicVest.Model = 776;
				WarriorEpicVest.IsDropable = true;
				WarriorEpicVest.IsPickable = true;
				WarriorEpicVest.DPS_AF = 100;
				WarriorEpicVest.SPD_ABS = 27;
				WarriorEpicVest.ObjectType = 35;
				WarriorEpicVest.Quality = 100;
				WarriorEpicVest.Weight = 22;
				WarriorEpicVest.ItemBonus = 35;
				WarriorEpicVest.MaxCondition = 50000;
				WarriorEpicVest.MaxDurability = 50000;
				WarriorEpicVest.Condition = 50000;
				WarriorEpicVest.Durability = 50000;

				WarriorEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				WarriorEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 13 });

				WarriorEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Matter, BonusValue = 6 });

				WarriorEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WarriorEpicVest);
				}

			}

			WarriorEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WarriorEpicLegs");
			if (WarriorEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warrior Epic Legs , creating it ...");
				WarriorEpicLegs = new ItemTemplate();
				WarriorEpicLegs.KeyName = "WarriorEpicLegs";
				WarriorEpicLegs.Name = "Tyr's Might Legs";
				WarriorEpicLegs.Level = 50;
				WarriorEpicLegs.ItemType = 27;
				WarriorEpicLegs.Model = 777;
				WarriorEpicLegs.IsDropable = true;
				WarriorEpicLegs.IsPickable = true;
				WarriorEpicLegs.DPS_AF = 100;
				WarriorEpicLegs.SPD_ABS = 27;
				WarriorEpicLegs.ObjectType = 35;
				WarriorEpicLegs.Quality = 100;
				WarriorEpicLegs.Weight = 22;
				WarriorEpicLegs.ItemBonus = 35;
				WarriorEpicLegs.MaxCondition = 50000;
				WarriorEpicLegs.MaxDurability = 50000;
				WarriorEpicLegs.Condition = 50000;
				WarriorEpicLegs.Durability = 50000;

				WarriorEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 22 });

				WarriorEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.STR, BonusValue = 15 });

				WarriorEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Cold, BonusValue = 8 });

				WarriorEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Body, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WarriorEpicLegs);
				}

			}

			WarriorEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WarriorEpicArms");
			if (WarriorEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warrior Epic Arms , creating it ...");
				WarriorEpicArms = new ItemTemplate();
				WarriorEpicArms.KeyName = "WarriorEpicArms";
				WarriorEpicArms.Name = "Tyr's Might Sleeves";
				WarriorEpicArms.Level = 50;
				WarriorEpicArms.ItemType = 28;
				WarriorEpicArms.Model = 778;
				WarriorEpicArms.IsDropable = true;
				WarriorEpicArms.IsPickable = true;
				WarriorEpicArms.DPS_AF = 100;
				WarriorEpicArms.SPD_ABS = 27;
				WarriorEpicArms.ObjectType = 35;
				WarriorEpicArms.Quality = 100;
				WarriorEpicArms.Weight = 22;
				WarriorEpicArms.ItemBonus = 35;
				WarriorEpicArms.MaxCondition = 50000;
				WarriorEpicArms.MaxDurability = 50000;
				WarriorEpicArms.Condition = 50000;
				WarriorEpicArms.Durability = 50000;

				WarriorEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.DEX, BonusValue = 22 });

				WarriorEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 15 });

				WarriorEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Crush, BonusValue = 8 });

				WarriorEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Slash, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WarriorEpicArms);
				}

			}
			BerserkerEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "BerserkerEpicBoots");
			if (BerserkerEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Berserker Epic Boots , creating it ...");
				BerserkerEpicBoots = new ItemTemplate();
				BerserkerEpicBoots.KeyName = "BerserkerEpicBoots";
				BerserkerEpicBoots.Name = "Courage Bound Boots";
				BerserkerEpicBoots.Level = 50;
				BerserkerEpicBoots.ItemType = 23;
				BerserkerEpicBoots.Model = 755;
				BerserkerEpicBoots.IsDropable = true;
				BerserkerEpicBoots.IsPickable = true;
				BerserkerEpicBoots.DPS_AF = 100;
				BerserkerEpicBoots.SPD_ABS = 19;
				BerserkerEpicBoots.ObjectType = 34;
				BerserkerEpicBoots.Quality = 100;
				BerserkerEpicBoots.Weight = 22;
				BerserkerEpicBoots.ItemBonus = 35;
				BerserkerEpicBoots.MaxCondition = 50000;
				BerserkerEpicBoots.MaxDurability = 50000;
				BerserkerEpicBoots.Condition = 50000;
				BerserkerEpicBoots.Durability = 50000;

				BerserkerEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.DEX, BonusValue = 19 });

				BerserkerEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 15 });

				BerserkerEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Spirit, BonusValue = 8 });

				BerserkerEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Energy, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(BerserkerEpicBoots);
				}

			}
//end item
			BerserkerEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "BerserkerEpicHelm");
			if (BerserkerEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Berserker Epic Helm , creating it ...");
				BerserkerEpicHelm = new ItemTemplate();
				BerserkerEpicHelm.KeyName = "BerserkerEpicHelm";
				BerserkerEpicHelm.Name = "Courage Bound Helm";
				BerserkerEpicHelm.Level = 50;
				BerserkerEpicHelm.ItemType = 21;
				BerserkerEpicHelm.Model = 829; //NEED TO WORK ON..
				BerserkerEpicHelm.IsDropable = true;
				BerserkerEpicHelm.IsPickable = true;
				BerserkerEpicHelm.DPS_AF = 100;
				BerserkerEpicHelm.SPD_ABS = 19;
				BerserkerEpicHelm.ObjectType = 34;
				BerserkerEpicHelm.Quality = 100;
				BerserkerEpicHelm.Weight = 22;
				BerserkerEpicHelm.ItemBonus = 35;
				BerserkerEpicHelm.MaxCondition = 50000;
				BerserkerEpicHelm.MaxDurability = 50000;
				BerserkerEpicHelm.Condition = 50000;
				BerserkerEpicHelm.Durability = 50000;

				BerserkerEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 10 });

				BerserkerEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 15 });

				BerserkerEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 10 });

				BerserkerEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.QUI, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(BerserkerEpicHelm);
				}
			}
//end item
			BerserkerEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "BerserkerEpicGloves");
			if (BerserkerEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Berserker Epic Gloves , creating it ...");
				BerserkerEpicGloves = new ItemTemplate();
				BerserkerEpicGloves.KeyName = "BerserkerEpicGloves";
				BerserkerEpicGloves.Name = "Courage Bound Gloves";
				BerserkerEpicGloves.Level = 50;
				BerserkerEpicGloves.ItemType = 22;
				BerserkerEpicGloves.Model = 754;
				BerserkerEpicGloves.IsDropable = true;
				BerserkerEpicGloves.IsPickable = true;
				BerserkerEpicGloves.DPS_AF = 100;
				BerserkerEpicGloves.SPD_ABS = 19;
				BerserkerEpicGloves.ObjectType = 34;
				BerserkerEpicGloves.Quality = 100;
				BerserkerEpicGloves.Weight = 22;
				BerserkerEpicGloves.ItemBonus = 35;
				BerserkerEpicGloves.MaxCondition = 50000;
				BerserkerEpicGloves.MaxDurability = 50000;
				BerserkerEpicGloves.Condition = 50000;
				BerserkerEpicGloves.Durability = 50000;

				BerserkerEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Left_Axe, BonusValue = 3 });

				BerserkerEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eProperty.Skill_Parry, BonusValue = 3 });

				BerserkerEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.STR, BonusValue = 12 });

				BerserkerEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 33 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(BerserkerEpicGloves);
				}
			}

			BerserkerEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "BerserkerEpicVest");
			if (BerserkerEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Berserker Epic Vest , creating it ...");
				BerserkerEpicVest = new ItemTemplate();
				BerserkerEpicVest.KeyName = "BerserkerEpicVest";
				BerserkerEpicVest.Name = "Courage Bound Jerkin";
				BerserkerEpicVest.Level = 50;
				BerserkerEpicVest.ItemType = 25;
				BerserkerEpicVest.Model = 751;
				BerserkerEpicVest.IsDropable = true;
				BerserkerEpicVest.IsPickable = true;
				BerserkerEpicVest.DPS_AF = 100;
				BerserkerEpicVest.SPD_ABS = 19;
				BerserkerEpicVest.ObjectType = 34;
				BerserkerEpicVest.Quality = 100;
				BerserkerEpicVest.Weight = 22;
				BerserkerEpicVest.ItemBonus = 35;
				BerserkerEpicVest.MaxCondition = 50000;
				BerserkerEpicVest.MaxDurability = 50000;
				BerserkerEpicVest.Condition = 50000;
				BerserkerEpicVest.Durability = 50000;

				BerserkerEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				BerserkerEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 13 });

				BerserkerEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Body, BonusValue = 6 });

				BerserkerEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(BerserkerEpicVest);
				}
			}

			BerserkerEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "BerserkerEpicLegs");
			if (BerserkerEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Berserker Epic Legs , creating it ...");
				BerserkerEpicLegs = new ItemTemplate();
				BerserkerEpicLegs.KeyName = "BerserkerEpicLegs";
				BerserkerEpicLegs.Name = "Courage Bound Leggings";
				BerserkerEpicLegs.Level = 50;
				BerserkerEpicLegs.ItemType = 27;
				BerserkerEpicLegs.Model = 752;
				BerserkerEpicLegs.IsDropable = true;
				BerserkerEpicLegs.IsPickable = true;
				BerserkerEpicLegs.DPS_AF = 100;
				BerserkerEpicLegs.SPD_ABS = 19;
				BerserkerEpicLegs.ObjectType = 34;
				BerserkerEpicLegs.Quality = 100;
				BerserkerEpicLegs.Weight = 22;
				BerserkerEpicLegs.ItemBonus = 35;
				BerserkerEpicLegs.MaxCondition = 50000;
				BerserkerEpicLegs.MaxDurability = 50000;
				BerserkerEpicLegs.Condition = 50000;
				BerserkerEpicLegs.Durability = 50000;

				BerserkerEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 15 });

				BerserkerEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 15 });

				BerserkerEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 7 });

				BerserkerEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Slash, BonusValue = 12 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(BerserkerEpicLegs);
				}
			}

			BerserkerEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "BerserkerEpicArms");
			if (BerserkerEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Berserker Epic Arms , creating it ...");
				BerserkerEpicArms = new ItemTemplate();
				BerserkerEpicArms.KeyName = "BerserkerEpicArms";
				BerserkerEpicArms.Name = "Courage Bound Sleeves";
				BerserkerEpicArms.Level = 50;
				BerserkerEpicArms.ItemType = 28;
				BerserkerEpicArms.Model = 753;
				BerserkerEpicArms.IsDropable = true;
				BerserkerEpicArms.IsPickable = true;
				BerserkerEpicArms.DPS_AF = 100;
				BerserkerEpicArms.SPD_ABS = 19;
				BerserkerEpicArms.ObjectType = 34;
				BerserkerEpicArms.Quality = 100;
				BerserkerEpicArms.Weight = 22;
				BerserkerEpicArms.ItemBonus = 35;
				BerserkerEpicArms.MaxCondition = 50000;
				BerserkerEpicArms.MaxDurability = 50000;
				BerserkerEpicArms.Condition = 50000;
				BerserkerEpicArms.Durability = 50000;

				BerserkerEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 19 });

				BerserkerEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 15 });

				BerserkerEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Thrust, BonusValue = 8 });

				BerserkerEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Heat, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(BerserkerEpicArms);
				}

			}
			ThaneEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ThaneEpicBoots");
			if (ThaneEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Thane Epic Boots , creating it ...");
				ThaneEpicBoots = new ItemTemplate();
				ThaneEpicBoots.KeyName = "ThaneEpicBoots";
				ThaneEpicBoots.Name = "Storm Touched Boots";
				ThaneEpicBoots.Level = 50;
				ThaneEpicBoots.ItemType = 23;
				ThaneEpicBoots.Model = 791;
				ThaneEpicBoots.IsDropable = true;
				ThaneEpicBoots.IsPickable = true;
				ThaneEpicBoots.DPS_AF = 100;
				ThaneEpicBoots.SPD_ABS = 27;
				ThaneEpicBoots.ObjectType = 35;
				ThaneEpicBoots.Quality = 100;
				ThaneEpicBoots.Weight = 22;
				ThaneEpicBoots.ItemBonus = 35;
				ThaneEpicBoots.MaxCondition = 50000;
				ThaneEpicBoots.MaxDurability = 50000;
				ThaneEpicBoots.Condition = 50000;
				ThaneEpicBoots.Durability = 50000;

				ThaneEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 13 });

				ThaneEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 13 });

				ThaneEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.QUI, BonusValue = 13 });

				ThaneEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Matter, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ThaneEpicBoots);
				}

			}
//end item
			ThaneEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ThaneEpicHelm");
			if (ThaneEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Thane Epic Helm , creating it ...");
				ThaneEpicHelm = new ItemTemplate();
				ThaneEpicHelm.KeyName = "ThaneEpicHelm";
				ThaneEpicHelm.Name = "Storm Touched Coif";
				ThaneEpicHelm.Level = 50;
				ThaneEpicHelm.ItemType = 21;
				ThaneEpicHelm.Model = 834;
				ThaneEpicHelm.IsDropable = true;
				ThaneEpicHelm.IsPickable = true;
				ThaneEpicHelm.DPS_AF = 100;
				ThaneEpicHelm.SPD_ABS = 27;
				ThaneEpicHelm.ObjectType = 35;
				ThaneEpicHelm.Quality = 100;
				ThaneEpicHelm.Weight = 22;
				ThaneEpicHelm.ItemBonus = 35;
				ThaneEpicHelm.MaxCondition = 50000;
				ThaneEpicHelm.MaxDurability = 50000;
				ThaneEpicHelm.Condition = 50000;
				ThaneEpicHelm.Durability = 50000;

				ThaneEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Stormcalling, BonusValue = 4 });

				ThaneEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 18 });

				ThaneEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Spirit, BonusValue = 4 });

				ThaneEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.PowerRegenerationRate, BonusValue = 6 });


				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ThaneEpicHelm);
				}

			}
//end item
			ThaneEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ThaneEpicGloves");
			if (ThaneEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Thane Epic Gloves , creating it ...");
				ThaneEpicGloves = new ItemTemplate();
				ThaneEpicGloves.KeyName = "ThaneEpicGloves";
				ThaneEpicGloves.Name = "Storm Touched Gloves";
				ThaneEpicGloves.Level = 50;
				ThaneEpicGloves.ItemType = 22;
				ThaneEpicGloves.Model = 790;
				ThaneEpicGloves.IsDropable = true;
				ThaneEpicGloves.IsPickable = true;
				ThaneEpicGloves.DPS_AF = 100;
				ThaneEpicGloves.SPD_ABS = 27;
				ThaneEpicGloves.ObjectType = 35;
				ThaneEpicGloves.Quality = 100;
				ThaneEpicGloves.Weight = 22;
				ThaneEpicGloves.ItemBonus = 35;
				ThaneEpicGloves.MaxCondition = 50000;
				ThaneEpicGloves.MaxDurability = 50000;
				ThaneEpicGloves.Condition = 50000;
				ThaneEpicGloves.Durability = 50000;

				ThaneEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Sword, BonusValue = 3 });

				ThaneEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eProperty.Skill_Hammer, BonusValue = 3 });

				ThaneEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.Skill_Axe, BonusValue = 3 });

				ThaneEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.STR, BonusValue = 19 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ThaneEpicGloves);
				}

			}

			ThaneEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ThaneEpicVest");
			if (ThaneEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Thane Epic Vest , creating it ...");
				ThaneEpicVest = new ItemTemplate();
				ThaneEpicVest.KeyName = "ThaneEpicVest";
				ThaneEpicVest.Name = "Storm Touched Hauberk";
				ThaneEpicVest.Level = 50;
				ThaneEpicVest.ItemType = 25;
				ThaneEpicVest.Model = 787;
				ThaneEpicVest.IsDropable = true;
				ThaneEpicVest.IsPickable = true;
				ThaneEpicVest.DPS_AF = 100;
				ThaneEpicVest.SPD_ABS = 27;
				ThaneEpicVest.ObjectType = 35;
				ThaneEpicVest.Quality = 100;
				ThaneEpicVest.Weight = 22;
				ThaneEpicVest.ItemBonus = 35;
				ThaneEpicVest.MaxCondition = 50000;
				ThaneEpicVest.MaxDurability = 50000;
				ThaneEpicVest.Condition = 50000;
				ThaneEpicVest.Durability = 50000;

				ThaneEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				ThaneEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 13 });

				ThaneEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Slash, BonusValue = 6 });

				ThaneEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });


				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ThaneEpicVest);
				}
			}

			ThaneEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ThaneEpicLegs");
			if (ThaneEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Thane Epic Legs , creating it ...");
				ThaneEpicLegs = new ItemTemplate();
				ThaneEpicLegs.KeyName = "ThaneEpicLegs";
				ThaneEpicLegs.Name = "Storm Touched Legs";
				ThaneEpicLegs.Level = 50;
				ThaneEpicLegs.ItemType = 27;
				ThaneEpicLegs.Model = 788;
				ThaneEpicLegs.IsDropable = true;
				ThaneEpicLegs.IsPickable = true;
				ThaneEpicLegs.DPS_AF = 100;
				ThaneEpicLegs.SPD_ABS = 27;
				ThaneEpicLegs.ObjectType = 35;
				ThaneEpicLegs.Quality = 100;
				ThaneEpicLegs.Weight = 22;
				ThaneEpicLegs.ItemBonus = 35;
				ThaneEpicLegs.MaxCondition = 50000;
				ThaneEpicLegs.MaxDurability = 50000;
				ThaneEpicLegs.Condition = 50000;
				ThaneEpicLegs.Durability = 50000;

				ThaneEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 19 });

				ThaneEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.PIE, BonusValue = 15 });

				ThaneEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Crush, BonusValue = 8 });

				ThaneEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Heat, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ThaneEpicLegs);
				}
			}

			ThaneEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ThaneEpicArms");
			if (ThaneEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Thane Epic Arms , creating it ...");
				ThaneEpicArms = new ItemTemplate();
				ThaneEpicArms.KeyName = "ThaneEpicArms";
				ThaneEpicArms.Name = "Storm Touched Sleeves";
				ThaneEpicArms.Level = 50;
				ThaneEpicArms.ItemType = 28;
				ThaneEpicArms.Model = 789;
				ThaneEpicArms.IsDropable = true;
				ThaneEpicArms.IsPickable = true;
				ThaneEpicArms.DPS_AF = 100;
				ThaneEpicArms.SPD_ABS = 27;
				ThaneEpicArms.ObjectType = 35;
				ThaneEpicArms.Quality = 100;
				ThaneEpicArms.Weight = 22;
				ThaneEpicArms.ItemBonus = 35;
				ThaneEpicArms.MaxCondition = 50000;
				ThaneEpicArms.MaxDurability = 50000;
				ThaneEpicArms.Condition = 50000;
				ThaneEpicArms.Durability = 50000;

				ThaneEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 18 });

				ThaneEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 16 });

				ThaneEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Thrust, BonusValue = 8 });

				ThaneEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Body, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ThaneEpicArms);
				}
			}
			//Valhalla Touched Boots
			SkaldEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SkaldEpicBoots");
			if (SkaldEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Skalds Epic Boots , creating it ...");
				SkaldEpicBoots = new ItemTemplate();
				SkaldEpicBoots.KeyName = "SkaldEpicBoots";
				SkaldEpicBoots.Name = "Battlesung Boots";
				SkaldEpicBoots.Level = 50;
				SkaldEpicBoots.ItemType = 23;
				SkaldEpicBoots.Model = 775;
				SkaldEpicBoots.IsDropable = true;
				SkaldEpicBoots.IsPickable = true;
				SkaldEpicBoots.DPS_AF = 100;
				SkaldEpicBoots.SPD_ABS = 27;
				SkaldEpicBoots.ObjectType = 35;
				SkaldEpicBoots.Quality = 100;
				SkaldEpicBoots.Weight = 22;
				SkaldEpicBoots.ItemBonus = 35;
				SkaldEpicBoots.MaxCondition = 50000;
				SkaldEpicBoots.MaxDurability = 50000;
				SkaldEpicBoots.Condition = 50000;
				SkaldEpicBoots.Durability = 50000;

				SkaldEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 13 });

				SkaldEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 13 });

				SkaldEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Cold, BonusValue = 10 });

				SkaldEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 24 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SkaldEpicBoots);
				}
			}
//end item
			//Valhalla Touched Coif 
			SkaldEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SkaldEpicHelm");
			if (SkaldEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Skalds Epic Helm , creating it ...");
				SkaldEpicHelm = new ItemTemplate();
				SkaldEpicHelm.KeyName = "SkaldEpicHelm";
				SkaldEpicHelm.Name = "Battlesung Coif";
				SkaldEpicHelm.Level = 50;
				SkaldEpicHelm.ItemType = 21;
				SkaldEpicHelm.Model = 832; //NEED TO WORK ON..
				SkaldEpicHelm.IsDropable = true;
				SkaldEpicHelm.IsPickable = true;
				SkaldEpicHelm.DPS_AF = 100;
				SkaldEpicHelm.SPD_ABS = 27;
				SkaldEpicHelm.ObjectType = 35;
				SkaldEpicHelm.Quality = 100;
				SkaldEpicHelm.Weight = 22;
				SkaldEpicHelm.ItemBonus = 35;
				SkaldEpicHelm.MaxCondition = 50000;
				SkaldEpicHelm.MaxDurability = 50000;
				SkaldEpicHelm.Condition = 50000;
				SkaldEpicHelm.Durability = 50000;

				SkaldEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Battlesongs, BonusValue = 5 });

				SkaldEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CHR, BonusValue = 15 });

				SkaldEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 33 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SkaldEpicHelm);
				}
			}
//end item
			//Valhalla Touched Gloves 
			SkaldEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SkaldEpicGloves");
			if (SkaldEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Skalds Epic Gloves , creating it ...");
				SkaldEpicGloves = new ItemTemplate();
				SkaldEpicGloves.KeyName = "SkaldEpicGloves";
				SkaldEpicGloves.Name = "Battlesung Gloves";
				SkaldEpicGloves.Level = 50;
				SkaldEpicGloves.ItemType = 22;
				SkaldEpicGloves.Model = 774;
				SkaldEpicGloves.IsDropable = true;
				SkaldEpicGloves.IsPickable = true;
				SkaldEpicGloves.DPS_AF = 100;
				SkaldEpicGloves.SPD_ABS = 27;
				SkaldEpicGloves.ObjectType = 35;
				SkaldEpicGloves.Quality = 100;
				SkaldEpicGloves.Weight = 22;
				SkaldEpicGloves.ItemBonus = 35;
				SkaldEpicGloves.MaxCondition = 50000;
				SkaldEpicGloves.MaxDurability = 50000;
				SkaldEpicGloves.Condition = 50000;
				SkaldEpicGloves.Durability = 50000;

				SkaldEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 18 });

				SkaldEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 15 });

				SkaldEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Body, BonusValue = 8 });

				SkaldEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Energy, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SkaldEpicGloves);
				}

			}
			//Valhalla Touched Hauberk 
			SkaldEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SkaldEpicVest");
			if (SkaldEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Skalds Epic Vest , creating it ...");
				SkaldEpicVest = new ItemTemplate();
				SkaldEpicVest.KeyName = "SkaldEpicVest";
				SkaldEpicVest.Name = "Battlesung Hauberk";
				SkaldEpicVest.Level = 50;
				SkaldEpicVest.ItemType = 25;
				SkaldEpicVest.Model = 771;
				SkaldEpicVest.IsDropable = true;
				SkaldEpicVest.IsPickable = true;
				SkaldEpicVest.DPS_AF = 100;
				SkaldEpicVest.SPD_ABS = 27;
				SkaldEpicVest.ObjectType = 35;
				SkaldEpicVest.Quality = 100;
				SkaldEpicVest.Weight = 22;
				SkaldEpicVest.ItemBonus = 35;
				SkaldEpicVest.MaxCondition = 50000;
				SkaldEpicVest.MaxDurability = 50000;
				SkaldEpicVest.Condition = 50000;
				SkaldEpicVest.Durability = 50000;

				SkaldEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				SkaldEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 13 });

				SkaldEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.CHR, BonusValue = 13 });

				SkaldEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Matter, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SkaldEpicVest);
				}
			}
			//Valhalla Touched Legs 
			SkaldEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SkaldEpicLegs");
			if (SkaldEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Skalds Epic Legs , creating it ...");
				SkaldEpicLegs = new ItemTemplate();
				SkaldEpicLegs.KeyName = "SkaldEpicLegs";
				SkaldEpicLegs.Name = "Battlesung Legs";
				SkaldEpicLegs.Level = 50;
				SkaldEpicLegs.ItemType = 27;
				SkaldEpicLegs.Model = 772;
				SkaldEpicLegs.IsDropable = true;
				SkaldEpicLegs.IsPickable = true;
				SkaldEpicLegs.DPS_AF = 100;
				SkaldEpicLegs.SPD_ABS = 27;
				SkaldEpicLegs.ObjectType = 35;
				SkaldEpicLegs.Quality = 100;
				SkaldEpicLegs.Weight = 22;
				SkaldEpicLegs.ItemBonus = 35;
				SkaldEpicLegs.MaxCondition = 50000;
				SkaldEpicLegs.MaxDurability = 50000;
				SkaldEpicLegs.Condition = 50000;
				SkaldEpicLegs.Durability = 50000;

				SkaldEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				SkaldEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 13 });

				SkaldEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Spirit, BonusValue = 8 });

				SkaldEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 27 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SkaldEpicLegs);
				}
			}
			//Valhalla Touched Sleeves 
			SkaldEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SkaldEpicArms");
			if (SkaldEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Skald Epic Arms , creating it ...");
				SkaldEpicArms = new ItemTemplate();
				SkaldEpicArms.KeyName = "SkaldEpicArms";
				SkaldEpicArms.Name = "Battlesung Sleeves";
				SkaldEpicArms.Level = 50;
				SkaldEpicArms.ItemType = 28;
				SkaldEpicArms.Model = 773;
				SkaldEpicArms.IsDropable = true;
				SkaldEpicArms.IsPickable = true;
				SkaldEpicArms.DPS_AF = 100;
				SkaldEpicArms.SPD_ABS = 27;
				SkaldEpicArms.ObjectType = 35;
				SkaldEpicArms.Quality = 100;
				SkaldEpicArms.Weight = 22;
				SkaldEpicArms.ItemBonus = 35;
				SkaldEpicArms.MaxCondition = 50000;
				SkaldEpicArms.MaxDurability = 50000;
				SkaldEpicArms.Condition = 50000;
				SkaldEpicArms.Durability = 50000;

				SkaldEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 16 });

				SkaldEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 15 });

				SkaldEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Thrust, BonusValue = 10 });

				SkaldEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Cold, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SkaldEpicArms);
				}
			}
			//Subterranean Boots 
			SavageEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SavageEpicBoots");
			if (SavageEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Savage Epic Boots , creating it ...");
				SavageEpicBoots = new ItemTemplate();
				SavageEpicBoots.KeyName = "SavageEpicBoots";
				SavageEpicBoots.Name = "Kelgor's Battle Boots";
				SavageEpicBoots.Level = 50;
				SavageEpicBoots.ItemType = 23;
				SavageEpicBoots.Model = 1196;
				SavageEpicBoots.IsDropable = true;
				SavageEpicBoots.IsPickable = true;
				SavageEpicBoots.DPS_AF = 100;
				SavageEpicBoots.SPD_ABS = 27;
				SavageEpicBoots.ObjectType = 34;
				SavageEpicBoots.Quality = 100;
				SavageEpicBoots.Weight = 22;
				SavageEpicBoots.ItemBonus = 35;
				SavageEpicBoots.MaxCondition = 50000;
				SavageEpicBoots.MaxDurability = 50000;
				SavageEpicBoots.Condition = 50000;
				SavageEpicBoots.Durability = 50000;

				SavageEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 15 });

				SavageEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 19 });

				SavageEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Matter, BonusValue = 8 });

				SavageEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Energy, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SavageEpicBoots);
				}
			}
			//Subterranean Coif 
			SavageEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SavageEpicHelm");
			if (SavageEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Savage Epic Helm , creating it ...");
				SavageEpicHelm = new ItemTemplate();
				SavageEpicHelm.KeyName = "SavageEpicHelm";
				SavageEpicHelm.Name = "Kelgor's Battle Helm";
				SavageEpicHelm.Level = 50;
				SavageEpicHelm.ItemType = 21;
				SavageEpicHelm.Model = 831; //NEED TO WORK ON..
				SavageEpicHelm.IsDropable = true;
				SavageEpicHelm.IsPickable = true;
				SavageEpicHelm.DPS_AF = 100;
				SavageEpicHelm.SPD_ABS = 19;
				SavageEpicHelm.ObjectType = 34;
				SavageEpicHelm.Quality = 100;
				SavageEpicHelm.Weight = 22;
				SavageEpicHelm.ItemBonus = 35;
				SavageEpicHelm.MaxCondition = 50000;
				SavageEpicHelm.MaxDurability = 50000;
				SavageEpicHelm.Condition = 50000;
				SavageEpicHelm.Durability = 50000;

				SavageEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 15 });

				SavageEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 10 });

				SavageEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 10 });

				SavageEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.QUI, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SavageEpicHelm);
				}
			}
			//Subterranean Gloves 
			SavageEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SavageEpicGloves");
			if (SavageEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Savage Epic Gloves , creating it ...");
				SavageEpicGloves = new ItemTemplate();
				SavageEpicGloves.KeyName = "SavageEpicGloves";
				SavageEpicGloves.Name = "Kelgor's Battle Gauntlets";
				SavageEpicGloves.Level = 50;
				SavageEpicGloves.ItemType = 22;
				SavageEpicGloves.Model = 1195;
				SavageEpicGloves.IsDropable = true;
				SavageEpicGloves.IsPickable = true;
				SavageEpicGloves.DPS_AF = 100;
				SavageEpicGloves.SPD_ABS = 19;
				SavageEpicGloves.ObjectType = 34;
				SavageEpicGloves.Quality = 100;
				SavageEpicGloves.Weight = 22;
				SavageEpicGloves.ItemBonus = 35;
				SavageEpicGloves.MaxCondition = 50000;
				SavageEpicGloves.MaxDurability = 50000;
				SavageEpicGloves.Condition = 50000;
				SavageEpicGloves.Durability = 50000;

				SavageEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Parry, BonusValue = 3 });

				SavageEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 12 });

				SavageEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 33 });

				SavageEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.Skill_HandToHand, BonusValue = 3 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SavageEpicGloves);
				}
			}
			//Subterranean Hauberk 
			SavageEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SavageEpicVest");
			if (SavageEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Savage Epic Vest , creating it ...");
				SavageEpicVest = new ItemTemplate();
				SavageEpicVest.KeyName = "SavageEpicVest";
				SavageEpicVest.Name = "Kelgor's Battle Vest";
				SavageEpicVest.Level = 50;
				SavageEpicVest.ItemType = 25;
				SavageEpicVest.Model = 1192;
				SavageEpicVest.IsDropable = true;
				SavageEpicVest.IsPickable = true;
				SavageEpicVest.DPS_AF = 100;
				SavageEpicVest.SPD_ABS = 19;
				SavageEpicVest.ObjectType = 34;
				SavageEpicVest.Quality = 100;
				SavageEpicVest.Weight = 22;
				SavageEpicVest.ItemBonus = 35;
				SavageEpicVest.MaxCondition = 50000;
				SavageEpicVest.MaxDurability = 50000;
				SavageEpicVest.Condition = 50000;
				SavageEpicVest.Durability = 50000;

				SavageEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				SavageEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 13 });

				SavageEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Slash, BonusValue = 6 });

				SavageEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SavageEpicVest);
				}
			}
			//Subterranean Legs 
			SavageEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SavageEpicLegs");
			if (SavageEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Savage Epic Legs , creating it ...");
				SavageEpicLegs = new ItemTemplate();
				SavageEpicLegs.KeyName = "SavageEpicLegs";
				SavageEpicLegs.Name = "Kelgor's Battle Leggings";
				SavageEpicLegs.Level = 50;
				SavageEpicLegs.ItemType = 27;
				SavageEpicLegs.Model = 1193;
				SavageEpicLegs.IsDropable = true;
				SavageEpicLegs.IsPickable = true;
				SavageEpicLegs.DPS_AF = 100;
				SavageEpicLegs.SPD_ABS = 19;
				SavageEpicLegs.ObjectType = 34;
				SavageEpicLegs.Quality = 100;
				SavageEpicLegs.Weight = 22;
				SavageEpicLegs.ItemBonus = 35;
				SavageEpicLegs.MaxCondition = 50000;
				SavageEpicLegs.MaxDurability = 50000;
				SavageEpicLegs.Condition = 50000;
				SavageEpicLegs.Durability = 50000;

				SavageEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eResist.Heat, BonusValue = 12 });

				SavageEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 7 });

				SavageEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 15 });

				SavageEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.QUI, BonusValue = 15 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SavageEpicLegs);
				}
			}
			//Subterranean Sleeves 
			SavageEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "SavageEpicArms");
			if (SavageEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Savage Epic Arms , creating it ...");
				SavageEpicArms = new ItemTemplate();
				SavageEpicArms.KeyName = "SavageEpicArms";
				SavageEpicArms.Name = "Kelgor's Battle Sleeves";
				SavageEpicArms.Level = 50;
				SavageEpicArms.ItemType = 28;
				SavageEpicArms.Model = 1194;
				SavageEpicArms.IsDropable = true;
				SavageEpicArms.IsPickable = true;
				SavageEpicArms.DPS_AF = 100;
				SavageEpicArms.SPD_ABS = 19;
				SavageEpicArms.ObjectType = 34;
				SavageEpicArms.Quality = 100;
				SavageEpicArms.Weight = 22;
				SavageEpicArms.ItemBonus = 35;
				SavageEpicArms.MaxCondition = 50000;
				SavageEpicArms.MaxDurability = 50000;
				SavageEpicArms.Condition = 50000;
				SavageEpicArms.Durability = 50000;

				SavageEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 19 });

				SavageEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 15 });

				SavageEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Cold, BonusValue = 8 });

				SavageEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Heat, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(SavageEpicArms);
				}

			}
			#region Valkyrie
			ValkyrieEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ValkyrieEpicBoots");
			if (ValkyrieEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Valkyrie Epic Boots , creating it ...");
				ValkyrieEpicBoots = new ItemTemplate();
				ValkyrieEpicBoots.KeyName = "ValkyrieEpicBoots";
				ValkyrieEpicBoots.Name = "Battle Maiden's Boots";
				ValkyrieEpicBoots.Level = 50;
				ValkyrieEpicBoots.ItemType = 23;
				ValkyrieEpicBoots.Model = 2932;
				ValkyrieEpicBoots.IsDropable = true;
				ValkyrieEpicBoots.IsPickable = true;
				ValkyrieEpicBoots.DPS_AF = 100;
				ValkyrieEpicBoots.SPD_ABS = 27;
				ValkyrieEpicBoots.ObjectType = 35;
				ValkyrieEpicBoots.Quality = 100;
				ValkyrieEpicBoots.Weight = 22;
				ValkyrieEpicBoots.ItemBonus = 35;
				ValkyrieEpicBoots.MaxCondition = 50000;
				ValkyrieEpicBoots.MaxDurability = 50000;
				ValkyrieEpicBoots.Durability = 50000;
				ValkyrieEpicBoots.Condition = 50000;

				/*
				 *   Constitution: 7 pts
				 *   Dexterity: 13 pts
				 *   Quickness: 13 pts
				 *   Body Resist: 8%
				 */

				ValkyrieEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 7 });

				ValkyrieEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 13 });

				ValkyrieEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.QUI, BonusValue = 13 });

				ValkyrieEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Body, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ValkyrieEpicBoots);
				}

			}
			//end item
			ValkyrieEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ValkyrieEpicHelm");
			if (ValkyrieEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Valkyrie Epic Helm , creating it ...");
				ValkyrieEpicHelm = new ItemTemplate();
				ValkyrieEpicHelm.KeyName = "ValkyrieEpicHelm";
				ValkyrieEpicHelm.Name = "Battle Maiden's Coif";
				ValkyrieEpicHelm.Level = 50;
				ValkyrieEpicHelm.ItemType = 21;
				ValkyrieEpicHelm.Model = 2951; //NEED TO WORK ON..
				ValkyrieEpicHelm.IsDropable = true;
				ValkyrieEpicHelm.IsPickable = true;
				ValkyrieEpicHelm.DPS_AF = 100;
				ValkyrieEpicHelm.SPD_ABS = 27;
				ValkyrieEpicHelm.ObjectType = 35;
				ValkyrieEpicHelm.Quality = 100;
				ValkyrieEpicHelm.Weight = 22;
				ValkyrieEpicHelm.ItemBonus = 35;
				ValkyrieEpicHelm.MaxCondition = 50000;
				ValkyrieEpicHelm.MaxDurability = 50000;
				ValkyrieEpicHelm.Condition = 50000;
				ValkyrieEpicHelm.Durability = 50000;

				/*
				 *   Sword: +4 pts
				 *   Constitution: 18 pts
				 *   Cold Resist: 4%
				 *   Energy Resist: 6%
				 */

				ValkyrieEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Sword, BonusValue = 4 });

				ValkyrieEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 18 });

				ValkyrieEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Cold, BonusValue = 4 });

				ValkyrieEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Energy, BonusValue = 6 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ValkyrieEpicHelm);
				}

			}
			//end item
			ValkyrieEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ValkyrieEpicGloves");
			if (ValkyrieEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Valkyrie Epic Gloves , creating it ...");
				ValkyrieEpicGloves = new ItemTemplate();
				ValkyrieEpicGloves.KeyName = "ValkyrieEpicGloves";
				ValkyrieEpicGloves.Name = "Battle Maiden's Gloves";
				ValkyrieEpicGloves.Level = 50;
				ValkyrieEpicGloves.ItemType = 22;
				ValkyrieEpicGloves.Model = 2931;
				ValkyrieEpicGloves.IsDropable = true;
				ValkyrieEpicGloves.IsPickable = true;
				ValkyrieEpicGloves.DPS_AF = 100;
				ValkyrieEpicGloves.SPD_ABS = 27;
				ValkyrieEpicGloves.ObjectType = 35;
				ValkyrieEpicGloves.Quality = 100;
				ValkyrieEpicGloves.Weight = 22;
				ValkyrieEpicGloves.ItemBonus = 35;
				ValkyrieEpicGloves.MaxCondition = 50000;
				ValkyrieEpicGloves.MaxDurability = 50000;
				ValkyrieEpicGloves.Condition = 50000;
				ValkyrieEpicGloves.Durability = 50000;

				/*
				 *   Spear: +3 pts
				 *   Parry: +3 pts
				 *   Strength: 19 pts
				 *   Odin's Will: +3 pts
				 */

				ValkyrieEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Spear, BonusValue = 3 });

				ValkyrieEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eProperty.Skill_Parry, BonusValue = 3 });

				ValkyrieEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.STR, BonusValue = 19 });

				ValkyrieEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.Skill_OdinsWill, BonusValue = 3 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ValkyrieEpicGloves);
				}

			}

			ValkyrieEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ValkyrieEpicVest");
			if (ValkyrieEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Valkyrie Epic Vest , creating it ...");
				ValkyrieEpicVest = new ItemTemplate();
				ValkyrieEpicVest.KeyName = "ValkyrieEpicVest";
				ValkyrieEpicVest.Name = "Battle Maiden's Hauberk";
				ValkyrieEpicVest.Level = 50;
				ValkyrieEpicVest.ItemType = 25;
				ValkyrieEpicVest.Model = 2928;
				ValkyrieEpicVest.IsDropable = true;
				ValkyrieEpicVest.IsPickable = true;
				ValkyrieEpicVest.DPS_AF = 100;
				ValkyrieEpicVest.SPD_ABS = 27;
				ValkyrieEpicVest.ObjectType = 35;
				ValkyrieEpicVest.Quality = 100;
				ValkyrieEpicVest.Weight = 22;
				ValkyrieEpicVest.ItemBonus = 35;
				ValkyrieEpicVest.MaxCondition = 50000;
				ValkyrieEpicVest.MaxDurability = 50000;
				ValkyrieEpicVest.Condition = 50000;
				ValkyrieEpicVest.Durability = 50000;

				/*
				 *   Strength: 13 pts
				 *   Constitution: 13 pts
				 *   Slash Resist: 6%
				 *   Hits: 30 pts
				 */

				ValkyrieEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 13 });

				ValkyrieEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 13 });

				ValkyrieEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Matter, BonusValue = 6 });

				ValkyrieEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ValkyrieEpicVest);
				}

			}

			ValkyrieEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ValkyrieEpicLegs");
			if (ValkyrieEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Valkyrie Epic Legs , creating it ...");
				ValkyrieEpicLegs = new ItemTemplate();
				ValkyrieEpicLegs.KeyName = "ValkyrieEpicLegs";
				ValkyrieEpicLegs.Name = "Battle Maiden's Legs";
				ValkyrieEpicLegs.Level = 50;
				ValkyrieEpicLegs.ItemType = 27;
				ValkyrieEpicLegs.Model = 2929;
				ValkyrieEpicLegs.IsDropable = true;
				ValkyrieEpicLegs.IsPickable = true;
				ValkyrieEpicLegs.DPS_AF = 100;
				ValkyrieEpicLegs.SPD_ABS = 27;
				ValkyrieEpicLegs.ObjectType = 35;
				ValkyrieEpicLegs.Quality = 100;
				ValkyrieEpicLegs.Weight = 22;
				ValkyrieEpicLegs.ItemBonus = 35;
				ValkyrieEpicLegs.MaxCondition = 50000;
				ValkyrieEpicLegs.MaxDurability = 50000;
				ValkyrieEpicLegs.Condition = 50000;
				ValkyrieEpicLegs.Durability = 50000;

				/*
				 *   Constitution: 19 pts
				 *   Piety: 15 pts
				 *   Crush Resist: 8%
				 *   Heat Resist: 8%
				 */

				ValkyrieEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 19 });

				ValkyrieEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.PIE, BonusValue = 15 });

				ValkyrieEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Crush, BonusValue = 8 });

				ValkyrieEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Heat, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ValkyrieEpicLegs);
				}

			}

			ValkyrieEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "ValkyrieEpicArms");
			if (ValkyrieEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Valkyrie Epic Arms , creating it ...");
				ValkyrieEpicArms = new ItemTemplate();
				ValkyrieEpicArms.KeyName = "ValkyrieEpicArms";
				ValkyrieEpicArms.Name = "Battle Maiden's Sleeves";
				ValkyrieEpicArms.Level = 50;
				ValkyrieEpicArms.ItemType = 28;
				ValkyrieEpicArms.Model = 2930;
				ValkyrieEpicArms.IsDropable = true;
				ValkyrieEpicArms.IsPickable = true;
				ValkyrieEpicArms.DPS_AF = 100;
				ValkyrieEpicArms.SPD_ABS = 27;
				ValkyrieEpicArms.ObjectType = 35;
				ValkyrieEpicArms.Quality = 100;
				ValkyrieEpicArms.Weight = 22;
				ValkyrieEpicArms.ItemBonus = 35;
				ValkyrieEpicArms.MaxCondition = 50000;
				ValkyrieEpicArms.MaxDurability = 50000;
				ValkyrieEpicArms.Condition = 50000;
				ValkyrieEpicArms.Durability = 50000;

				/*
				 *   Strength: 18 pts
				 *   Quickness: 16 pts
				 *   Thrust Resist: 8%
				 *   Spirit Resist: 8%
				 */

				ValkyrieEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 18 });

				ValkyrieEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 16 });

				ValkyrieEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Thrust, BonusValue = 8 });

				ValkyrieEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Spirit, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(ValkyrieEpicArms);
				}

			}
			#endregion

            // Graveen: we assume items are existing in the DB
            // TODO: insert here creation of items if they do not exists
            MaulerMidEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerMidEpicBoots");
            MaulerMidEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerMidEpicHelm");
            MaulerMidEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerMidEpicGloves");
            MaulerMidEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerMidEpicVest");
            MaulerMidEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerMidEpicLegs");
            MaulerMidEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerMidEpicArms");

//Item Descriptions End

			#endregion

			GameEventMgr.AddHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.AddHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.AddHandler(Lynnleigh, GameObjectEvent.Interact, new DOLEventHandler(TalkToLynnleigh));
			GameEventMgr.AddHandler(Lynnleigh, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToLynnleigh));

			GameEventMgr.AddHandler(Elizabeth, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToElizabeth));
			GameEventMgr.AddHandler(Elizabeth, GameLivingEvent.Interact, new DOLEventHandler(TalkToElizabeth));

			/* Now we bring to Lynnleigh the possibility to give this quest to players */
			Lynnleigh.AddQuestToGive(typeof (Viking_50));

			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initialized");
		}

		[ScriptUnloadedEvent]
		public static void ScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			//if not loaded, don't worry
			if (Lynnleigh == null)
				return;
			// remove handlers
			GameEventMgr.RemoveHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.RemoveHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.RemoveHandler(Lynnleigh, GameObjectEvent.Interact, new DOLEventHandler(TalkToLynnleigh));
			GameEventMgr.RemoveHandler(Lynnleigh, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToLynnleigh));

			GameEventMgr.RemoveHandler(Elizabeth, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToElizabeth));
			GameEventMgr.RemoveHandler(Elizabeth, GameLivingEvent.Interact, new DOLEventHandler(TalkToElizabeth));
		
			/* Now we remove to Lynnleigh the possibility to give this quest to players */
			Lynnleigh.RemoveQuestToGive(typeof (Viking_50));
		}

		protected static void TalkToLynnleigh(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs) args).Source as GamePlayer;
			if (player == null)
				return;

			if(Lynnleigh.CanGiveQuest(typeof (Viking_50), player)  <= 0)
				return;

			// player is not allowed to start this quest until the quest rewards are available
			if (player.CharacterClass.ID == (byte)eCharacterClass.MaulerMid &&
				(MaulerMidEpicArms == null || MaulerMidEpicBoots == null || MaulerMidEpicGloves == null ||
				MaulerMidEpicHelm == null || MaulerMidEpicLegs == null || MaulerMidEpicVest == null))
			{
				Elizabeth.SayTo(player, "This quest is not available to Maulers yet.");
				return;
			}

			//We also check if the player is already doing the quest
			Viking_50 quest = player.IsDoingQuest(typeof (Viking_50)) as Viking_50;

			if (e == GameObjectEvent.Interact)
			{
				if (quest != null)
				{
					Lynnleigh.SayTo(player, "Check your Journal for information about what to do!");
				}
				else
				{
					Lynnleigh.SayTo(player, "Ah, this reveals exactly where Jango and his deserters took Ydenia to dispose of him. He also has a note here about how strong Ydenia really was. That [worries me].");
				}
			}
				// The player whispered to the NPC
			else if (e == GameLivingEvent.WhisperReceive)
			{
				WhisperReceiveEventArgs wArgs = (WhisperReceiveEventArgs) args;

				if (quest == null)
				{
					switch (wArgs.Text)
					{
						case "worries me":
							Lynnleigh.SayTo(player, "Yes, it worries me, but I think that you are ready to [face Ydenia] and his minions.");
							break;
						case "face Ydenia":
							player.Out.SendQuestSubscribeCommand(Lynnleigh, QuestMgr.GetIDForQuestType(typeof(Viking_50)), "Will you face Ydenia [Viking Level 50 Epic]?");
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

		protected static void TalkToElizabeth(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs) args).Source as GamePlayer;
			if (player == null)
				return;

			if(Lynnleigh.CanGiveQuest(typeof (Viking_50), player)  <= 0)
				return;

			//We also check if the player is already doing the quest
			Viking_50 quest = player.IsDoingQuest(typeof (Viking_50)) as Viking_50;

			if (e == GameObjectEvent.Interact)
			{
				if (quest != null)
				{
					switch (quest.Step)
					{
                        case 4:
                            {
                                Elizabeth.SayTo(player, "There are six parts to your reward, so make sure you have room for them. Just let me know when you are ready, and then you can [take them] with our thanks!");
                                break;
                            }

					}
				}
			}
				// The player whispered to the NPC
			else if (e == GameLivingEvent.WhisperReceive)
			{
				WhisperReceiveEventArgs wArgs = (WhisperReceiveEventArgs) args;

				if (quest != null)
				{
					switch (wArgs.Text)
					{
						case "take them":
							if (quest.Step == 4)
								quest.FinishQuest();
							break;
					}
				}
			}
		}

		public override bool CheckQuestQualification(GamePlayer player)
		{
			// if the player is already doing the quest his level is no longer of relevance
			if (player.IsDoingQuest(typeof (Viking_50)) != null)
				return true;

			if (player.CharacterClass.ID != (byte) eCharacterClass.Warrior &&
				player.CharacterClass.ID != (byte) eCharacterClass.Berserker &&
				player.CharacterClass.ID != (byte) eCharacterClass.Thane &&
				player.CharacterClass.ID != (byte) eCharacterClass.Skald &&
				player.CharacterClass.ID != (byte) eCharacterClass.Savage &&
                player.CharacterClass.ID != (byte) eCharacterClass.MaulerMid &&
				player.CharacterClass.ID != (byte) eCharacterClass.Valkyrie)
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
			Viking_50 quest = player.IsDoingQuest(typeof (Viking_50)) as Viking_50;

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

			if (qargs.QuestID != QuestMgr.GetIDForQuestType(typeof(Viking_50)))
				return;

			if (e == GamePlayerEvent.AcceptQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x01);
			else if (e == GamePlayerEvent.DeclineQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x00);
		}

		private static void CheckPlayerAcceptQuest(GamePlayer player, byte response)
		{
			if(Lynnleigh.CanGiveQuest(typeof (Viking_50), player)  <= 0)
				return;

			if (player.IsDoingQuest(typeof (Viking_50)) != null)
				return;

			if (response == 0x00)
			{
				player.Out.SendMessage("Our God forgives your laziness, just look out for stray lightning bolts.", eChatType.CT_Say, eChatLoc.CL_PopupWindow);
			}
			else
			{
				//Check if we can add the quest!
				if (!Lynnleigh.GiveQuest(typeof (Viking_50), player, 1))
					return;

				Lynnleigh.SayTo(player, "Yes, you must face and defeat him! There is a note scrawled in the corner of the map that even in death Ydenia is strong. He has gathered followers to protect him in his spirit state and they will come to his aid if he is attacked. Even though you have improved your skills quite a bit, I would highley recommed taking some friends with you to face Ydenia. It is imperative that you defeat him and obtain the totem he holds if I am to end the spell. According to the map you can find Ydenia in Raumarik. Head to the river in Raumarik and go north. When you reach the end of it, go northwest to the next river. Cross the river and head west. Follow the snowline until you reach a group of trees. That is where you will find Ydenia and his followers. Return to me when you have the totem. May all the gods be with you.");
			}
		}

		//Set quest name
		public override string Name
		{
			get { return "An End to the Daggers (level 50 Viking epic)"; }
		}

		// Define Steps
		public override string Description
		{
			get
			{
				switch (Step)
				{
					case 1:
						return "[Step #1] Seek out Ydenia in Raumarik Loc 48k, 30k kill her!";
					case 2:
						return "[Step #2] Return to Lynnleigh and give her tome of Enchantments!";
					case 3:
						return "[Step #3] Take the Sealed Pouch to Elizabeth in Mularn";
					case 4:
						return "[Step #4] Tell Elizabeth you can 'take them' for your rewards!";
				}
				return base.Description;
			}
		}

		public override void Notify(DOLEvent e, object sender, EventArgs args)
		{
			GamePlayer player = sender as GamePlayer;

			if (player==null || player.IsDoingQuest(typeof (Viking_50)) == null)
				return;

			if (Step == 1 && e == GameLivingEvent.EnemyKilled)
			{
				EnemyKilledEventArgs gArgs = (EnemyKilledEventArgs) args;

				if (gArgs.Target.Name == Ydenia.Name)
				{
					Step = 2;
					GiveItem(m_questPlayer, tome_enchantments);
					m_questPlayer.Out.SendMessage("Ydenia drops the Tome of Enchantments and you pick it up!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
					return;
				}
			}

			if (Step == 2 && e == GamePlayerEvent.GiveItem)
			{
        		GiveItemEventArgs gArgs = (GiveItemEventArgs) args;
				if (gArgs.Target.Name == Lynnleigh.Name && gArgs.Item.ItemTemplate.KeyName == tome_enchantments.KeyName)
				{
					RemoveItem(Lynnleigh, player, tome_enchantments);
					Lynnleigh.SayTo(player, "Take this sealed pouch to Elizabeth in Mularn for your reward!");
					GiveItem(Lynnleigh, player, sealed_pouch);
					Step = 3;
				}
			}

			if (Step == 3 && e == GamePlayerEvent.GiveItem)
			{
				GiveItemEventArgs gArgs = (GiveItemEventArgs) args;
				if (gArgs.Target.Name == Elizabeth.Name && gArgs.Item.ItemTemplate.KeyName == sealed_pouch.KeyName)
				{
					RemoveItem(Elizabeth, player, sealed_pouch);
					Elizabeth.SayTo(player, "There are six parts to your reward, so make sure you have room for them. Just let me know when you are ready, and then you can [take them] with our thanks!");
					Step = 4;
				}
			}
		}

		public override void AbortQuest()
		{
			base.AbortQuest(); //Defined in Quest, changes the state, stores in DB etc ...

			RemoveItem(m_questPlayer, sealed_pouch, false);
			RemoveItem(m_questPlayer, tome_enchantments, false);
		}

		public override void FinishQuest()
		{
			if (m_questPlayer.Inventory.IsSlotsFree(6, eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack))
			{
				base.FinishQuest(); //Defined in Quest, changes the state, stores in DB etc ...

				switch ((eCharacterClass)m_questPlayer.CharacterClass.ID)
				{
					case eCharacterClass.Warrior:
						{
							GiveItem(m_questPlayer, WarriorEpicArms);
							GiveItem(m_questPlayer, WarriorEpicBoots);
							GiveItem(m_questPlayer, WarriorEpicGloves);
							GiveItem(m_questPlayer, WarriorEpicHelm);
							GiveItem(m_questPlayer, WarriorEpicLegs);
							GiveItem(m_questPlayer, WarriorEpicVest);
							break;
						}
					case eCharacterClass.Berserker:
						{
							GiveItem(m_questPlayer, BerserkerEpicArms);
							GiveItem(m_questPlayer, BerserkerEpicBoots);
							GiveItem(m_questPlayer, BerserkerEpicGloves);
							GiveItem(m_questPlayer, BerserkerEpicHelm);
							GiveItem(m_questPlayer, BerserkerEpicLegs);
							GiveItem(m_questPlayer, BerserkerEpicVest);
							break;
						}
					case eCharacterClass.Thane:
						{
							GiveItem(m_questPlayer, ThaneEpicArms);
							GiveItem(m_questPlayer, ThaneEpicBoots);
							GiveItem(m_questPlayer, ThaneEpicGloves);
							GiveItem(m_questPlayer, ThaneEpicHelm);
							GiveItem(m_questPlayer, ThaneEpicLegs);
							GiveItem(m_questPlayer, ThaneEpicVest);
							break;
						}
					case eCharacterClass.Skald:
						{
							GiveItem(m_questPlayer, SkaldEpicArms);
							GiveItem(m_questPlayer, SkaldEpicBoots);
							GiveItem(m_questPlayer, SkaldEpicGloves);
							GiveItem(m_questPlayer, SkaldEpicHelm);
							GiveItem(m_questPlayer, SkaldEpicLegs);
							GiveItem(m_questPlayer, SkaldEpicVest);
							break;
						}
					case eCharacterClass.Savage:
						{
							GiveItem(m_questPlayer, SavageEpicArms);
							GiveItem(m_questPlayer, SavageEpicBoots);
							GiveItem(m_questPlayer, SavageEpicGloves);
							GiveItem(m_questPlayer, SavageEpicHelm);
							GiveItem(m_questPlayer, SavageEpicLegs);
							GiveItem(m_questPlayer, SavageEpicVest);
							break;
						}
					case eCharacterClass.Valkyrie:
						{
							GiveItem(m_questPlayer, ValkyrieEpicArms);
							GiveItem(m_questPlayer, ValkyrieEpicBoots);
							GiveItem(m_questPlayer, ValkyrieEpicGloves);
							GiveItem(m_questPlayer, ValkyrieEpicHelm);
							GiveItem(m_questPlayer, ValkyrieEpicLegs);
							GiveItem(m_questPlayer, ValkyrieEpicVest);
							break;
						}
					case eCharacterClass.MaulerMid:
						{
							GiveItem(m_questPlayer, MaulerMidEpicArms);
							GiveItem(m_questPlayer, MaulerMidEpicBoots);
							GiveItem(m_questPlayer, MaulerMidEpicGloves);
							GiveItem(m_questPlayer, MaulerMidEpicHelm);
							GiveItem(m_questPlayer, MaulerMidEpicLegs);
							GiveItem(m_questPlayer, MaulerMidEpicVest);
							break;
						}
				}

				m_questPlayer.GainExperience(eXPSource.Quest, 1937768448, true);
				//m_questPlayer.AddMoney(Money.GetMoney(0,0,0,2,Util.Random(50)), "You recieve {0} as a reward.");		
			}
			else
			{
				m_questPlayer.Out.SendMessage("You do not have enough free space in your inventory!", eChatType.CT_Important, eChatLoc.CL_SystemWindow);
			}
		}
	}
}
