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
*Source         : http://camelot.allakhazam.com
*Date           : 22 November 2004
*Quest Name     : Unnatural Powers (level 50)
*Quest Classes  : Eldritch, Hero, Ranger, and Warden (Path of Focus)
*Quest Version  : v1
*
*ToDo:
*   Add Bonuses to Epic Items
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

namespace DOL.GS.Quests.Hibernia
{
	public class Focus_50 : BaseQuest
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected const string questTitle = "Unnatural Powers";
		protected const int minimumLevel = 50;
		protected const int maximumLevel = 50;

		private static GameNPC Ainrebh = null; // Start NPC
		private static GameNPC GreenMaw = null; // Mob to kill

		private static ItemTemplate GreenMaw_key = null; //ball of flame
		private static ItemTemplate RangerEpicBoots = null; //Mist Shrouded Boots 
		private static ItemTemplate RangerEpicHelm = null; //Mist Shrouded Coif 
		private static ItemTemplate RangerEpicGloves = null; //Mist Shrouded Gloves 
		private static ItemTemplate RangerEpicVest = null; //Mist Shrouded Hauberk 
		private static ItemTemplate RangerEpicLegs = null; //Mist Shrouded Legs 
		private static ItemTemplate RangerEpicArms = null; //Mist Shrouded Sleeves 
		private static ItemTemplate HeroEpicBoots = null; //Shadow Shrouded Boots 
		private static ItemTemplate HeroEpicHelm = null; //Shadow Shrouded Coif 
		private static ItemTemplate HeroEpicGloves = null; //Shadow Shrouded Gloves 
		private static ItemTemplate HeroEpicVest = null; //Shadow Shrouded Hauberk 
		private static ItemTemplate HeroEpicLegs = null; //Shadow Shrouded Legs 
		private static ItemTemplate HeroEpicArms = null; //Shadow Shrouded Sleeves 
		private static ItemTemplate EldritchEpicBoots = null; //Valhalla Touched Boots 
		private static ItemTemplate EldritchEpicHelm = null; //Valhalla Touched Coif 
		private static ItemTemplate EldritchEpicGloves = null; //Valhalla Touched Gloves 
		private static ItemTemplate EldritchEpicVest = null; //Valhalla Touched Hauberk 
		private static ItemTemplate EldritchEpicLegs = null; //Valhalla Touched Legs 
		private static ItemTemplate EldritchEpicArms = null; //Valhalla Touched Sleeves 
		private static ItemTemplate WardenEpicBoots = null; //Subterranean Boots 
		private static ItemTemplate WardenEpicHelm = null; //Subterranean Coif 
		private static ItemTemplate WardenEpicGloves = null; //Subterranean Gloves 
		private static ItemTemplate WardenEpicVest = null; //Subterranean Hauberk 
		private static ItemTemplate WardenEpicLegs = null; //Subterranean Legs 
		private static ItemTemplate WardenEpicArms = null; //Subterranean Sleeves    
        private static ItemTemplate MaulerHibEpicBoots = null;
        private static ItemTemplate MaulerHibEpicHelm = null;
        private static ItemTemplate MaulerHibEpicGloves = null;
        private static ItemTemplate MaulerHibEpicVest = null;
        private static ItemTemplate MaulerHibEpicLegs = null;
        private static ItemTemplate MaulerHibEpicArms = null;      

		// Constructors
		public Focus_50() : base()
		{
		}

		public Focus_50(GamePlayer questingPlayer) : base(questingPlayer)
		{
		}

		public Focus_50(GamePlayer questingPlayer, int step) : base(questingPlayer, step)
		{
		}

		public Focus_50(GamePlayer questingPlayer, Quest dbQuest) : base(questingPlayer, dbQuest)
		{
		}

		[ScriptLoadedEvent]
		public static void ScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			if (!ServerProperties.Properties.LOAD_QUESTS)
				return;
			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initializing ...");

			#region NPC Declarations

			GameNPC[] npcs = WorldMgr.GetNPCsByName("Ainrebh", eRealm.Hibernia);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 200 && npc.X == 421281 && npc.Y == 516273)
					{
						Ainrebh = npc;
						break;
					}

			if (Ainrebh == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Ainrebh , creating it ...");
				Ainrebh = new GameNPC();
				Ainrebh.Model = 384;
				Ainrebh.Name = "Ainrebh";
				Ainrebh.GuildName = "Enchanter";
				Ainrebh.Realm = eRealm.Hibernia;
				Ainrebh.CurrentRegionID = 200;
				Ainrebh.Size = 48;
				Ainrebh.Level = 40;
				Ainrebh.X = 421281;
				Ainrebh.Y = 516273;
				Ainrebh.Z = 1877;
				Ainrebh.Heading = 3254;
				Ainrebh.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					Ainrebh.SaveIntoDatabase();
				}
			}
			// end npc

			npcs = WorldMgr.GetNPCsByName("Green Maw", eRealm.None);

			if (npcs.Length > 0)
				foreach (GameNPC npc in npcs)
					if (npc.CurrentRegionID == 200 && npc.X == 488306 && npc.Y == 521440)
					{
						GreenMaw = npc;
						break;
					}

			if (GreenMaw == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find GreenMaw , creating it ...");
				GreenMaw = new GameNPC();
				GreenMaw.Model = 146;
				GreenMaw.Name = "Green Maw";
				GreenMaw.GuildName = "";
				GreenMaw.Realm = eRealm.None;
				GreenMaw.CurrentRegionID = 200;
				GreenMaw.Size = 50;
				GreenMaw.Level = 65;
				GreenMaw.X = 488306;
				GreenMaw.Y = 521440;
				GreenMaw.Z = 6328;
				GreenMaw.Heading = 1162;
				GreenMaw.AddToWorld();
				if (SAVE_INTO_DATABASE)
				{
					GreenMaw.SaveIntoDatabase();
				}
			}
			// end npc

			#endregion

			#region Item Declarations

			GreenMaw_key = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "GreenMaw_key");
			if (GreenMaw_key == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find GreenMaw's Key , creating it ...");
				GreenMaw_key = new ItemTemplate();
				GreenMaw_key.KeyName = "GreenMaw_key";
				GreenMaw_key.Name = "GreenMaw's Key";
				GreenMaw_key.Level = 8;
				GreenMaw_key.ItemType = 29;
				GreenMaw_key.Model = 583;
				GreenMaw_key.IsDropable = false;
				GreenMaw_key.IsPickable = false;
				GreenMaw_key.DPS_AF = 0;
				GreenMaw_key.SPD_ABS = 0;
				GreenMaw_key.ObjectType = 41;
				GreenMaw_key.Hand = 0;
				GreenMaw_key.TypeDamage = 0;
				GreenMaw_key.Quality = 100;
				GreenMaw_key.Weight = 12;
				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(GreenMaw_key);
				}

			}
