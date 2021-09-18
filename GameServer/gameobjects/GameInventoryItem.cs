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
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using DOL.Language;
using DOL.GS.PacketHandler;
using Atlas.DataLayer.Models;
using DOL.GS.Spells;

using log4net;

namespace DOL.GS
{
	/// <summary>
	/// This class represents an inventory item
	/// </summary>
	public class GameInventoryItem : InventoryItem, IGameInventoryItem
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected GamePlayer m_owner = null;

		public GameInventoryItem()
			: base()
		{
		}

		public GameInventoryItem(ItemTemplate template)
			: base(template)
		{
		}

		public GameInventoryItem(ItemUnique template)
			: base(template)
		{
		}

		public GameInventoryItem(InventoryItem item)
			: base(item)
		{
			CharacterID = item.CharacterID;
			this.Id = item.Id;
		}

        /// <summary>
        /// Holds the translation id.
        /// </summary>
        protected string m_translationId = "";

		/// <summary>
		/// Gets or sets the translation id.
		/// </summary>
		public string TranslationId
		{
			get { return m_translationId; }
			set { m_translationId = (value == null ? "" : value); }
		}

		/// <summary>
		/// Is this a valid item for this player?
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool CheckValid(GamePlayer player)
		{
			m_owner = player;
			return true;
		}

		/// <summary>
		/// Can this item be saved or loaded from the database?
		/// </summary>
		public virtual bool CanPersist
		{
			get 
			{
				if (ItemTemplate == null || ItemTemplate.KeyName == InventoryItem.BLANK_ITEM)
					return false;

				return true;
			}
		}

		/// <summary>
		/// Can player equip this item?
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool CanEquip(GamePlayer player)
		{
			return GameServer.ServerRules.CheckAbilityToUseItem(player, ItemTemplate);
		}

		#region Create From Object Source
		
		/// <summary>
		/// This is used to create a PlayerInventoryItem
		/// ClassType will be checked and the approrpiate GameInventoryItem created
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		[Obsolete("Use Create() instead")]
		public static GameInventoryItem Create<T>(ItemTemplate item)
		{
			return Create(item);
		}
		
		/// <summary>
		/// This is used to create a PlayerInventoryItem
		/// template.ClassType will be checked and the approrpiate GameInventoryItem created
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		[Obsolete("Use Create() instead")]
		public static GameInventoryItem Create<T>(InventoryItem item)
		{
			return Create(item);
		}
		
		/// <summary>
		/// This is used to create a PlayerInventoryItem
		/// ClassType will be checked and the approrpiate GameInventoryItem created
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static GameInventoryItem Create(ItemTemplate item)
		{
			string classType = item.ClassType;
			var itemUnique = item as ItemUnique;
			
			if (!string.IsNullOrEmpty(classType))
			{
				GameInventoryItem gameItem;
				if (itemUnique != null)
					gameItem = ScriptMgr.CreateObjectFromClassType<GameInventoryItem, ItemUnique>(classType, itemUnique);
				else
					gameItem = ScriptMgr.CreateObjectFromClassType<GameInventoryItem, ItemTemplate>(classType, item);
				
				if (gameItem != null)
					return gameItem;
				
				if (log.IsWarnEnabled)
					log.WarnFormat("Failed to construct game inventory item of ClassType {0}!", classType);
			}
			
			if (itemUnique != null)
				return new GameInventoryItem(itemUnique);
				
			return new GameInventoryItem(item);
		}

		/// <summary>
		/// This is used to create a PlayerInventoryItem
		/// template.ClassType will be checked and the approrpiate GameInventoryItem created
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static GameInventoryItem Create(InventoryItem item)
		{
			string classType = item.ItemTemplate.ClassType;
			
			if (!string.IsNullOrEmpty(classType))
			{
				GameInventoryItem gameItem = ScriptMgr.CreateObjectFromClassType<GameInventoryItem, InventoryItem>(classType, item);
				
				if (gameItem != null)
					return gameItem;
				
				if (log.IsWarnEnabled)
					log.WarnFormat("Failed to construct game inventory item of ClassType {0}!", classType);
			}
			
			return new GameInventoryItem(item);
		}

		#endregion

		/// <summary>
		/// Player receives this item (added to players inventory)
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnReceive(GamePlayer player)
		{
			m_owner = player;
		}

		/// <summary>
		/// Player loses this item (removed from inventory)
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnLose(GamePlayer player)
		{
			m_owner = null;
		}

		/// <summary>
		/// Drop this item on the ground
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual WorldInventoryItem Drop(GamePlayer player)
		{
			WorldInventoryItem worldItem = new WorldInventoryItem(this);

			Point2D itemloc = player.GetPointFromHeading(player.Heading, 30);
			worldItem.X = itemloc.X;
			worldItem.Y = itemloc.Y;
			worldItem.Z = player.Z;
			worldItem.Heading = player.Heading;
			worldItem.CurrentRegionID = player.CurrentRegionID;

			worldItem.AddOwner(player);
			worldItem.AddToWorld();

			return worldItem;
		}

