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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DOL.GS;
using log4net;
using System.Reflection;
using Atlas.DataLayer.Models;
using Atlas.DataLayer;
using DOL.Events;
using DOL.GS.PacketHandler;
using DOL.Language;

namespace DOL.GS
{
	/// <summary>
	/// An artifact inside an inventory.
	/// </summary>
	/// <author>Aredhel</author>
	public class InventoryArtifact : GameInventoryItem
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private int m_artifactID;
		private int m_artifactLevel;
		private int[] m_levelRequirements;

		/// <summary>
		/// This constructor shouldn't be called, so we prevent anyone
		/// from using it.
		/// </summary>
		private InventoryArtifact() { }

		/// <summary>
		/// Create a new inventory artifact from an item template.
		/// </summary>
		/// <param name="template"></param>
		public InventoryArtifact(ItemTemplate template)
			: base(template)
			
		{
			ItemTemplate = ItemTemplate.Clone() as ItemTemplate;
			ArtifactID = ArtifactMgr.GetArtifactIDFromItemID(template.Id) ?? 0;
			ArtifactLevel = 0;
			m_levelRequirements = ArtifactMgr.GetLevelRequirements(ArtifactID);			

			for (var bonusID = eArtifactBonus.Min; bonusID <= eArtifactBonus.Max; ++bonusID)
			{
				// Clear all bonuses except the base (L0) bonuses.

				if (m_levelRequirements[(int)bonusID] > 0)
				{
					var bonus = ItemTemplate.Bonuses.FirstOrDefault(x => x.BonusOrder == (int)bonusID);
					
					if (bonus != null)
                    {
						ItemTemplate.Bonuses.Remove(bonus);
                    }
				}
			}
		}

		/// <summary>
		/// Create a new inventory artifact from an existing inventory item.
		/// </summary>
		/// <param name="item"></param>
		public InventoryArtifact(InventoryItem item)
			: base(item)
		{
			if (item != null)
			{
				// We want a new copy from the DB to avoid everyone sharing the same template
				var template = item.ItemTemplate != null ? item.ItemTemplate.Clone() as ItemTemplate : item.ItemTemplate;

				if (template == null)
				{
					log.ErrorFormat("Artifact: Error loading artifact for owner {0} holding item {1} with an id_nb of {2}", item.CharacterID, item.Name, item.Id);
					return;
				}

				this.ItemTemplate = template;
				this.Id = item.Id;	// This is the key for the 'inventoryitem' table
				this.CharacterID = item.CharacterID;
				ItemTemplate.CanUseEvery = ArtifactMgr.GetReuseTimer(this);
				ArtifactID = ArtifactMgr.GetArtifactIDFromItemID(item.Id) ?? 0;
				ArtifactLevel = ArtifactMgr.GetCurrentLevel(this);
				m_levelRequirements = ArtifactMgr.GetLevelRequirements(ArtifactID);
				UpdateAbilities(template);
			}
		}


		/// <summary>
		/// The ID of this artifact.
		/// </summary>
		public int ArtifactID
		{
			get { return m_artifactID; }
			protected set { m_artifactID = value; }
		}

		/// <summary>
		/// The level of this artifact.
		/// </summary>
		public int ArtifactLevel
		{
			get { return m_artifactLevel; }
			set { m_artifactLevel = value; }
		}

		/// <summary>
		/// Called from ArtifactMgr when this artifact has gained a level.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="artifactLevel"></param>
		public void OnLevelGained(GamePlayer player, int artifactLevel)
		{
			if (artifactLevel > ArtifactLevel && artifactLevel <= 10)
			{
				ArtifactLevel = artifactLevel;
				if (AddAbilities(player, ArtifactLevel) && player != null)
					player.Out.SendMessage(String.Format("Your {0} has gained a new ability!", Name), eChatType.CT_Important, eChatLoc.CL_SystemWindow);
			}
		}