// end item			
			RangerEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "RangerEpicBoots");
			if (RangerEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Rangers Epic Boots , creating it ...");
				RangerEpicBoots = new ItemTemplate();
				RangerEpicBoots.KeyName = "RangerEpicBoots";
				RangerEpicBoots.Name = "Mist Shrouded Boots";
				RangerEpicBoots.Level = 50;
				RangerEpicBoots.ItemType = 23;
				RangerEpicBoots.Model = 819;
				RangerEpicBoots.IsDropable = true;
				RangerEpicBoots.IsPickable = true;
				RangerEpicBoots.DPS_AF = 100;
				RangerEpicBoots.SPD_ABS = 19;
				RangerEpicBoots.ObjectType = 37;
				RangerEpicBoots.Quality = 100;
				RangerEpicBoots.Weight = 22;
				RangerEpicBoots.ItemBonus = 35;
				RangerEpicBoots.MaxCondition = 50000;
				RangerEpicBoots.MaxDurability = 50000;
				RangerEpicBoots.Condition = 50000;
				RangerEpicBoots.Durability = 50000;

				RangerEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.DEX, BonusValue = 13 });

				RangerEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 12 });

				RangerEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Thrust, BonusValue = 8 });

				RangerEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(RangerEpicBoots);
				}

			}
//end item
			//Mist Shrouded Coif 
			RangerEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "RangerEpicHelm");
			if (RangerEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Rangers Epic Helm , creating it ...");
				RangerEpicHelm = new ItemTemplate();
				RangerEpicHelm.KeyName = "RangerEpicHelm";
				RangerEpicHelm.Name = "Mist Shrouded Helm";
				RangerEpicHelm.Level = 50;
				RangerEpicHelm.ItemType = 21;
				RangerEpicHelm.Model = 1292; //NEED TO WORK ON..
				RangerEpicHelm.IsDropable = true;
				RangerEpicHelm.IsPickable = true;
				RangerEpicHelm.DPS_AF = 100;
				RangerEpicHelm.SPD_ABS = 19;
				RangerEpicHelm.ObjectType = 37;
				RangerEpicHelm.Quality = 100;
				RangerEpicHelm.Weight = 22;
				RangerEpicHelm.ItemBonus = 35;
				RangerEpicHelm.MaxCondition = 50000;
				RangerEpicHelm.MaxDurability = 50000;
				RangerEpicHelm.Condition = 50000;
				RangerEpicHelm.Durability = 50000;

				RangerEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.DEX, BonusValue = 19 });

				RangerEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eResist.Spirit, BonusValue = 10 });

				RangerEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 27 });

				RangerEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Energy, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(RangerEpicHelm);
				}

			}
//end item
			//Mist Shrouded Gloves 
			RangerEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "RangerEpicGloves");
			if (RangerEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Rangers Epic Gloves , creating it ...");
				RangerEpicGloves = new ItemTemplate();
				RangerEpicGloves.KeyName = "RangerEpicGloves";
				RangerEpicGloves.Name = "Mist Shrouded Gloves ";
				RangerEpicGloves.Level = 50;
				RangerEpicGloves.ItemType = 22;
				RangerEpicGloves.Model = 818;
				RangerEpicGloves.IsDropable = true;
				RangerEpicGloves.IsPickable = true;
				RangerEpicGloves.DPS_AF = 100;
				RangerEpicGloves.SPD_ABS = 19;
				RangerEpicGloves.ObjectType = 37;
				RangerEpicGloves.Quality = 100;
				RangerEpicGloves.Weight = 22;
				RangerEpicGloves.ItemBonus = 35;
				RangerEpicGloves.MaxCondition = 50000;
				RangerEpicGloves.MaxDurability = 50000;
				RangerEpicGloves.Condition = 50000;
				RangerEpicGloves.Durability = 50000;

				RangerEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_RecurvedBow, BonusValue = 3 });

				RangerEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 15 });

				RangerEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.QUI, BonusValue = 15 });

				RangerEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Crush, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(RangerEpicGloves);
				}

			}
			//Mist Shrouded Hauberk 
			RangerEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "RangerEpicVest");
			if (RangerEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Rangers Epic Vest , creating it ...");
				RangerEpicVest = new ItemTemplate();
				RangerEpicVest.KeyName = "RangerEpicVest";
				RangerEpicVest.Name = "Mist Shrouded Hauberk";
				RangerEpicVest.Level = 50;
				RangerEpicVest.ItemType = 25;
				RangerEpicVest.Model = 815;
				RangerEpicVest.IsDropable = true;
				RangerEpicVest.IsPickable = true;
				RangerEpicVest.DPS_AF = 100;
				RangerEpicVest.SPD_ABS = 19;
				RangerEpicVest.ObjectType = 37;
				RangerEpicVest.Quality = 100;
				RangerEpicVest.Weight = 22;
				RangerEpicVest.ItemBonus = 35;
				RangerEpicVest.MaxCondition = 50000;
				RangerEpicVest.MaxDurability = 50000;
				RangerEpicVest.Condition = 50000;
				RangerEpicVest.Durability = 50000;

				RangerEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 7 });

				RangerEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 7 });

				RangerEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.QUI, BonusValue = 7 });

				RangerEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 48 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(RangerEpicVest);
				}

			}
			//Mist Shrouded Legs 
			RangerEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "RangerEpicLegs");
			if (RangerEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Rangers Epic Legs , creating it ...");
				RangerEpicLegs = new ItemTemplate();
				RangerEpicLegs.KeyName = "RangerEpicLegs";
				RangerEpicLegs.Name = "Mist Shrouded Leggings";
				RangerEpicLegs.Level = 50;
				RangerEpicLegs.ItemType = 27;
				RangerEpicLegs.Model = 816;
				RangerEpicLegs.IsDropable = true;
				RangerEpicLegs.IsPickable = true;
				RangerEpicLegs.DPS_AF = 100;
				RangerEpicLegs.SPD_ABS = 19;
				RangerEpicLegs.ObjectType = 37;
				RangerEpicLegs.Quality = 100;
				RangerEpicLegs.Weight = 22;
				RangerEpicLegs.ItemBonus = 35;
				RangerEpicLegs.MaxCondition = 50000;
				RangerEpicLegs.MaxDurability = 50000;
				RangerEpicLegs.Condition = 50000;
				RangerEpicLegs.Durability = 50000;

				RangerEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 12 });

				RangerEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 12 });

				RangerEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Body, BonusValue = 12 });

				RangerEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 39 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(RangerEpicLegs);
				}
				;

			}
			//Mist Shrouded Sleeves 
			RangerEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "RangerEpicArms");
			if (RangerEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Ranger Epic Arms , creating it ...");
				RangerEpicArms = new ItemTemplate();
				RangerEpicArms.KeyName = "RangerEpicArms";
				RangerEpicArms.Name = "Mist Shrouded Sleeves";
				RangerEpicArms.Level = 50;
				RangerEpicArms.ItemType = 28;
				RangerEpicArms.Model = 817;
				RangerEpicArms.IsDropable = true;
				RangerEpicArms.IsPickable = true;
				RangerEpicArms.DPS_AF = 100;
				RangerEpicArms.SPD_ABS = 19;
				RangerEpicArms.ObjectType = 37;
				RangerEpicArms.Quality = 100;
				RangerEpicArms.Weight = 22;
				RangerEpicArms.ItemBonus = 35;
				RangerEpicArms.MaxCondition = 50000;
				RangerEpicArms.MaxDurability = 50000;
				RangerEpicArms.Condition = 50000;
				RangerEpicArms.Durability = 50000;

				RangerEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 12 });

				RangerEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 12 });

				RangerEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Spirit, BonusValue = 10 });

				RangerEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(RangerEpicArms);
				}

			}