		/// <summary>
		/// This object is being removed from the world
		/// </summary>
		public virtual void OnRemoveFromWorld()
		{
		}

		/// <summary>
		/// Player equips this item
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnEquipped(GamePlayer player)
		{
			CheckValid(player);
		}

		/// <summary>
		/// Player unequips this item
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnUnEquipped(GamePlayer player)
		{
			CheckValid(player);
		}

        /// <summary>
		/// This inventory is used for a spell cast (staves lose condition when spells are cast)
		/// </summary>
		/// <param name="player"></param>
		/// <param name="target"></param>
        public virtual void OnSpellCast(GameLiving owner, GameObject target, Spell spell)
        {
            OnStrikeTarget(owner, target);
        }

		/// <summary>
		/// This inventory strikes an enemy
		/// </summary>
		/// <param name="player"></param>
		/// <param name="target"></param>
		public virtual void OnStrikeTarget(GameLiving owner, GameObject target)
		{
			if (owner is GamePlayer)
			{
				GamePlayer player = owner as GamePlayer;

				if (ConditionPercent > 70 && Util.Chance(ServerProperties.Properties.ITEM_CONDITION_LOSS_CHANCE))
				{
					int oldPercent = ConditionPercent;
					double con = GamePlayer.GetConLevel(player.Level, this.ItemTemplate.Level);
					if (con < -3.0)
						con = -3.0;
					int sub = (int)(con + 4);
					if (oldPercent < 91)
					{
						sub *= 2;
					}

					// Subtract condition
					Condition -= sub;
					if (Condition < 0)
						Condition = 0;

					if (ConditionPercent != oldPercent)
					{
						if (ConditionPercent == 90)
							player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "GamePlayer.Attack.CouldRepair", Name), eChatType.CT_System, eChatLoc.CL_SystemWindow);
						else if (ConditionPercent == 80)
							player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "GamePlayer.Attack.NeedRepair", Name), eChatType.CT_System, eChatLoc.CL_SystemWindow);
						else if (ConditionPercent == 70)
							player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "GamePlayer.Attack.NeedRepairDire", Name), eChatType.CT_System, eChatLoc.CL_SystemWindow);

						player.Out.SendUpdateWeaponAndArmorStats();
						player.Out.SendInventorySlotsUpdate(new int[] { SlotPosition });
					}
				}
			}
		}

		/// <summary>
		/// This inventory is struck by an enemy
		/// </summary>
		/// <param name="player"></param>
		/// <param name="enemy"></param>
		public virtual void OnStruckByEnemy(GameLiving owner, GameLiving enemy)
		{
			if (owner is GamePlayer)
			{
				GamePlayer player = owner as GamePlayer;

				if (ConditionPercent > 70 && Util.Chance(ServerProperties.Properties.ITEM_CONDITION_LOSS_CHANCE))
				{
					int oldPercent = ConditionPercent;
					double con = GamePlayer.GetConLevel(player.Level, ItemTemplate.Level);
					if (con < -3.0)
						con = -3.0;
					int sub = (int)(con + 4);
					if (oldPercent < 91)
					{
						sub *= 2;
					}

					// Subtract condition
					Condition -= sub;
					if (Condition < 0)
						Condition = 0;

					if (ConditionPercent != oldPercent)
					{
						if (ConditionPercent == 90)
							player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "GamePlayer.Attack.CouldRepair", Name), eChatType.CT_System, eChatLoc.CL_SystemWindow);
						else if (ConditionPercent == 80)
							player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "GamePlayer.Attack.NeedRepair", Name), eChatType.CT_System, eChatLoc.CL_SystemWindow);
						else if (ConditionPercent == 70)
							player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "GamePlayer.Attack.NeedRepairDire", Name), eChatType.CT_System, eChatLoc.CL_SystemWindow);

						player.Out.SendUpdateWeaponAndArmorStats();
						player.Out.SendInventorySlotsUpdate(new int[] { SlotPosition });
					}
				}
			}
		}

		/// <summary>
		/// Try and use this item
		/// </summary>
		/// <param name="player"></param>
		/// <returns>true if item use is handled here</returns>
		public virtual bool Use(GamePlayer player)
		{
			return false;
		}


		/// <summary>
		/// Combine this item with the target item
		/// </summary>
		/// <param name="player"></param>
		/// <param name="targetItem"></param>
		/// <returns>true if combine is handled here</returns>
		public virtual bool Combine(GamePlayer player, InventoryItem targetItem)
		{
			return false;
		}

		/// <summary>
		/// Delve this item
		/// </summary>
		/// <param name="delve"></param>
		/// <param name="player"></param>
		public virtual void Delve(List<String> delve, GamePlayer player)
		{
			if (player == null)
				return;

			//**********************************
			//show crafter name
			//**********************************
			if (IsCrafted)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.HandlePacket.CrafterName", Creator));
				delve.Add(" ");
			}
			else if (ItemTemplate.Description != null && ItemTemplate.Description != "")
			{
				delve.Add(ItemTemplate.Description);
				delve.Add(" ");
			}

			if ((ItemTemplate.ObjectType >= (int)eObjectType.GenericWeapon) && (ItemTemplate.ObjectType <= (int)eObjectType._LastWeapon) ||
				ItemTemplate.ObjectType == (int)eObjectType.Instrument)
			{
				WriteUsableClasses(delve, player.Client);
				WriteMagicalBonuses(delve, player.Client, false);
				DelveWeaponStats(delve, player);
			}

			if (ItemTemplate.ObjectType >= (int)eObjectType.Cloth && ItemTemplate.ObjectType <= (int)eObjectType.Scale)
			{
				WriteUsableClasses(delve, player.Client);
				WriteMagicalBonuses(delve, player.Client, false);
				DelveArmorStats(delve, player);
			}

			if (ItemTemplate.ObjectType == (int)eObjectType.Shield)
			{
				WriteUsableClasses(delve, player.Client);
				WriteMagicalBonuses(delve, player.Client, false);
				DelveShieldStats(delve, player.Client);
			}

            if (ItemTemplate.ObjectType == (int)eObjectType.Magical || ItemTemplate.ObjectType == (int)eObjectType.AlchemyTincture || ItemTemplate.ObjectType == (int)eObjectType.SpellcraftGem)
            {
                WriteMagicalBonuses(delve, player.Client, false);
            }

			//***********************************
			//shows info for Poison Potions
			//***********************************
			if (ItemTemplate.ObjectType == (int)eObjectType.Poison)
			{
				WritePoisonInfo(delve, player.Client);
			}

			if (ItemTemplate.ObjectType == (int)eObjectType.Magical && ItemTemplate.ItemType == (int)eInventorySlot.FirstBackpack) // potion
			{
				WritePotionInfo(delve, player.Client);
			}
			else if (ItemTemplate.CanUseEvery > 0)
			{
				// Items with a reuse timer (aka cooldown).
				delve.Add(" ");

				int minutes = ItemTemplate.CanUseEvery / 60;
				int seconds = ItemTemplate.CanUseEvery % 60;

				if (minutes == 0)
				{
                    delve.Add(String.Format("Can use item every: {0} sec", seconds));
				}
				else
				{
                    delve.Add(String.Format("Can use item every: {0}:{1:00} min", minutes, seconds));
				}

				// delve.Add(String.Format("Can use item every: {0:00}:{1:00}", minutes, seconds));

				int cooldown = CanUseAgainIn;

				if (cooldown > 0)
				{
					minutes = cooldown / 60;
					seconds = cooldown % 60;

					if (minutes == 0)
					{
                        delve.Add(String.Format("Can use again in: {0} sec", seconds));
					}
					else
					{
                        delve.Add(String.Format("Can use again in: {0}:{1:00} min", minutes, seconds));
					}
				}
			}

			if (!ItemTemplate.IsDropable || !ItemTemplate.IsPickable || ItemTemplate.IsIndestructible)
				delve.Add(" ");

			if (!ItemTemplate.IsPickable)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.HandlePacket.CannotTraded"));

			if (!ItemTemplate.IsDropable)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.HandlePacket.CannotSold"));

			if (ItemTemplate.IsIndestructible)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.HandlePacket.CannotDestroyed"));

			if (ItemTemplate.BonusLevel > 0)
			{
				delve.Add(" ");
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.HandlePacket.BonusLevel", ItemTemplate.BonusLevel));
			}

			//Add admin info
			if (player.Client.Account.PrivLevel > 1)
			{
                WriteTechnicalInfo(delve, player.Client);
			}
		}

		protected virtual void WriteUsableClasses(IList<string> output, GameClient client)
		{
			if (Util.IsEmpty(ItemTemplate.AllowedClasses, true))
				return;

            output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteUsableClasses.UsableBy"));

			foreach (string allowed in Util.SplitCSV(ItemTemplate.AllowedClasses, true))
			{
				int classID = -1;
				if (int.TryParse(allowed, out classID))
				{
					output.Add("- " + ((eCharacterClass)classID).ToString());
				}
				else
				{
					log.Error("Item Template " + ItemTemplate.Id + " has an invalid entry for allowed classes '" + allowed + "'");
				}
			}
		}


		protected virtual void WriteMagicalBonuses(IList<string> output, GameClient client, bool shortInfo)
		{
			int oldCount = output.Count;

			foreach (var bonus in ItemTemplate.Bonuses.OrderBy(x => x.BonusOrder))
			{
				WriteBonusLine(output, client, bonus.BonusType, bonus.BonusValue);
			}

			if (output.Count > oldCount)
			{
				output.Add(" ");
                output.Insert(oldCount, LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.MagicBonus"));
				output.Insert(oldCount, " ");
			}

			oldCount = output.Count;

			foreach (var bonus in ItemTemplate.Bonuses.OrderBy(x => x.BonusOrder))
			{
				WriteFocusLine(output, bonus.BonusType, bonus.BonusValue);
			}

			if (output.Count > oldCount)
			{
				output.Add(" ");
                output.Insert(oldCount, LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.FocusBonus"));
				output.Insert(oldCount, " ");
			}

			if (!shortInfo)
			{
				if (Spells.Any(x=> !x.IsPoison))
                {
					int requiredLevel = ItemTemplate.LevelRequirement > 0 ? ItemTemplate.LevelRequirement : Math.Min(50, ItemTemplate.Level);
					if (requiredLevel > 1)
					{
						output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.LevelRequired2", requiredLevel));
						output.Add(" ");
					}
				}				

				if (ItemTemplate.ObjectType == (int)eObjectType.Magical && ItemTemplate.ItemType == (int)eInventorySlot.FirstBackpack) // potion
				{
					// let WritePotion handle the rest of the display
					return;
				}

				foreach (var itemSpell in Spells.Where(x => !x.IsPoison).OrderByDescending(x => x.ProcChance))
				{
					string spellNote = "";
					output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.MagicAbility"));
					if (GlobalConstants.IsWeapon(ItemTemplate.ObjectType))
					{
						spellNote = LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.StrikeEnemy");
					}
					else if (GlobalConstants.IsArmor(ItemTemplate.ObjectType))
					{
						spellNote = LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.StrikeArmor");
					}

					SpellLine line = SkillBase.GetSpellLine(GlobalSpellsLines.Item_Effects);
					if (line != null)
					{
						Spell spell = SkillBase.FindSpell(itemSpell.SpellID, line);

						if (spell != null)
						{
							ISpellHandler spellHandler = ScriptMgr.CreateSpellHandler(client.Player, spell, line);
							if (spellHandler != null)
							{
								if (itemSpell.MaxCharges > 0)
								{
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.ChargedMagic"));
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.Charges", itemSpell.Charges));
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.MaxCharges", itemSpell.MaxCharges));
									output.Add(" ");
								}

								Util.AddRange(output, spellHandler.DelveInfo);
								output.Add(" ");

								if (itemSpell.ProcChance <= 0)
                                {
									output.Add("- This spell is cast when the item is used.");
								}
							}
							else
							{
								output.Add("-" + spell.Name + " (Spell Handler Not Implemented)");
							}

							output.Add(spellNote);
						}
						else
						{
							output.Add("- Spell Not Found: " + itemSpell.SpellID);
						}
					}
					else
					{
						output.Add("- Item_Effects Spell Line Missing");
					}

					output.Add(" ");
				}

				#region Poison
				foreach (var itemSpell in Spells.Where(x => x.IsPoison))
				{
					if (GlobalConstants.IsWeapon(ItemTemplate.ObjectType))// Poisoned Weapon
					{
						SpellLine poisonLine = SkillBase.GetSpellLine(GlobalSpellsLines.Mundane_Poisons);
						if (poisonLine != null)
						{
							List<Spell> spells = SkillBase.GetSpellList(poisonLine.KeyName);
							foreach (Spell spl in spells)
							{
								if (spl.ID == itemSpell.SpellID)
								{
									output.Add(" ");
                                    output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.LevelRequired"));
                                    output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.Level", spl.Level));
									output.Add(" ");
                                    output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.ChargedMagic"));
                                    output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.Charges", itemSpell.Charges));
                                    output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.MaxCharges", itemSpell.MaxCharges));
									output.Add(" ");

									ISpellHandler spellHandler = ScriptMgr.CreateSpellHandler(client.Player, spl, poisonLine);
									if (spellHandler != null)
									{
										Util.AddRange(output, spellHandler.DelveInfo);
										output.Add(" ");
									}
									else
									{
										output.Add("-" + spl.Name + "(Not implemented yet)");
									}
                                    output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.StrikeEnemy"));
									return;
								}
							}
						}
					}

					SpellLine chargeEffectsLine = SkillBase.GetSpellLine(GlobalSpellsLines.Item_Effects);
					if (chargeEffectsLine != null)
					{
						List<Spell> spells = SkillBase.GetSpellList(chargeEffectsLine.KeyName);
						foreach (Spell spl in spells)
						{
							if (spl.ID == itemSpell.SpellID)
							{
								output.Add(" ");
                                output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.LevelRequired"));
                                output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.Level", spl.Level));
								output.Add(" ");
								if (itemSpell.MaxCharges > 0)
								{
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.ChargedMagic"));
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.Charges", itemSpell.Charges));
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.MaxCharges", itemSpell.MaxCharges));
								}
								else
								{
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.MagicAbility"));
								}
								output.Add(" ");

								ISpellHandler spellHandler = ScriptMgr.CreateSpellHandler(client.Player, spl, chargeEffectsLine);
								if (spellHandler != null)
								{
									Util.AddRange(output, spellHandler.DelveInfo);
									output.Add(" ");
								}
								else
								{
									output.Add("-" + spl.Name + "(Not implemented yet)");
								}
								output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.UsedItem"));
								output.Add(" ");
								if (spl.RecastDelay > 0)
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.UseItem1", Util.FormatTime(spl.RecastDelay / 1000)));
								else
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.UseItem2"));
								long lastChargedItemUseTick = client.Player.TempProperties.getProperty<long>(GamePlayer.LAST_CHARGED_ITEM_USE_TICK);
								long changeTime = client.Player.CurrentRegion.Time - lastChargedItemUseTick;
								long recastDelay = (spl.RecastDelay > 0) ? spl.RecastDelay : 60000 * 3;
								if (changeTime < recastDelay) //3 minutes reuse timer
									output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.UseItem3", Util.FormatTime((recastDelay - changeTime) / 1000)));
								return;
							}
						}
					}
				}
				#endregion
			}
		}


        protected virtual void WriteBonusLine(IList<string> list, GameClient client, int bonusCat, int bonusValue)
		{
			if (bonusCat != 0 && bonusValue != 0 && !SkillBase.CheckPropertyType((eProperty)bonusCat, ePropertyType.Focus))
			{
				if (IsPvEBonus((eProperty)bonusCat))
				{
					// Evade: {0}% (PvE Only)
					list.Add(string.Format(SkillBase.GetPropertyName((eProperty)bonusCat), bonusValue));
				}
				else
				{
					//- Axe: 5 pts
					//- Strength: 15 pts
					//- Constitution: 15 pts
					//- Hits: 40 pts
					//- Fatigue: 8 pts
					//- Heat: 7%
					//Bonus to casting speed: 2%
					//Bonus to armor factor (AF): 18
					//Power: 6 % of power pool.
					list.Add(string.Format(
						"- {0}: {1}{2}",
						SkillBase.GetPropertyName((eProperty)bonusCat),
						bonusValue.ToString("+0 ;-0 ;0 "), //Eden
						((bonusCat == (int)eProperty.PowerPool)
						 || (bonusCat >= (int)eProperty.Resist_First && bonusCat <= (int)eProperty.Resist_Last)
						 || (bonusCat >= (int)eProperty.ResCapBonus_First && bonusCat <= (int)eProperty.ResCapBonus_Last)
						 || bonusCat == (int)eProperty.Conversion
						 || bonusCat == (int)eProperty.ExtraHP
						 || bonusCat == (int)eProperty.RealmPoints
						 || bonusCat == (int)eProperty.StyleAbsorb
						 || bonusCat == (int)eProperty.ArcaneSyphon
						 || bonusCat == (int)eProperty.BountyPoints
						 || bonusCat == (int)eProperty.XpPoints)
                        ? ((bonusCat == (int)eProperty.PowerPool) ? LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteBonusLine.PowerPool") : "%")
                        : LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteBonusLine.Points")
					));
				}
			}
		}

		protected virtual void WriteFocusLine(IList<string> list, int focusCat, int focusLevel)
		{
			if (SkillBase.CheckPropertyType((eProperty)focusCat, ePropertyType.Focus))
			{
				//- Body Magic: 4 lvls
				list.Add(string.Format("- {0}: {1} lvls", SkillBase.GetPropertyName((eProperty)focusCat), focusLevel));
			}
		}


		protected virtual bool IsPvEBonus(eProperty property)
		{
			switch (property)
			{
				case eProperty.DefensiveBonus:
				case eProperty.BladeturnReinforcement:
				case eProperty.NegativeReduction:
				case eProperty.PieceAblative:
				case eProperty.ReactionaryStyleDamage:
				case eProperty.SpellPowerCost:
				case eProperty.StyleCostReduction:
				case eProperty.ToHitBonus:
					return true;

				default:
					return false;
			}
		}


		protected virtual void WritePoisonInfo(IList<string> list, GameClient client)
		{
			foreach (var itemSpell in Spells.Where(x => x.IsPoison))
			{
				SpellLine poisonLine = SkillBase.GetSpellLine(GlobalSpellsLines.Mundane_Poisons);
				if (poisonLine != null)
				{
					List<Spell> spells = SkillBase.GetSpellList(poisonLine.KeyName);

					foreach (Spell spl in spells)
					{
						if (spl.ID == itemSpell.SpellID)
						{
							list.Add(" ");
							list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePoisonInfo.LevelRequired"));
							list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePoisonInfo.Level", spl.Level));
							list.Add(" ");
							list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePoisonInfo.ProcAbility"));
							list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePoisonInfo.Charges", itemSpell.Charges));
							list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePoisonInfo.MaxCharges", itemSpell.MaxCharges));
							list.Add(" ");

							ISpellHandler spellHandler = ScriptMgr.CreateSpellHandler(client.Player, spl, poisonLine);
							if (spellHandler != null)
							{
								Util.AddRange(list, spellHandler.DelveInfo);
							}
							else
							{
								list.Add("-" + spl.Name + " (Not implemented yet)");
							}
							break;
						}
					}
				}
			}
		}


		protected virtual void WritePotionInfo(IList<string> list, GameClient client)
		{
			foreach (var itemSpell in Spells)
			{
				SpellLine potionLine = SkillBase.GetSpellLine(GlobalSpellsLines.Potions_Effects);
				if (potionLine != null)
				{
					List<Spell> spells = SkillBase.GetSpellList(potionLine.KeyName);

					foreach (Spell spl in spells)
					{
						if (spl.ID == itemSpell.SpellID)
						{
							list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.ChargedMagic"));
							list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.Charges", itemSpell.Charges));
							list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.MaxCharges", itemSpell.MaxCharges));
							list.Add(" ");
							WritePotionSpellsInfos(list, client, spl, potionLine);
							list.Add(" ");
							long nextPotionAvailTime = client.Player.TempProperties.getProperty<long>("LastPotionItemUsedTick_Type" + spl.SharedTimerGroup);
							// Satyr Update: Individual Reuse-Timers for Pots need a Time looking forward
							// into Future, set with value of "itemtemplate.CanUseEvery" and no longer back into past
							if (nextPotionAvailTime > client.Player.CurrentRegion.Time)
							{
								list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.UseItem3", Util.FormatTime((nextPotionAvailTime - client.Player.CurrentRegion.Time) / 1000)));
							}
							else
							{
								int minutes = ItemTemplate.CanUseEvery / 60;
								int seconds = ItemTemplate.CanUseEvery % 60;

								if (minutes == 0)
								{
                                    list.Add(String.Format("Can use item every: {0} sec", seconds));
								}
								else
								{
                                    list.Add(String.Format("Can use item every: {0}:{1:00} min", minutes, seconds));
								}
							}

							if (spl.CastTime > 0)
							{
								list.Add(" ");
								list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.NoUseInCombat"));
							}
							break;
						}
					}
				}
			}
		}


		protected static void WritePotionSpellsInfos(IList<string> list, GameClient client, Spell spl, NamedSkill line)
		{
			if (spl != null)
			{
				list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteMagicalBonuses.MagicAbility"));
				list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.Type", spl.SpellType));
				list.Add(" ");
				list.Add(spl.Description);
				list.Add(" ");
				if (spl.Value != 0)
				{
					list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.Value", spl.Value));
				}
				list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.Target", spl.Target));
				if (spl.Range > 0)
				{
					list.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WritePotionInfo.Range", spl.Range));
				}
				list.Add(" ");
				list.Add(" ");
				if (spl.SubSpellID > 0)
				{
					List<Spell> spells = SkillBase.GetSpellList(line.KeyName);
					foreach (Spell subSpell in spells)
					{
						if (subSpell.ID == spl.SubSpellID)
						{
							WritePotionSpellsInfos(list, client, subSpell, line);
							break;
						}
					}
				}
			}
		}


		protected virtual void DelveShieldStats(IList<string> output, GameClient client)
		{
			double itemDPS = ItemTemplate.DPS_AF / 10.0;
			double clampedDPS = Math.Min(itemDPS, 1.2 + 0.3 * client.Player.Level);
			double itemSPD = ItemTemplate.SPD_ABS / 10.0;

			output.Add(" ");
			output.Add(" ");
			output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteClassicShieldInfos.DamageMod"));
			if (itemDPS != 0)
			{
				output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteClassicShieldInfos.BaseDPS", itemDPS.ToString("0.0")));
				output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteClassicShieldInfos.ClampDPS", clampedDPS.ToString("0.0")));
			}
			if (ItemTemplate.SPD_ABS >= 0)
			{
				output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteClassicShieldInfos.SPD", itemSPD.ToString("0.0")));
			}

			output.Add(" ");

			switch (ItemTemplate.TypeDamage)
			{
					case 1: output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteClassicShieldInfos.Small")); break;
					case 2: output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteClassicShieldInfos.Medium")); break;
					case 3: output.Add(LanguageMgr.GetTranslation(client.Account.Language, "DetailDisplayHandler.WriteClassicShieldInfos.Large")); break;
			}
		}


		protected virtual void DelveWeaponStats(List<String> delve, GamePlayer player)
		{
			double itemDPS = ItemTemplate.DPS_AF / 10.0;
			double clampedDPS = Math.Min(itemDPS, 1.2 + 0.3 * player.Level);
			double itemSPD = ItemTemplate.SPD_ABS / 10.0;
			double effectiveDPS = clampedDPS * ItemTemplate.Quality / 100.0 * Condition / ItemTemplate.MaxCondition;

			delve.Add(" ");
			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicWeaponInfos.DamageMod"));

			if (itemDPS != 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicWeaponInfos.BaseDPS", itemDPS.ToString("0.0")));
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicWeaponInfos.ClampDPS", clampedDPS.ToString("0.0")));
			}

			if (ItemTemplate.SPD_ABS >= 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicWeaponInfos.SPD", itemSPD.ToString("0.0")));
			}

			if (ItemTemplate.Quality != 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicWeaponInfos.Quality", ItemTemplate.Quality));
			}

			if (Condition != 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicWeaponInfos.Condition", ConditionPercent));
			}

			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
			                                     "DetailDisplayHandler.WriteClassicWeaponInfos.DamageType",
			                                     (ItemTemplate.TypeDamage == 0 ? "None" : GlobalConstants.WeaponDamageTypeToName(ItemTemplate.TypeDamage))));

			delve.Add(" ");

			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicWeaponInfos.EffDamage"));

			if (itemDPS != 0)
			{
				delve.Add("- " + effectiveDPS.ToString("0.0") + " DPS");
			}
		}

		protected virtual void DelveArmorStats(List<String> delve, GamePlayer player)
		{
			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.ArmorMod"));

			double af = 0;
			int afCap = player.Level + (player.RealmLevel > 39 ? 1 : 0);
			double effectiveAF = 0;

			if (ItemTemplate.DPS_AF != 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.BaseFactor", ItemTemplate.DPS_AF));

				if (ItemTemplate.ObjectType != (int)eObjectType.Cloth)
				{
					afCap *= 2;
				}

				af = Math.Min(afCap, ItemTemplate.DPS_AF);

				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.ClampFact", (int)af));
			}

			if (ItemTemplate.SPD_ABS >= 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.Absorption", ItemTemplate.SPD_ABS));
			}

			if (ItemTemplate.Quality != 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.Quality", ItemTemplate.Quality));
			}

			if (Condition != 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.Condition", ConditionPercent));
			}

			delve.Add(" ");
			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.EffArmor"));

			if (ItemTemplate.DPS_AF != 0)
			{
				effectiveAF = af * ItemTemplate.Quality / 100.0 * Condition / ItemTemplate.MaxCondition * (1 + ItemTemplate.SPD_ABS / 100.0);
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.Factor", (int)effectiveAF));
			}
		}

		/// <summary>
		/// Write item technical info
		/// </summary>
		/// <param name="output"></param>
		/// <param name="item"></param>
		public virtual void WriteTechnicalInfo(List<String> delve, GameClient client)
		{
			delve.Add("");
			delve.Add("--- Technical Information ---");
			delve.Add("");

			if (ItemTemplate is ItemUnique)
			{
				delve.Add("  Item Unique: " + ItemTemplate.Id);
			}
			else
			{
				delve.Add("Item Template: " + ItemTemplate.Id);
				delve.Add("Allow Updates: " + (ItemTemplate as ItemTemplate).AllowUpdate);
			}

			delve.Add("");

			delve.Add("         Name: " + Name);
			delve.Add("    ClassType: " + this.GetType().FullName);
			delve.Add("");
			delve.Add(" SlotPosition: " + SlotPosition);
			if (OwnerLot != 0 || SellPrice != 0)
			{
				delve.Add("    Owner Lot: " + OwnerLot);
				delve.Add("   Sell Price: " + SellPrice);
			}
			delve.Add("");
			delve.Add("        Level: " + ItemTemplate.Level);
			delve.Add("       Object: " + GlobalConstants.ObjectTypeToName(ItemTemplate.ObjectType) + " (" + ItemTemplate.ObjectType + ")");
			delve.Add("         Type: " + GlobalConstants.SlotToName(ItemTemplate.ItemType) + " (" + ItemTemplate.ItemType + ")");
			delve.Add("");
			delve.Add("        Model: " + ItemTemplate.Model);
			delve.Add("    Extension: " + Extension);
			delve.Add("        Color: " + Color);
			delve.Add("       Emblem: " + Emblem);
			delve.Add("       Effect: " + ItemTemplate.Effect);
			delve.Add("");
			delve.Add("       DPS_AF: " + ItemTemplate.DPS_AF);
			delve.Add("      SPD_ABS: " + ItemTemplate.SPD_ABS);
			delve.Add("         Hand: " + ItemTemplate.Hand);
			delve.Add("  TypeDamage: " + ItemTemplate.TypeDamage);
			delve.Add("        Bonus: " + ItemTemplate.ItemBonus);

			if (GlobalConstants.IsWeapon(ItemTemplate.ObjectType))
			{
				delve.Add("");
				delve.Add("         Hand: " + GlobalConstants.ItemHandToName(ItemTemplate.Hand) + " (" + ItemTemplate.Hand + ")");
				delve.Add("Damage/Second: " + (ItemTemplate.DPS_AF / 10.0f));
				delve.Add("        Speed: " + (ItemTemplate.SPD_ABS / 10.0f));
				delve.Add("  Damage type: " + GlobalConstants.WeaponDamageTypeToName(ItemTemplate.TypeDamage) + " (" + ItemTemplate.TypeDamage + ")");
				delve.Add("        Bonus: " + ItemTemplate.ItemBonus);
			}
			else if (GlobalConstants.IsArmor(ItemTemplate.ObjectType))
			{
				delve.Add("");
				delve.Add("  Armorfactor: " + ItemTemplate.DPS_AF);
				delve.Add("   Absorption: " + ItemTemplate.SPD_ABS);
				delve.Add("        Bonus: " + ItemTemplate.ItemBonus);
			}
			else if (ItemTemplate.ObjectType == (int)eObjectType.Shield)
			{
				delve.Add("");
				delve.Add("Damage/Second: " + (ItemTemplate.DPS_AF / 10.0f));
				delve.Add("        Speed: " + (ItemTemplate.SPD_ABS / 10.0f));
				delve.Add("  Shield type: " + GlobalConstants.ShieldTypeToName(ItemTemplate.TypeDamage) + " (" + ItemTemplate.TypeDamage + ")");
				delve.Add("        Bonus: " + ItemTemplate.ItemBonus);
			}
			else if (ItemTemplate.ObjectType == (int)eObjectType.Arrow || ItemTemplate.ObjectType == (int)eObjectType.Bolt)
			{
				delve.Add("");
				delve.Add(" Ammunition #: " + ItemTemplate.DPS_AF);
				delve.Add("       Damage: " + GlobalConstants.AmmunitionTypeToDamageName(ItemTemplate.SPD_ABS));
				delve.Add("        Range: " + GlobalConstants.AmmunitionTypeToRangeName(ItemTemplate.SPD_ABS));
				delve.Add("     Accuracy: " + GlobalConstants.AmmunitionTypeToAccuracyName(ItemTemplate.SPD_ABS));
				delve.Add("        Bonus: " + ItemTemplate.ItemBonus);
			}
			else if (ItemTemplate.ObjectType == (int)eObjectType.Instrument)
			{
				delve.Add("");
				delve.Add("   Instrument: " + GlobalConstants.InstrumentTypeToName(ItemTemplate.DPS_AF));
			}

			if (OwnerLot != 0)
			{
				delve.Add("");
				delve.Add("   Owner Lot#: " + OwnerLot);
				delve.Add("   Sell Price: " + SellPrice);
			}

			delve.Add("");
            delve.Add("   Value/Price: " + Money.GetShortString(ItemTemplate.Price) + " / " + Money.GetShortString((long)(ItemTemplate.Price * (long)ServerProperties.Properties.ITEM_SELL_RATIO * .01)));
			delve.Add("Count/MaxCount: " + Count + " / " + ItemTemplate.MaxCount);
			delve.Add("        Weight: " + (ItemTemplate.Weight / 10.0f) + "lbs");
			delve.Add("       Quality: " + ItemTemplate.Quality + "%");
			delve.Add("    Durability: " + Durability + "/" + ItemTemplate.MaxDurability);
			delve.Add("     Condition: " + Condition + "/" + ItemTemplate.MaxCondition);
			delve.Add("         Realm: " + ItemTemplate.Realm);
			delve.Add("");
			delve.Add("   Is dropable: " + (ItemTemplate.IsDropable ? "yes" : "no"));
			delve.Add("   Is pickable: " + (ItemTemplate.IsPickable ? "yes" : "no"));
			delve.Add("   Is tradable: " + (ItemTemplate.IsTradable ? "yes" : "no"));
			delve.Add("  Is alwaysDUR: " + (ItemTemplate.IsNotLosingDur ? "yes" : "no"));
			delve.Add(" Is Indestruct: " + (ItemTemplate.IsIndestructible ? "yes" : "no"));
			delve.Add("  Is stackable: " + (ItemTemplate.IsStackable ? "yes (" + ItemTemplate.MaxCount + ")" : "no"));
			delve.Add("");

			foreach (var spell in Spells.Where(x => !x.IsPoison && x.ProcChance > 0))
			{
				delve.Add("   ProcSpellID: " + spell.SpellID);
				delve.Add("    ProcChance: " + spell.ProcChance);
			}

			foreach (var spell in Spells.Where(x => !x.IsPoison && x.ProcChance <= 0))
			{
				delve.Add("       SpellID: " + spell.SpellID + " (" + spell.Charges + "/" + spell.MaxCharges + ")");
			}

			foreach (var spell in Spells.Where(x => x.IsPoison))
			{
				delve.Add(" PoisonSpellID: " + spell.SpellID + " (" + spell.Charges + "/" + spell.MaxCharges + ") ");
			}

			delve.Add("");
			delve.Add("AllowedClasses: " + ItemTemplate.AllowedClasses);
			delve.Add(" LevelRequired: " + ItemTemplate.LevelRequirement);
			delve.Add("    BonusLevel: " + ItemTemplate.BonusLevel);
			delve.Add(" ");
			delve.Add("              Flags: " + ItemTemplate.Flags);
			delve.Add("     SalvageYieldID: " + ItemTemplate.SalvageYieldID);
			delve.Add("          PackageID: " + ItemTemplate.PackageID);
			delve.Add("Requested ClassType: " + ItemTemplate.ClassType);
		}
	}
}