		/// <summary>
		/// Repair cost for this artifact in the current state.
		/// </summary>
		public override long RepairCost
		{
			get
			{
				return (ItemTemplate.MaxCondition - Condition) / 100;
			}
		}

		/// <summary>
		/// Verify that this artifact has all the correct abilities
		/// </summary>
		public void UpdateAbilities(ItemTemplate template)
		{
			if (template == null)
				return;

			for (var bonusID = eArtifactBonus.Min; bonusID <= eArtifactBonus.Max; ++bonusID)
			{
				var bonus = this.Bonuses.FirstOrDefault(x => x.BonusOrder == (int)bonusID);

				if (m_levelRequirements[(int)bonusID] <= ArtifactLevel)
				{
					if (bonus == null)
                    {
						var templateBonus = template.Bonuses.FirstOrDefault(x => x.BonusOrder == (int)bonusID);
						if (templateBonus == null)
							continue;

						this.Bonuses.Add(new ItemBonus()
						{
							BonusOrder = (int)bonusID,
							BonusType = templateBonus.BonusType,
							BonusValue = templateBonus.BonusValue,
							
						});
                    }					
				}
				else
				{
					if (bonus != null)
                    {
						template.Bonuses.Remove(bonus);
                    }
				}
			}
		}

		/// <summary>
		/// Add all abilities for this level.
		/// </summary>
		/// <param name="artifactLevel">The level to add abilities for.</param>
		/// <param name="player"></param>
		/// <returns>True, if artifact gained 1 or more abilities, else false.</returns>
		private bool AddAbilities(GamePlayer player, int artifactLevel)
		{
			ItemTemplate template = GameServer.Database.ItemTemplates.Find(this.Id);

			if (template == null)
			{
				log.Warn(String.Format("Item template missing for artifact '{0}'", Name));
				return false;
			}

			bool abilityGained = false;

			for (var bonusID = eArtifactBonus.Min; bonusID <= eArtifactBonus.Max; ++bonusID)
			{
				if (m_levelRequirements[(int)bonusID] == artifactLevel)
				{
					var bonus = this.Bonuses.FirstOrDefault(x => x.BonusOrder == (int)bonusID);
					var templateBonus = template.Bonuses.FirstOrDefault(x => x.BonusOrder == (int)bonusID);
					if (bonus == null && templateBonus != null)
					{
						bonus = new ItemBonus()
						{
							BonusOrder = (int)bonusID,
							BonusType = templateBonus.BonusType,
							BonusValue = templateBonus.BonusValue,

						};

						this.Bonuses.Add(bonus);

						if (bonusID <= eArtifactBonus.MaxStat)
							player.Notify(PlayerInventoryEvent.ItemBonusChanged, this, new ItemBonusChangedEventArgs(bonus.BonusType, bonus.BonusValue));

						abilityGained = true;
					}
				}
			}

			return abilityGained;
		}

		#region Delve