//Hero Epic Sleeves End
			HeroEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HeroEpicBoots");
			if (HeroEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Heros Epic Boots , creating it ...");
				HeroEpicBoots = new ItemTemplate();
				HeroEpicBoots.KeyName = "HeroEpicBoots";
				HeroEpicBoots.Name = "Misted Boots";
				HeroEpicBoots.Level = 50;
				HeroEpicBoots.ItemType = 23;
				HeroEpicBoots.Model = 712;
				HeroEpicBoots.IsDropable = true;
				HeroEpicBoots.IsPickable = true;
				HeroEpicBoots.DPS_AF = 100;
				HeroEpicBoots.SPD_ABS = 27;
				HeroEpicBoots.ObjectType = 38;
				HeroEpicBoots.Quality = 100;
				HeroEpicBoots.Weight = 22;
				HeroEpicBoots.ItemBonus = 35;
				HeroEpicBoots.MaxCondition = 50000;
				HeroEpicBoots.MaxDurability = 50000;
				HeroEpicBoots.Condition = 50000;
				HeroEpicBoots.Durability = 50000;

				HeroEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 12 });

				HeroEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 12 });

				HeroEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Spirit, BonusValue = 8 });

				HeroEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 33 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HeroEpicBoots);
				}

			}
//end item
			//Misted Coif 
			HeroEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HeroEpicHelm");
			if (HeroEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Heros Epic Helm , creating it ...");
				HeroEpicHelm = new ItemTemplate();
				HeroEpicHelm.KeyName = "HeroEpicHelm";
				HeroEpicHelm.Name = "Misted Coif";
				HeroEpicHelm.Level = 50;
				HeroEpicHelm.ItemType = 21;
				HeroEpicHelm.Model = 1292; //NEED TO WORK ON..
				HeroEpicHelm.IsDropable = true;
				HeroEpicHelm.IsPickable = true;
				HeroEpicHelm.DPS_AF = 100;
				HeroEpicHelm.SPD_ABS = 27;
				HeroEpicHelm.ObjectType = 38;
				HeroEpicHelm.Quality = 100;
				HeroEpicHelm.Weight = 22;
				HeroEpicHelm.ItemBonus = 35;
				HeroEpicHelm.MaxCondition = 50000;
				HeroEpicHelm.MaxDurability = 50000;
				HeroEpicHelm.Condition = 50000;
				HeroEpicHelm.Durability = 50000;

				HeroEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 15 });

				HeroEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eResist.Spirit, BonusValue = 8 });

				HeroEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 48 });

				HeroEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Heat, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HeroEpicHelm);
				}

			}
