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
 * Credits go to:
 * - Echostorm's Mob Drop Loot System
 * - Roach's modifications to add loottemplate base mobdrops
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Atlas.DataLayer.Models;
using DOL.AI.Brain;
using DOL.GS.Utils;
using Microsoft.EntityFrameworkCore;

namespace DOL.GS
{
	/// <summary>
	/// TemplateLootGenerator
	/// This implementation uses LootTemplates to relate loots to a specific mob type.
	/// Used DB Tables:
	///				MobxLootTemplate  (Relation between Mob and loottemplate
	///				LootTemplate	(loottemplate containing possible loot items)
	/// </summary>
	public class LootGeneratorTemplate : LootGeneratorBase
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Map holding a list of ItemTemplateIDs for each TemplateName
		/// 1:n mapping between loottemplateName and loottemplate entries
		/// </summary>
		protected static List<LootTable> m_lootTables = null;
		/// <summary>
		/// Construct a new templategenerate and load its values from database.
		/// </summary>
		public LootGeneratorTemplate()
		{
			PreloadLootTemplates();
		}

		public static void ReloadLootTemplates()
		{
			m_lootTables = null;
			PreloadLootTemplates();
		}

		/// <summary>
		/// Loads the loottemplates
		/// </summary>
		/// <returns></returns>
		protected static bool PreloadLootTemplates()
		{
			if (m_lootTables == null)
			{
				m_lootTables = new List<LootTable>();
				Dictionary<string, ItemTemplate> itemtemplates = new Dictionary<string, ItemTemplate>();

				lock (m_lootTables)
				{
					try
					{
						// TemplateName (typically the mob name), ItemTemplateID, Chance
						m_lootTables = GameServer.Database.LootTables.Include(x => x.Items).ThenInclude(x => x.ItemTemplate).ToList();
					}
					catch (Exception e)
					{
						if (log.IsErrorEnabled)
						{
							log.Error("LootGeneratorTemplate: LootTemplates could not be loaded:", e);
						}
						return false;
					}
				}

				log.Info("LootTemplates pre-loaded.");
			}

			return true;
		}

		/// <summary>
		/// Reload the loot templates for this mob
		/// </summary>
		/// <param name="mob"></param>
		public override void Refresh(GameNPC mob)
		{
			if (mob == null || !mob.LootTableID.HasValue)
				return;

			RefreshLootTemplate(mob.LootTableID.Value);
		}

		protected void RefreshLootTemplate(int lootTableId)
		{
			LootTable lootTable = null;
			lock (m_lootTables)
			{
				lootTable = m_lootTables.FirstOrDefault(x => x.Id == lootTableId);
				if (lootTable != null)
				{
					m_lootTables.Remove(lootTable);
				}
			}

			lootTable = GameServer.Database.LootTables.Include(x => x.Items).ThenInclude(x => x.ItemTemplate).FirstOrDefault(x => x.Id == lootTableId);

			if (lootTable != null)
			{
				lock (m_lootTables)
				{
					m_lootTables.Add(lootTable);
				}
			}
		}

		public override LootList GenerateLoot(GameNPC mob, GameObject killer)
		{
			LootList loot = base.GenerateLoot(mob, killer);

			try
			{
				GamePlayer player = null;

				if (killer is GamePlayer)
				{
					player = killer as GamePlayer;
				}
				else if (killer is GameNPC && (killer as GameNPC).Brain is IControlledBrain)
				{
					player = ((killer as GameNPC).Brain as ControlledNpcBrain).GetPlayerOwner();
				}

				// allow the leader to decide the loot realm
				if (player != null && player.Group != null)
				{
					player = player.Group.Leader;
				}

				if (player != null)
				{
					LootTable lootTable = m_lootTables.FirstOrDefault(x => x.Id == mob.LootTableID);
					
					if (lootTable != null)					
					{
						var items = lootTable.Items.ToList();
						loot = GenerateLootFromMobXLootTemplates(lootTable, items, loot, player);

						if (items.Any())
						{
							foreach (var lootTemplate in items)
							{
								ItemTemplate drop = lootTemplate.ItemTemplate;

								if (drop != null && (drop.Realm == (int)player.Realm || drop.Realm == 0 || player.CanUseCrossRealmItems))
								{
									loot.AddRandom(lootTemplate.Chance, drop, 1);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				log.ErrorFormat("Error in LootGeneratorTemplate for mob {0}.  Exception: {1} {2}", mob.Name, ex.Message, ex.StackTrace);
			}

			return loot;
		}

		/// <summary>
		/// Add all loot templates specified in MobXLootTemplate for an entry in LootTemplates
		/// If the item has a 100% drop chance add it as a fixed drop to the loot list.
		/// </summary>
		/// <param name="mobXLootTemplate">Entry in MobXLootTemplate.</param>
		/// <param name="lootTemplates">List of all itemtemplates this mob can drop and the chance to drop</param>
		/// <param name="lootList">List to hold loot.</param>
		/// <param name="player">Player used to determine realm</param>
		/// <returns>lootList (for readability)</returns>
		private LootList GenerateLootFromMobXLootTemplates(LootTable mobXLootTemplates, List<LootTableItem> lootTemplates, LootList lootList, GamePlayer player)
		{
			if (mobXLootTemplates == null || lootTemplates == null || player == null)
				return lootList;

			// Using Name + Realm (if ALLOW_CROSS_REALM_ITEMS) for the key to try and prevent duplicate drops
			foreach (var lootTemplate in lootTemplates)
			{
				ItemTemplate drop = lootTemplate.ItemTemplate;

				if (drop != null && (drop.Realm == (int)player.Realm || drop.Realm == 0 || player.CanUseCrossRealmItems))
				{
					if (lootTemplate.Chance == 100)
					{
						// Added support for specifying drop count in LootTemplate rather than relying on MobXLootTemplate DropCount
						if (lootTemplate.Count > 0)
							lootList.AddFixed(drop, lootTemplate.Count);
						else
							lootList.AddFixed(drop, mobXLootTemplates.DropCount);
					}
					else
					{
						lootTemplates.Add(lootTemplate);
						lootList.DropCount = Math.Max(lootList.DropCount, mobXLootTemplates.DropCount);
					}
				}
			}

			return lootList;
		}

	}
}