		/// <summary>
		/// Artifact delve information.
		/// </summary>
		public override void Delve(List<String> delve, GamePlayer player)
		{
			if (player == null)
				return;

			// Artifact specific information.

			if (ArtifactLevel < 10)
			{
				delve.Add(string.Format("Artifact (Current level: {0})", ArtifactLevel));
				delve.Add(string.Format("- {0}% exp earned towards level {1}",
				                        ArtifactMgr.GetXPGainedForLevel(this), ArtifactLevel + 1));
				delve.Add(string.Format("- Artifact will gain new abilities at level {0}",
				                        GainsNewAbilityAtLevel()));
				delve.Add(string.Format("(Earns exp: {0})",
				                        ArtifactMgr.GetEarnsXP(this)));
			}
			else
			{
				delve.Add("Artifact:");
				delve.Add("Current level: 10");
			}

			// Item bonuses.

			delve.Add("");
			delve.Add("Magical Bonuses:");

			foreach (var bonus in this.Bonuses.OrderBy(x => x.BonusOrder))
            {
				DelveMagicalBonus(delve, bonus.BonusValue, bonus.BonusType, m_levelRequirements[bonus.BonusOrder]);
			}

			foreach (var bonus in this.Bonuses.OrderBy(x => x.BonusOrder))
			{
				DelveFocusBonus(delve, bonus.BonusValue, bonus.BonusType, m_levelRequirements[bonus.BonusOrder]);
			}

			delve.Add("");

			foreach (var bonus in this.Bonuses.OrderBy(x => x.BonusOrder))
			{
				DelveBonus(delve, bonus.BonusValue, bonus.BonusType, m_levelRequirements[bonus.BonusOrder]);
			}

			// Spells & Procs

			foreach (var spell in this.Spells)
            {
				DelveMagicalAbility(delve, spell, m_levelRequirements[(int)eArtifactBonus.Spell]);
			}

			delve.Add("");

			// Weapon & Armor Stats.

			if ((ItemTemplate.ObjectType >= (int)eObjectType.GenericWeapon) &&
			    (ItemTemplate.ObjectType <= (int)eObjectType.MaulerStaff) ||
			    (ItemTemplate.ObjectType == (int)eObjectType.Instrument))
				DelveWeaponStats(delve, player);
			else if (ItemTemplate.ObjectType >= (int)eObjectType.Cloth &&
					 ItemTemplate.ObjectType <= (int)eObjectType.Scale)
				DelveArmorStats(delve, player);

			// Reuse Timer

			int reuseTimer = ArtifactMgr.GetReuseTimer(this);
			
			if (reuseTimer > 0)
			{
				TimeSpan reuseTimeSpan = new TimeSpan(0, 0, reuseTimer);
				delve.Add("");
				delve.Add(String.Format("Can use item every {0} min",
				                        reuseTimeSpan.ToString().Substring(3)));
			}

			if (player.Client.Account.PrivLevel > 1)
                WriteTechnicalInfo(delve, player.Client);
			
		}

		/// <summary>
		/// Artifact classic magical bonus delve information.
		/// </summary>
		/// <param name="delve"></param>
		/// <param name="bonusAmount"></param>
		/// <param name="bonusType"></param>
		/// <param name="levelRequirement"></param>
		protected virtual void DelveMagicalBonus(List<String> delve, int bonusAmount, int bonusType, int levelRequirement)
		{
			String levelTag = (levelRequirement > 0)
				? String.Format("[L{0}]: ", levelRequirement)
				: "";

			if (IsStatBonus(bonusType) || IsSkillBonus(bonusType))
			{
				delve.Add(String.Format("- {0}{1}: {2} pts",
				                        levelTag,
				                        SkillBase.GetPropertyName((eProperty)bonusType),
				                        bonusAmount.ToString("+0;-0;0")));
			}
			else if (IsResistBonus(bonusType))
			{
				delve.Add(String.Format("- {0}{1}: {2}%",
				                        levelTag,
				                        SkillBase.GetPropertyName((eProperty)bonusType),
				                        bonusAmount.ToString("+0;-0;0")));
			}
			else if (bonusType == (int)eProperty.PowerPool)
			{
				delve.Add(String.Format("- {0}{1}: {2}% of power pool.",
				                        levelTag,
				                        SkillBase.GetPropertyName((eProperty)bonusType),
				                        bonusAmount.ToString("+0;-0;0")));
			}
		}


		protected virtual void DelveFocusBonus(List<String> delve, int bonusAmount, int bonusType, int levelRequirement)
		{
			String levelTag = (levelRequirement > 0)
				? String.Format("[L{0}]: ", levelRequirement)
				: "";

			bool addedFocusDescription = false;

			if (IsFocusBonus(bonusType))
			{
				if (!addedFocusDescription)
				{
					delve.Add("");
					delve.Add("Focus Bonuses:");
					addedFocusDescription = true;
				}

				delve.Add(String.Format("- {0}{1}: {2} lvls",
				                        levelTag,
				                        SkillBase.GetPropertyName((eProperty)bonusType),
				                        bonusAmount));
			}
		}