//end item
			//Misted Gloves 
			HeroEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HeroEpicGloves");
			if (HeroEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Heros Epic Gloves , creating it ...");
				HeroEpicGloves = new ItemTemplate();
				HeroEpicGloves.KeyName = "HeroEpicGloves";
				HeroEpicGloves.Name = "Misted Gloves ";
				HeroEpicGloves.Level = 50;
				HeroEpicGloves.ItemType = 22;
				HeroEpicGloves.Model = 711;
				HeroEpicGloves.IsDropable = true;
				HeroEpicGloves.IsPickable = true;
				HeroEpicGloves.DPS_AF = 100;
				HeroEpicGloves.SPD_ABS = 27;
				HeroEpicGloves.ObjectType = 38;
				HeroEpicGloves.Quality = 100;
				HeroEpicGloves.Weight = 22;
				HeroEpicGloves.ItemBonus = 35;
				HeroEpicGloves.MaxCondition = 50000;
				HeroEpicGloves.MaxDurability = 50000;
				HeroEpicGloves.Condition = 50000;
				HeroEpicGloves.Durability = 50000;

				HeroEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Shields, BonusValue = 2 });

				HeroEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eProperty.Skill_Parry, BonusValue = 2 });

				HeroEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 16 });

				HeroEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.QUI, BonusValue = 18 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HeroEpicGloves);
				}

			}
			//Misted Hauberk 
			HeroEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HeroEpicVest");
			if (HeroEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Heros Epic Vest , creating it ...");
				HeroEpicVest = new ItemTemplate();
				HeroEpicVest.KeyName = "HeroEpicVest";
				HeroEpicVest.Name = "Misted Hauberk";
				HeroEpicVest.Level = 50;
				HeroEpicVest.ItemType = 25;
				HeroEpicVest.Model = 708;
				HeroEpicVest.IsDropable = true;
				HeroEpicVest.IsPickable = true;
				HeroEpicVest.DPS_AF = 100;
				HeroEpicVest.SPD_ABS = 27;
				HeroEpicVest.ObjectType = 38;
				HeroEpicVest.Quality = 100;
				HeroEpicVest.Weight = 22;
				HeroEpicVest.ItemBonus = 35;
				HeroEpicVest.MaxCondition = 50000;
				HeroEpicVest.MaxDurability = 50000;
				HeroEpicVest.Condition = 50000;
				HeroEpicVest.Durability = 50000;

				HeroEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 15 });

				HeroEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 16 });

				HeroEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 15 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HeroEpicVest);
				}

			}
			//Misted Legs 
			HeroEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HeroEpicLegs");
			if (HeroEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Heros Epic Legs , creating it ...");
				HeroEpicLegs = new ItemTemplate();
				HeroEpicLegs.KeyName = "HeroEpicLegs";
				HeroEpicLegs.Name = "Misted Leggings";
				HeroEpicLegs.Level = 50;
				HeroEpicLegs.ItemType = 27;
				HeroEpicLegs.Model = 709;
				HeroEpicLegs.IsDropable = true;
				HeroEpicLegs.IsPickable = true;
				HeroEpicLegs.DPS_AF = 100;
				HeroEpicLegs.SPD_ABS = 27;
				HeroEpicLegs.ObjectType = 38;
				HeroEpicLegs.Quality = 100;
				HeroEpicLegs.Weight = 22;
				HeroEpicLegs.ItemBonus = 35;
				HeroEpicLegs.MaxCondition = 50000;
				HeroEpicLegs.MaxDurability = 50000;
				HeroEpicLegs.Condition = 50000;
				HeroEpicLegs.Durability = 50000;

				HeroEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 10 });

				HeroEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 21 });

				HeroEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Thrust, BonusValue = 10 });

				HeroEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Heat, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HeroEpicLegs);
				}

			}
			//Misted Sleeves 
			HeroEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "HeroEpicArms");
			if (HeroEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Hero Epic Arms , creating it ...");
				HeroEpicArms = new ItemTemplate();
				HeroEpicArms.KeyName = "HeroEpicArms";
				HeroEpicArms.Name = "Misted Sleeves";
				HeroEpicArms.Level = 50;
				HeroEpicArms.ItemType = 28;
				HeroEpicArms.Model = 710;
				HeroEpicArms.IsDropable = true;
				HeroEpicArms.IsPickable = true;
				HeroEpicArms.DPS_AF = 100;
				HeroEpicArms.SPD_ABS = 27;
				HeroEpicArms.ObjectType = 38;
				HeroEpicArms.Quality = 100;
				HeroEpicArms.Weight = 22;
				HeroEpicArms.ItemBonus = 35;
				HeroEpicArms.MaxCondition = 50000;
				HeroEpicArms.MaxDurability = 50000;
				HeroEpicArms.Condition = 50000;
				HeroEpicArms.Durability = 50000;

				HeroEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 24 });

				HeroEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 10 });

				HeroEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Cold, BonusValue = 8 });

				HeroEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Spirit, BonusValue = 8 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(HeroEpicArms);
				}

			}
			WardenEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WardenEpicBoots");
			if (WardenEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warden Epic Boots , creating it ...");
				WardenEpicBoots = new ItemTemplate();
				WardenEpicBoots.KeyName = "WardenEpicBoots";
				WardenEpicBoots.Name = "Mystical Boots";
				WardenEpicBoots.Level = 50;
				WardenEpicBoots.ItemType = 23;
				WardenEpicBoots.Model = 809;
				WardenEpicBoots.IsDropable = true;
				WardenEpicBoots.IsPickable = true;
				WardenEpicBoots.DPS_AF = 100;
				WardenEpicBoots.SPD_ABS = 27;
				WardenEpicBoots.ObjectType = 38;
				WardenEpicBoots.Quality = 100;
				WardenEpicBoots.Weight = 22;
				WardenEpicBoots.ItemBonus = 35;
				WardenEpicBoots.MaxCondition = 50000;
				WardenEpicBoots.MaxDurability = 50000;
				WardenEpicBoots.Condition = 50000;
				WardenEpicBoots.Durability = 50000;

				WardenEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.DEX, BonusValue = 15 });

				WardenEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.QUI, BonusValue = 16 });

				WardenEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Crush, BonusValue = 10 });

				WardenEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eResist.Matter, BonusValue = 10 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WardenEpicBoots);
				}

			}
//end item
			//Mystical Coif 
			WardenEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WardenEpicHelm");
			if (WardenEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warden Epic Helm , creating it ...");
				WardenEpicHelm = new ItemTemplate();
				WardenEpicHelm.KeyName = "WardenEpicHelm";
				WardenEpicHelm.Name = "Mystical Coif";
				WardenEpicHelm.Level = 50;
				WardenEpicHelm.ItemType = 21;
				WardenEpicHelm.Model = 1292; //NEED TO WORK ON..
				WardenEpicHelm.IsDropable = true;
				WardenEpicHelm.IsPickable = true;
				WardenEpicHelm.DPS_AF = 100;
				WardenEpicHelm.SPD_ABS = 27;
				WardenEpicHelm.ObjectType = 38;
				WardenEpicHelm.Quality = 100;
				WardenEpicHelm.Weight = 22;
				WardenEpicHelm.ItemBonus = 35;
				WardenEpicHelm.MaxCondition = 50000;
				WardenEpicHelm.MaxDurability = 50000;
				WardenEpicHelm.Condition = 50000;
				WardenEpicHelm.Durability = 50000;

				WardenEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.EMP, BonusValue = 15 });

				WardenEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eProperty.PowerRegenerationRate, BonusValue = 2 });

				WardenEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				WardenEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.Skill_Regrowth, BonusValue = 4 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WardenEpicHelm);
				}

			}
//end item
			//Mystical Gloves 
			WardenEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WardenEpicGloves");
			if (WardenEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warden Epic Gloves , creating it ...");
				WardenEpicGloves = new ItemTemplate();
				WardenEpicGloves.KeyName = "WardenEpicGloves";
				WardenEpicGloves.Name = "Mystical Gloves ";
				WardenEpicGloves.Level = 50;
				WardenEpicGloves.ItemType = 22;
				WardenEpicGloves.Model = 808;
				WardenEpicGloves.IsDropable = true;
				WardenEpicGloves.IsPickable = true;
				WardenEpicGloves.DPS_AF = 100;
				WardenEpicGloves.SPD_ABS = 27;
				WardenEpicGloves.ObjectType = 38;
				WardenEpicGloves.Quality = 100;
				WardenEpicGloves.Weight = 22;
				WardenEpicGloves.ItemBonus = 35;
				WardenEpicGloves.MaxCondition = 50000;
				WardenEpicGloves.MaxDurability = 50000;
				WardenEpicGloves.Condition = 50000;
				WardenEpicGloves.Durability = 50000;

				WardenEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Skill_Nurture, BonusValue = 4 });

				WardenEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eResist.Slash, BonusValue = 12 });

				WardenEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.PowerRegenerationRate, BonusValue = 4 });

				WardenEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 33 });


				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WardenEpicGloves);
				}

			}
			//Mystical Hauberk 
			WardenEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WardenEpicVest");
			if (WardenEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warden Epic Vest , creating it ...");
				WardenEpicVest = new ItemTemplate();
				WardenEpicVest.KeyName = "WardenEpicVest";
				WardenEpicVest.Name = "Mystical Hauberk";
				WardenEpicVest.Level = 50;
				WardenEpicVest.ItemType = 25;
				WardenEpicVest.Model = 805;
				WardenEpicVest.IsDropable = true;
				WardenEpicVest.IsPickable = true;
				WardenEpicVest.DPS_AF = 100;
				WardenEpicVest.SPD_ABS = 27;
				WardenEpicVest.ObjectType = 38;
				WardenEpicVest.Quality = 100;
				WardenEpicVest.Weight = 22;
				WardenEpicVest.ItemBonus = 35;
				WardenEpicVest.MaxCondition = 50000;
				WardenEpicVest.MaxDurability = 50000;
				WardenEpicVest.Condition = 50000;
				WardenEpicVest.Durability = 50000;

				WardenEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 9 });

				WardenEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 9 });

				WardenEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.EMP, BonusValue = 9 });

				WardenEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eProperty.MaxHealth, BonusValue = 39 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WardenEpicVest);
				}

			}
			//Mystical Legs 
			WardenEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WardenEpicLegs");
			if (WardenEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warden Epic Legs , creating it ...");
				WardenEpicLegs = new ItemTemplate();
				WardenEpicLegs.KeyName = "WardenEpicLegs";
				WardenEpicLegs.Name = "Mystical Legs";
				WardenEpicLegs.Level = 50;
				WardenEpicLegs.ItemType = 27;
				WardenEpicLegs.Model = 806;
				WardenEpicLegs.IsDropable = true;
				WardenEpicLegs.IsPickable = true;
				WardenEpicLegs.DPS_AF = 100;
				WardenEpicLegs.SPD_ABS = 27;
				WardenEpicLegs.ObjectType = 38;
				WardenEpicLegs.Quality = 100;
				WardenEpicLegs.Weight = 22;
				WardenEpicLegs.ItemBonus = 35;
				WardenEpicLegs.MaxCondition = 50000;
				WardenEpicLegs.MaxDurability = 50000;
				WardenEpicLegs.Condition = 50000;
				WardenEpicLegs.Durability = 50000;

				WardenEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 10 });

				WardenEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.CON, BonusValue = 10 });

				WardenEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 10 });

				WardenEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 30 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WardenEpicLegs);
				}

			}
			//Mystical Sleeves 
			WardenEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "WardenEpicArms");
			if (WardenEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Warden Epic Arms , creating it ...");
				WardenEpicArms = new ItemTemplate();
				WardenEpicArms.KeyName = "WardenEpicArms";
				WardenEpicArms.Name = "Mystical Sleeves";
				WardenEpicArms.Level = 50;
				WardenEpicArms.ItemType = 28;
				WardenEpicArms.Model = 807;
				WardenEpicArms.IsDropable = true;
				WardenEpicArms.IsPickable = true;
				WardenEpicArms.DPS_AF = 100;
				WardenEpicArms.SPD_ABS = 27;
				WardenEpicArms.ObjectType = 38;
				WardenEpicArms.Quality = 100;
				WardenEpicArms.Weight = 22;
				WardenEpicArms.ItemBonus = 35;
				WardenEpicArms.MaxCondition = 50000;
				WardenEpicArms.MaxDurability = 50000;
				WardenEpicArms.Condition = 50000;
				WardenEpicArms.Durability = 50000;

				WardenEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.STR, BonusValue = 12 });

				WardenEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eResist.Matter, BonusValue = 8 });

				WardenEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eResist.Spirit, BonusValue = 8 });

				WardenEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 45 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(WardenEpicArms);
				}

			}
			EldritchEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "EldritchEpicBoots");
			if (EldritchEpicBoots == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Eldritch Epic Boots , creating it ...");
				EldritchEpicBoots = new ItemTemplate();
				EldritchEpicBoots.KeyName = "EldritchEpicBoots";
				EldritchEpicBoots.Name = "Mistwoven Boots";
				EldritchEpicBoots.Level = 50;
				EldritchEpicBoots.ItemType = 23;
				EldritchEpicBoots.Model = 382;
				EldritchEpicBoots.IsDropable = true;
				EldritchEpicBoots.IsPickable = true;
				EldritchEpicBoots.DPS_AF = 50;
				EldritchEpicBoots.SPD_ABS = 0;
				EldritchEpicBoots.ObjectType = 32;
				EldritchEpicBoots.Quality = 100;
				EldritchEpicBoots.Weight = 22;
				EldritchEpicBoots.ItemBonus = 35;
				EldritchEpicBoots.MaxCondition = 50000;
				EldritchEpicBoots.MaxDurability = 50000;
				EldritchEpicBoots.Condition = 50000;
				EldritchEpicBoots.Durability = 50000;

				EldritchEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.CON, BonusValue = 9 });

				EldritchEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 9 });

				EldritchEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.PowerRegenerationRate, BonusValue = 6 });

				EldritchEpicBoots.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 21 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(EldritchEpicBoots);
				}

			}