		/// <summary>
		/// Artifact ToA magical bonus delve information.
		/// </summary>
		/// <param name="delve"></param>
		/// <param name="bonusAmount"></param>
		/// <param name="bonusType"></param>
		/// <param name="levelRequirement"></param>
		protected virtual void DelveBonus(List<String> delve, int bonusAmount, int bonusType,
		                                  int levelRequirement)
		{
			if (!IsToABonus(bonusType))
				return;

			String levelTag = (levelRequirement > 0)
				? String.Format("[L{0}]: ", levelRequirement)
				: "";

			if (IsCapIncreaseBonus(bonusType) || bonusType == (int)eProperty.ArmorFactor)
				delve.Add(String.Format("{0}{1}: {2}",
				                        levelTag,
				                        SkillBase.GetPropertyName((eProperty)bonusType),
				                        bonusAmount));
			else
				delve.Add(String.Format("{0}{1}: {2}%",
				                        levelTag,
				                        SkillBase.GetPropertyName((eProperty)bonusType),
				                        bonusAmount));
		}

		/// <summary>
		/// Artifact Magical Ability delve information (spells, procs).
		/// </summary>
		/// <param name="delve"></param>
		/// <param name="bonusID"></param>
		/// <param name="levelRequirement"></param>
		public virtual void DelveMagicalAbility(List<String> delve, InventoryItemSpell ability, int levelRequirement)
		{
			String levelTag = (levelRequirement > 0)
				? String.Format("[L{0}]: ", levelRequirement)
				: "";		

			if (ability == null || ability.SpellID <= 0)
				return;

			delve.Add("");
			delve.Add($"{levelTag}Magical Ability:");

			SpellLine spellLine = SkillBase.GetSpellLine(GlobalSpellsLines.Item_Effects);
			if (spellLine != null)
			{
				List<Spell> spells = SkillBase.GetSpellList(spellLine.KeyName);
				foreach (Spell spell in spells)
					if (spell.ID == ability.SpellID)
						spell.Delve(delve);
			}

			if (ability.ProcChance > 0)
				delve.Add(String.Format("- Spell has a chance of casting when this {0} enemy.",
				                        (GlobalConstants.IsWeapon(ItemTemplate.ObjectType))
				                        ? "weapon strikes an" : "armor is hit by"));
			else
				delve.Add("- This spell is cast when the item is used.");
		}