//end item
			//Mist Woven Coif 
			EldritchEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "EldritchEpicHelm");
			if (EldritchEpicHelm == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Eldritch Epic Helm , creating it ...");
				EldritchEpicHelm = new ItemTemplate();
				EldritchEpicHelm.KeyName = "EldritchEpicHelm";
				EldritchEpicHelm.Name = "Mistwoven Cap";
				EldritchEpicHelm.Level = 50;
				EldritchEpicHelm.ItemType = 21;
				EldritchEpicHelm.Model = 1298; //NEED TO WORK ON..
				EldritchEpicHelm.IsDropable = true;
				EldritchEpicHelm.IsPickable = true;
				EldritchEpicHelm.DPS_AF = 50;
				EldritchEpicHelm.SPD_ABS = 0;
				EldritchEpicHelm.ObjectType = 32;
				EldritchEpicHelm.Quality = 100;
				EldritchEpicHelm.Weight = 22;
				EldritchEpicHelm.ItemBonus = 35;
				EldritchEpicHelm.MaxCondition = 50000;
				EldritchEpicHelm.MaxDurability = 50000;
				EldritchEpicHelm.Condition = 50000;
				EldritchEpicHelm.Durability = 50000;

				EldritchEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eResist.Heat, BonusValue = 10 });

				EldritchEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eResist.Spirit, BonusValue = 10 });

				EldritchEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.Focus_Void, BonusValue = 4 });

				EldritchEpicHelm.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.INT, BonusValue = 19 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(EldritchEpicHelm);
				}

			}
//end item
			//Mist Woven Gloves 
			EldritchEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "EldritchEpicGloves");
			if (EldritchEpicGloves == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Eldritch Epic Gloves , creating it ...");
				EldritchEpicGloves = new ItemTemplate();
				EldritchEpicGloves.KeyName = "EldritchEpicGloves";
				EldritchEpicGloves.Name = "Mistwoven Gloves ";
				EldritchEpicGloves.Level = 50;
				EldritchEpicGloves.ItemType = 22;
				EldritchEpicGloves.Model = 381;
				EldritchEpicGloves.IsDropable = true;
				EldritchEpicGloves.IsPickable = true;
				EldritchEpicGloves.DPS_AF = 50;
				EldritchEpicGloves.SPD_ABS = 0;
				EldritchEpicGloves.ObjectType = 32;
				EldritchEpicGloves.Quality = 100;
				EldritchEpicGloves.Weight = 22;
				EldritchEpicGloves.ItemBonus = 35;
				EldritchEpicGloves.MaxCondition = 50000;
				EldritchEpicGloves.MaxDurability = 50000;
				EldritchEpicGloves.Condition = 50000;
				EldritchEpicGloves.Durability = 50000;

				EldritchEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Focus_Light, BonusValue = 4 });

				EldritchEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 9 });

				EldritchEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.PowerRegenerationRate, BonusValue = 4 });

				EldritchEpicGloves.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 24 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(EldritchEpicGloves);
				}

			}
			//Mist Woven Hauberk 
			EldritchEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "EldritchEpicVest");
			if (EldritchEpicVest == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Eldritch Epic Vest , creating it ...");
				EldritchEpicVest = new ItemTemplate();
				EldritchEpicVest.KeyName = "EldritchEpicVest";
				EldritchEpicVest.Name = "Mistwoven Vest";
				EldritchEpicVest.Level = 50;
				EldritchEpicVest.ItemType = 25;
				EldritchEpicVest.Model = 744;
				EldritchEpicVest.IsDropable = true;
				EldritchEpicVest.IsPickable = true;
				EldritchEpicVest.DPS_AF = 50;
				EldritchEpicVest.SPD_ABS = 0;
				EldritchEpicVest.ObjectType = 32;
				EldritchEpicVest.Quality = 100;
				EldritchEpicVest.Weight = 22;
				EldritchEpicVest.ItemBonus = 35;
				EldritchEpicVest.MaxCondition = 50000;
				EldritchEpicVest.MaxDurability = 50000;
				EldritchEpicVest.Condition = 50000;
				EldritchEpicVest.Durability = 50000;

				EldritchEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eStat.DEX, BonusValue = 15 });

				EldritchEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.INT, BonusValue = 15 });

				EldritchEpicVest.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eProperty.MaxHealth, BonusValue = 33 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(EldritchEpicVest);
				}

			}
			//Mist Woven Legs 
			EldritchEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "EldritchEpicLegs");
			if (EldritchEpicLegs == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Eldritch Epic Legs , creating it ...");
				EldritchEpicLegs = new ItemTemplate();
				EldritchEpicLegs.KeyName = "EldritchEpicLegs";
				EldritchEpicLegs.Name = "Mistwoven Pants";
				EldritchEpicLegs.Level = 50;
				EldritchEpicLegs.ItemType = 27;
				EldritchEpicLegs.Model = 379;
				EldritchEpicLegs.IsDropable = true;
				EldritchEpicLegs.IsPickable = true;
				EldritchEpicLegs.DPS_AF = 50;
				EldritchEpicLegs.SPD_ABS = 0;
				EldritchEpicLegs.ObjectType = 32;
				EldritchEpicLegs.Quality = 100;
				EldritchEpicLegs.Weight = 22;
				EldritchEpicLegs.ItemBonus = 35;
				EldritchEpicLegs.MaxCondition = 50000;
				EldritchEpicLegs.MaxDurability = 50000;
				EldritchEpicLegs.Condition = 50000;
				EldritchEpicLegs.Durability = 50000;

				EldritchEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eResist.Cold, BonusValue = 10 });

				EldritchEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eResist.Body, BonusValue = 10 });

				EldritchEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.DEX, BonusValue = 15 });

				EldritchEpicLegs.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eStat.CON, BonusValue = 16 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(EldritchEpicLegs);
				}

			}
			//Mist Woven Sleeves 
			EldritchEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "EldritchEpicArms");
			if (EldritchEpicArms == null)
			{
				if (log.IsWarnEnabled)
					log.Warn("Could not find Eldritch Epic Arms , creating it ...");
				EldritchEpicArms = new ItemTemplate();
				EldritchEpicArms.KeyName = "EldritchEpicArms";
				EldritchEpicArms.Name = "Mistwoven Sleeves";
				EldritchEpicArms.Level = 50;
				EldritchEpicArms.ItemType = 28;
				EldritchEpicArms.Model = 380;
				EldritchEpicArms.IsDropable = true;
				EldritchEpicArms.IsPickable = true;
				EldritchEpicArms.DPS_AF = 50;
				EldritchEpicArms.SPD_ABS = 0;
				EldritchEpicArms.ObjectType = 32;
				EldritchEpicArms.Quality = 100;
				EldritchEpicArms.Weight = 22;
				EldritchEpicArms.ItemBonus = 35;
				EldritchEpicArms.MaxCondition = 50000;
				EldritchEpicArms.MaxDurability = 50000;
				EldritchEpicArms.Condition = 50000;
				EldritchEpicArms.Durability = 50000;

				EldritchEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 1, BonusType = (int)eProperty.Focus_Mana, BonusValue = 4 });

				EldritchEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 2, BonusType = (int)eStat.DEX, BonusValue = 10 });

				EldritchEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 3, BonusType = (int)eStat.INT, BonusValue = 10 });

				EldritchEpicArms.Bonuses.Add(new ItemBonus() { BonusOrder = 4, BonusType = (int)eProperty.MaxHealth, BonusValue = 27 });

				if (SAVE_INTO_DATABASE)
				{
					GameServer.Instance.SaveDataObject(EldritchEpicArms);
				}

			}

//Hero Epic Sleeves End

            // Graveen: we assume items are existing in the DB
            // TODO: insert here creation of items if they do not exists
            MaulerHibEpicBoots = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerHibEpicBoots");
            MaulerHibEpicHelm = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerHibEpicHelm");
            MaulerHibEpicGloves = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerHibEpicGloves");
            MaulerHibEpicVest = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerHibEpicVest");
            MaulerHibEpicLegs = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerHibEpicLegs");
            MaulerHibEpicArms = GameServer.Database.ItemTemplates.FirstOrDefault(x => x.KeyName == "MaulerHibEpicArms");

//Item Descriptions End

			#endregion

			GameEventMgr.AddHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.AddHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.AddHandler(Ainrebh, GameObjectEvent.Interact, new DOLEventHandler(TalkToAinrebh));
			GameEventMgr.AddHandler(Ainrebh, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToAinrebh));

			/* Now we bring to Ainrebh the possibility to give this quest to players */
			Ainrebh.AddQuestToGive(typeof (Focus_50));

			if (log.IsInfoEnabled)
				log.Info("Quest \"" + questTitle + "\" initialized");
		}

		[ScriptUnloadedEvent]
		public static void ScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			//if not loaded, don't worry
			if (Ainrebh == null)
				return;
			// remove handlers
			GameEventMgr.RemoveHandler(GamePlayerEvent.AcceptQuest, new DOLEventHandler(SubscribeQuest));
			GameEventMgr.RemoveHandler(GamePlayerEvent.DeclineQuest, new DOLEventHandler(SubscribeQuest));

			GameEventMgr.RemoveHandler(Ainrebh, GameObjectEvent.Interact, new DOLEventHandler(TalkToAinrebh));
			GameEventMgr.RemoveHandler(Ainrebh, GameLivingEvent.WhisperReceive, new DOLEventHandler(TalkToAinrebh));

			/* Now we remove to Ainrebh the possibility to give this quest to players */
			Ainrebh.RemoveQuestToGive(typeof (Focus_50));
		}

		protected static void TalkToAinrebh(DOLEvent e, object sender, EventArgs args)
		{
			//We get the player from the event arguments and check if he qualifies		
			GamePlayer player = ((SourceEventArgs) args).Source as GamePlayer;
			if (player == null)
				return;

			if (Ainrebh.CanGiveQuest(typeof (Focus_50), player)  <= 0)
				return;

			// player is not allowed to start this quest until the quest rewards are available
			if (player.CharacterClass.ID == (byte)eCharacterClass.MaulerHib &&
				(MaulerHibEpicBoots == null || MaulerHibEpicBoots == null || MaulerHibEpicGloves == null ||
				MaulerHibEpicHelm == null || MaulerHibEpicLegs == null || MaulerHibEpicVest == null))
			{
				Ainrebh.SayTo(player, "This quest is not available to Maulers yet.");
				return;
			}

			//We also check if the player is already doing the quest
			Focus_50 quest = player.IsDoingQuest(typeof (Focus_50)) as Focus_50;

			if (e == GameObjectEvent.Interact)
			{
				if (quest != null)
				{
					Ainrebh.SayTo(player, "Check your Journal for instructions!");
				}
				else
				{
					Ainrebh.SayTo(player, "Hibernia needs your [services]");
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
							player.Out.SendQuestSubscribeCommand(Ainrebh, QuestMgr.GetIDForQuestType(typeof(Focus_50)), "Will you help Ainrebh [Path of Focus Level 50 Epic]?");
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

		public override bool CheckQuestQualification(GamePlayer player)
		{
			// if the player is already doing the quest his level is no longer of relevance
			if (player.IsDoingQuest(typeof (Focus_50)) != null)
				return true;

			if (player.CharacterClass.ID != (byte) eCharacterClass.Hero &&
				player.CharacterClass.ID != (byte) eCharacterClass.Ranger &&
                player.CharacterClass.ID != (byte) eCharacterClass.MaulerHib &&
                player.CharacterClass.ID != (byte) eCharacterClass.Warden &&
				player.CharacterClass.ID != (byte) eCharacterClass.Eldritch)
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
			Focus_50 quest = player.IsDoingQuest(typeof (Focus_50)) as Focus_50;

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

			if (qargs.QuestID != QuestMgr.GetIDForQuestType(typeof(Focus_50)))
				return;

			if (e == GamePlayerEvent.AcceptQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x01);
			else if (e == GamePlayerEvent.DeclineQuest)
				CheckPlayerAcceptQuest(qargs.Player, 0x00);
		}

		private static void CheckPlayerAcceptQuest(GamePlayer player, byte response)
		{
			if(Ainrebh.CanGiveQuest(typeof (Focus_50), player)  <= 0)
				return;

			if (player.IsDoingQuest(typeof (Focus_50)) != null)
				return;

			if (response == 0x00)
			{
				player.Out.SendMessage("Our God forgives your laziness, just look out for stray lightning bolts.", eChatType.CT_Say, eChatLoc.CL_PopupWindow);
			}
			else
			{
				//Check if we can add the quest!
				if (!Ainrebh.GiveQuest(typeof (Focus_50), player, 1))
					return;
				player.Out.SendMessage("Kill Green Maw in Cursed Forest loc 37k, 38k!", eChatType.CT_System, eChatLoc.CL_PopupWindow);
			}
		}

		//Set quest name
		public override string Name
		{
			get { return "Unnatural Powers (Level 50 Path of Focus Epic)"; }
		}

		// Define Steps
		public override string Description
		{
			get
			{
				switch (Step)
				{
					case 1:
						return "[Step #1] Seek out GreenMaw in Cursed Forest Loc 37k,38k kill it!";
					case 2:
						return "[Step #2] Return to Ainrebh and give her Green Maw's Key!";
				}
				return base.Description;
			}
		}

		public override void Notify(DOLEvent e, object sender, EventArgs args)
		{
			GamePlayer player = sender as GamePlayer;

			if (player==null || player.IsDoingQuest(typeof (Focus_50)) == null)
				return;

			if (Step == 1 && e == GameLivingEvent.EnemyKilled)
			{
				EnemyKilledEventArgs gArgs = (EnemyKilledEventArgs) args;

				if (gArgs.Target.Name == GreenMaw.Name)
				{
					m_questPlayer.Out.SendMessage("You collect Green Maw's Key", eChatType.CT_System, eChatLoc.CL_SystemWindow);
					GiveItem(m_questPlayer, GreenMaw_key);
					Step = 2;
					return;
				}

			}

			if (Step == 2 && e == GamePlayerEvent.GiveItem)
            {
				GiveItemEventArgs gArgs = (GiveItemEventArgs) args;
				if (gArgs.Target.Name == Ainrebh.Name && gArgs.Item.ItemTemplate.KeyName == GreenMaw_key.KeyName)
				{
					Ainrebh.SayTo(player, "You have earned this Epic Armour!");
					FinishQuest();
					return;
				}
			}
		}

		public override void AbortQuest()
		{
			base.AbortQuest(); //Defined in Quest, changes the state, stores in DB etc ...

			RemoveItem(m_questPlayer, GreenMaw_key, false);
		}

		public override void FinishQuest()
		{
			if (m_questPlayer.Inventory.IsSlotsFree(6, eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack))
			{
				RemoveItem(Ainrebh, m_questPlayer, GreenMaw_key);

				base.FinishQuest(); //Defined in Quest, changes the state, stores in DB etc ...

				if (m_questPlayer.CharacterClass.ID == (byte)eCharacterClass.Hero)
				{
					GiveItem(m_questPlayer, HeroEpicArms);
					GiveItem(m_questPlayer, HeroEpicBoots);
					GiveItem(m_questPlayer, HeroEpicGloves);
					GiveItem(m_questPlayer, HeroEpicHelm);
					GiveItem(m_questPlayer, HeroEpicLegs);
					GiveItem(m_questPlayer, HeroEpicVest);
				}
				else if (m_questPlayer.CharacterClass.ID == (byte)eCharacterClass.Ranger)
				{
					GiveItem(m_questPlayer, RangerEpicArms);
					GiveItem(m_questPlayer, RangerEpicBoots);
					GiveItem(m_questPlayer, RangerEpicGloves);
					GiveItem(m_questPlayer, RangerEpicHelm);
					GiveItem(m_questPlayer, RangerEpicLegs);
					GiveItem(m_questPlayer, RangerEpicVest);
				}
				else if (m_questPlayer.CharacterClass.ID == (byte)eCharacterClass.Eldritch)
				{
					GiveItem(m_questPlayer, EldritchEpicArms);
					GiveItem(m_questPlayer, EldritchEpicBoots);
					GiveItem(m_questPlayer, EldritchEpicGloves);
					GiveItem(m_questPlayer, EldritchEpicHelm);
					GiveItem(m_questPlayer, EldritchEpicLegs);
					GiveItem(m_questPlayer, EldritchEpicVest);
				}
				else if (m_questPlayer.CharacterClass.ID == (byte)eCharacterClass.Warden)
				{
					GiveItem(m_questPlayer, WardenEpicArms);
					GiveItem(m_questPlayer, WardenEpicBoots);
					GiveItem(m_questPlayer, WardenEpicGloves);
					GiveItem(m_questPlayer, WardenEpicHelm);
					GiveItem(m_questPlayer, WardenEpicLegs);
					GiveItem(m_questPlayer, WardenEpicVest);
				}
				else if (m_questPlayer.CharacterClass.ID == (byte)eCharacterClass.MaulerHib)
				{
					GiveItem(m_questPlayer, MaulerHibEpicBoots);
					GiveItem(m_questPlayer, MaulerHibEpicArms);
					GiveItem(m_questPlayer, MaulerHibEpicGloves);
					GiveItem(m_questPlayer, MaulerHibEpicHelm);
					GiveItem(m_questPlayer, MaulerHibEpicVest);
					GiveItem(m_questPlayer, MaulerHibEpicLegs);
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
        *#25 talk to Ainrebh
        *#26 seek out Loken in Raumarik Loc 47k, 25k, 4k, and kill him purp and 2 blue adds 
        *#27 return to Ainrebh 
        *#28 give her the ball of flame
        *#29 talk with Ainrebh about Loken�s demise
        *#30 go to MorlinCaan in Jordheim 
        *#31 give her the sealed pouch
        *#32 you get your epic armor as a reward
        */

		/*
            *Mist Shrouded Boots 
            *Mist Shrouded Coif
            *Mist Shrouded Gloves
            *Mist Shrouded Hauberk
            *Mist Shrouded Legs
            *Mist Shrouded Sleeves
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