		/// <summary>
		/// Artifact weapon stats delve info.
		/// </summary>
		/// <param name="delve"></param>
		/// <param name="player"></param>
		protected override void DelveWeaponStats(List<String> delve, GamePlayer player)
		{
			double itemDPS = ItemTemplate.DPS_AF / 10.0;
			double clampedDPS = Math.Min(itemDPS, 1.2 + 0.3 * player.Level);
			double itemSPD = ItemTemplate.SPD_ABS / 10.0;
			double effectiveDPS = itemDPS * ItemTemplate.Quality / 100.0 * Condition / ItemTemplate.MaxCondition;

			delve.Add("Damage Modifiers:");

			if (itemDPS != 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicWeaponInfos.BaseDPS", itemDPS.ToString("0.0")));
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicWeaponInfos.ClampDPS", clampedDPS.ToString("0.0")));
			}

			if (ItemTemplate.SPD_ABS >= 0)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicWeaponInfos.SPD", itemSPD.ToString("0.0")));

			if (ItemTemplate.Quality != 0)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicWeaponInfos.Quality", ItemTemplate.Quality));

			if (Condition != 0)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicWeaponInfos.Condition", ConditionPercent));

			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
			                                     "DetailDisplayHandler.WriteClassicWeaponInfos.DamageType",
			                                     (ItemTemplate.TypeDamage == 0 ? "None" : GlobalConstants.WeaponDamageTypeToName(ItemTemplate.TypeDamage))));
			delve.Add(" ");

			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
			                                     "DetailDisplayHandler.WriteClassicWeaponInfos.EffDamage"));
			
			if (itemDPS != 0)
				delve.Add("- " + effectiveDPS.ToString("0.0") + " DPS");
		}

		/// <summary>
		/// Artifact armor stats delve info.
		/// </summary>
		/// <param name="delve"></param>
		/// <param name="player"></param>
		protected override void DelveArmorStats(List<String> delve, GamePlayer player)
		{
			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "DetailDisplayHandler.WriteClassicArmorInfos.ArmorMod"));

			double af = 0;
			int afCap = player.Level;
			double effectiveAF = 0;

			if (ItemTemplate.DPS_AF != 0)
			{
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicArmorInfos.BaseFactor", ItemTemplate.DPS_AF));

				if (ItemTemplate.ObjectType != (int)eObjectType.Cloth)
					afCap *= 2;

				af = Math.Min(afCap, ItemTemplate.DPS_AF);

				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicArmorInfos.ClampFact", (int)af));
			}

			if (ItemTemplate.SPD_ABS >= 0)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicArmorInfos.Absorption", ItemTemplate.SPD_ABS));

			if (ItemTemplate.Quality != 0)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicArmorInfos.Quality", ItemTemplate.Quality));

			if (Condition != 0)
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicArmorInfos.Condition", ConditionPercent));

			delve.Add(" ");
			delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
			                                     "DetailDisplayHandler.WriteClassicArmorInfos.EffArmor"));

			if (ItemTemplate.DPS_AF != 0)
			{
				effectiveAF = af * ItemTemplate.Quality / 100.0 * Condition / ItemTemplate.MaxCondition * (1 + ItemTemplate.SPD_ABS / 100.0);
				delve.Add(LanguageMgr.GetTranslation(player.Client.Account.Language,
				                                     "DetailDisplayHandler.WriteClassicArmorInfos.Factor", (int)effectiveAF));
			}
		}

		/// <summary>
		/// Write item technical info
		/// </summary>
		/// <param name="output"></param>
		/// <param name="item"></param>
        public override void WriteTechnicalInfo(List<String> delve, GameClient client)
		{
			delve.Add(" ");
			delve.Add("--- Artifact/Item technical information ---");
			delve.Add(" ");
			delve.Add("Item Template: " + ItemTemplate.Id);
			delve.Add("         Name: " + Name);
			delve.Add("   Experience: " + Experience);
			delve.Add("       Object: " + GlobalConstants.ObjectTypeToName(ItemTemplate.ObjectType) + " (" + ItemTemplate.ObjectType + ")");
			delve.Add("         Type: " + GlobalConstants.SlotToName(ItemTemplate.ItemType) + " (" + ItemTemplate.ItemType + ")");
			delve.Add("    Extension: " + Extension);
			delve.Add("        Model: " + ItemTemplate.Model);
			delve.Add("        Color: " + Color);
			delve.Add("       Emblem: " + Emblem);
			delve.Add("       Effect: " + ItemTemplate.Effect);
			delve.Add("  Value/Price: " + Money.GetShortString(ItemTemplate.Price));
			delve.Add("       Weight: " + (ItemTemplate.Weight / 10.0f) + "lbs");
			delve.Add("      Quality: " + ItemTemplate.Quality + "%");
			delve.Add("   Durability: " + Durability + "/" + ItemTemplate.MaxDurability + "(max)");
			delve.Add("    Condition: " + Condition + "/" + ItemTemplate.MaxCondition + "(max)");
			delve.Add("        Realm: " + ItemTemplate.Realm);
			delve.Add("  Is dropable: " + (ItemTemplate.IsDropable ? "yes" : "no"));
			delve.Add("  Is pickable: " + (ItemTemplate.IsPickable ? "yes" : "no"));
			delve.Add(" Is stackable: " + (ItemTemplate.IsStackable ? "yes" : "no"));
			delve.Add(" Is tradeable: " + (ItemTemplate.IsTradable ? "yes" : "no"));

			foreach (var spell in Spells)
            {
				if (spell.IsPoison)
                {
					delve.Add("PoisonSpellID: " + spell.SpellID + " (" + spell.Charges + "/" + spell.MaxCharges + ") ");
				}
				else if (spell.ProcChance > 0)
                {
					delve.Add("  ProcSpellID: " + spell.SpellID + " (Chance " + spell.ProcChance + "%) ");
				}
				else
                {
					delve.Add("      SpellID: " + spell.SpellID + " (" + spell.Charges + "/" + spell.MaxCharges + ")");
				}
            }

			if (GlobalConstants.IsWeapon(ItemTemplate.ObjectType))
			{
				delve.Add("         Hand: " + GlobalConstants.ItemHandToName(ItemTemplate.Hand) + " (" + ItemTemplate.Hand + ")");
				delve.Add("Damage/Second: " + (ItemTemplate.DPS_AF / 10.0f));
				delve.Add("        Speed: " + (ItemTemplate.SPD_ABS / 10.0f));
				delve.Add("  Damage type: " + GlobalConstants.WeaponDamageTypeToName(ItemTemplate.TypeDamage) + " (" + ItemTemplate.TypeDamage + ")");
				delve.Add("        Bonus: " + ItemTemplate.ItemBonus);
			}
			else if (GlobalConstants.IsArmor(ItemTemplate.ObjectType))
			{
				delve.Add("  Armorfactor: " + ItemTemplate.DPS_AF);
				delve.Add("   Absorption: " + ItemTemplate.SPD_ABS);
				delve.Add("        Bonus: " + ItemTemplate.ItemBonus);
			}
			else if (ItemTemplate.ObjectType == (int)eObjectType.Shield)
			{
				delve.Add("Damage/Second: " + (ItemTemplate.DPS_AF / 10.0f));
				delve.Add("        Speed: " + (ItemTemplate.SPD_ABS / 10.0f));
				delve.Add("  Shield type: " + GlobalConstants.ShieldTypeToName(ItemTemplate.TypeDamage) + " (" + ItemTemplate.TypeDamage + ")");
				delve.Add("        Bonus: " + ItemTemplate.ItemBonus);
			}
			else if (ItemTemplate.ObjectType == (int)eObjectType.Arrow || ItemTemplate.ObjectType == (int)eObjectType.Bolt)
			{
				delve.Add(" Ammunition #: " + ItemTemplate.DPS_AF);
				delve.Add("       Damage: " + GlobalConstants.AmmunitionTypeToDamageName(ItemTemplate.SPD_ABS));
				delve.Add("        Range: " + GlobalConstants.AmmunitionTypeToRangeName(ItemTemplate.SPD_ABS));
				delve.Add("     Accuracy: " + GlobalConstants.AmmunitionTypeToAccuracyName(ItemTemplate.SPD_ABS));
				delve.Add("        Bonus: " + ItemTemplate.ItemBonus);
			}
			else if (ItemTemplate.ObjectType == (int)eObjectType.Instrument)
			{
				delve.Add("   Instrument: " + GlobalConstants.InstrumentTypeToName(ItemTemplate.DPS_AF));
			}
		}

		/// <summary>
		/// Returns the level when this artifact will gain a new
		/// ability.
		/// </summary>
		/// <returns></returns>
		private int GainsNewAbilityAtLevel()
		{
			for (eArtifactBonus bonusID = eArtifactBonus.Min; bonusID <= eArtifactBonus.Max; ++bonusID)
				if (m_levelRequirements[(int)bonusID] > ArtifactLevel)
					return m_levelRequirements[(int)bonusID];

			return 10;
		}

		/// <summary>
		/// Check whether this type is a cap increase bonus.
		/// </summary>
		/// <param name="bonusType"></param>
		/// <returns></returns>
		protected virtual bool IsCapIncreaseBonus(int bonusType)
		{
			if ((bonusType >= (int)eProperty.StatCapBonus_First) && (bonusType <= (int)eProperty.StatCapBonus_Last))
				return true;
			return false;
		}

		/// <summary>
		/// Check whether this type is focus.
		/// </summary>
		/// <param name="bonusType"></param>
		/// <returns></returns>
		protected virtual bool IsFocusBonus(int bonusType)
		{
			if ((bonusType >= (int)eProperty.Focus_Darkness) && (bonusType <= (int)eProperty.Focus_Arboreal))
				return true;
			if ((bonusType >= (int)eProperty.Focus_EtherealShriek) && (bonusType <= (int)eProperty.Focus_Witchcraft))
				return true;
			if (bonusType == (int)eProperty.AllFocusLevels)
				return true;
			return false;
		}

		/// <summary>
		/// Check whether this type is resist.
		/// </summary>
		/// <param name="bonusType"></param>
		/// <returns></returns>
		protected virtual bool IsResistBonus(int bonusType)
		{
			if ((bonusType >= (int)eProperty.Resist_First) && (bonusType <= (int)eProperty.Resist_Last))
				return true;
			return false;
		}

		/// <summary>
		/// Check whether this type is a skill.
		/// </summary>
		/// <param name="bonusType"></param>
		/// <returns></returns>
		protected virtual bool IsSkillBonus(int bonusType)
		{
			if ((bonusType >= (int)eProperty.Skill_First) && (bonusType <= (int)eProperty.Skill_Last))
				return true;
			return false;
		}

		/// <summary>
		/// Check whether this type is a stat bonus.
		/// </summary>
		/// <param name="bonusType"></param>
		/// <returns></returns>
		protected virtual bool IsStatBonus(int bonusType)
		{
			if ((bonusType >= (int)eProperty.Stat_First) && (bonusType <= (int)eProperty.Stat_Last))
				return true;
			if ((bonusType == (int)eProperty.MaxHealth) || (bonusType == (int)eProperty.MaxMana))
				return true;
			if ((bonusType == (int)eProperty.Fatigue) || (bonusType == (int)eProperty.PowerPool))
				return true;
			if (bonusType == (int)eProperty.Acuity)
				return true;
			if ((bonusType == (int)eProperty.AllMeleeWeaponSkills) || (bonusType == (int)eProperty.AllMagicSkills))
				return true;
			if ((bonusType == (int)eProperty.AllDualWieldingSkills) || (bonusType == (int)eProperty.AllArcherySkills))
				return true;
			return false;
		}

		/// <summary>
		/// Check whether this type is a ToA bonus (it's all mixed
		/// up in GlobalConstants.cs, but let's try anyway).
		/// </summary>
		/// <param name="bonusType"></param>
		/// <returns></returns>
		protected virtual bool IsToABonus(int bonusType)
		{
			if (IsCapIncreaseBonus(bonusType))
				return true;
			if ((bonusType >= (int)eProperty.ToABonus_First) && (bonusType <= (int)eProperty.ToABonus_Last) &&
			    (bonusType != (int)eProperty.PowerPool) && (bonusType != (int)eProperty.Fatigue))
				return true;
			if ((bonusType >= (int)eProperty.MaxSpeed) && (bonusType <= (int)eProperty.MeleeSpeed))
				return true;
			if ((bonusType >= (int)eProperty.CriticalMeleeHitChance) &&
			    (bonusType <= (int)eProperty.CriticalHealHitChance))
				return true;
			if ((bonusType >= (int)eProperty.EvadeChance) && (bonusType <= (int)eProperty.SpeedDecreaseDurationReduction))
				return true;
			if ((bonusType >= (int)eProperty.BountyPoints) && (bonusType <= (int)eProperty.ArcaneSyphon))
				return true;
			return false;
		}

		#endregion
	}
}
