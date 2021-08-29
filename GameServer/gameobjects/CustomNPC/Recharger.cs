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
using Atlas.DataLayer.Models;
using DOL.Language;
using DOL.GS.PacketHandler;

namespace DOL.GS
{
	[NPCGuildScript("Recharger")]
	public class Recharger : GameNPC
	{
		private const string RECHARGE_ITEM_WEAK = "recharged item";

		/// <summary>
		/// Can accept any item
		/// </summary>
		public override bool CanTradeAnyItem
		{
			get { return true; }
		}

		#region Examine/Interact Message

		public override IList GetExamineMessages(GamePlayer player)
		{
			IList list = new ArrayList();
            list.Add(LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.GetExamineMessages",
                 GetName(0, false), GetPronoun(0, true, player.Client.Account.Language), GetAggroLevelString(player, false)));
			return list;
		}

		public override bool Interact(GamePlayer player)
		{
			if (!base.Interact(player))
				return false;

			TurnTo(player.X, player.Y);
			SayTo(player, eChatLoc.CL_ChatWindow, LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.Interact"));
			return true;
		}

		#endregion Examine/Interact Message

		#region Receive item

		public override bool ReceiveItem(GameLiving source, InventoryItem item)
		{
			GamePlayer player = source as GamePlayer;
			if (player == null || item == null)
				return false;

			if (item.Count != 1)
			{
				player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.ReceiveItem.StackedObjects",
                    GetName(0, false)), eChatType.CT_System, eChatLoc.CL_SystemWindow);
				return false;
			}

			if((!item.Spells.Any()) ||
				(item.ItemTemplate.ObjectType == (int)eObjectType.Poison) ||
				(item.ItemTemplate.ObjectType == (int)eObjectType.Magical && (item.ItemTemplate.ItemType == 40 || item.ItemTemplate.ItemType == 41)))
			{
				SayTo(player, LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.ReceiveItem.CantThat"));
				return false;
			}
			if (!item.Spells.Any(x => x.MaxCharges > 0 && x.Charges < x.MaxCharges))
			{
				SayTo(player, LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.ReceiveItem.FullyCharged"));
				return false;
			}

			long NeededMoney=0;

			foreach (var spell in item.Spells.Where(x=>x.MaxCharges > 0 && x.Charges < x.MaxCharges))
            {
				player.TempProperties.setProperty(RECHARGE_ITEM_WEAK, new WeakRef(item));
				NeededMoney += (spell.MaxCharges - spell.Charges) * Money.GetMoney(0, 0, 10, 0, 0);
			}
			
			if(NeededMoney > 0)
			{
				player.Client.Out.SendCustomDialog(LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.ReceiveItem.Cost", Money.GetString(NeededMoney)), new CustomDialogResponse(RechargerDialogResponse));
				return true;
			}
			return false;
		}

		protected void RechargerDialogResponse(GamePlayer player, byte response)
		{
			WeakReference itemWeak =
				(WeakReference) player.TempProperties.getProperty<object>(
				RECHARGE_ITEM_WEAK,
				new WeakRef(null)
				);
			player.TempProperties.removeProperty(RECHARGE_ITEM_WEAK);

			InventoryItem item = (InventoryItem) itemWeak.Target;

			if (item == null || item.SlotPosition == (int) eInventorySlot.Ground
				|| item.CharacterID == null || item.CharacterID != player.InternalID)
			{
				player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.RechargerDialogResponse.InvalidItem"), eChatType.CT_System, eChatLoc.CL_SystemWindow);
				return;
			}

			if (response != 0x01)
			{
				player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.RechargerDialogResponse.Decline", item.Name), eChatType.CT_System, eChatLoc.CL_SystemWindow);
				return;
			}

			long cost = 0;
			foreach (var spell in item.Spells.Where(x => x.MaxCharges > 0 && x.Charges < x.MaxCharges))
			{
				cost += (spell.MaxCharges - spell.Charges) * Money.GetMoney(0, 0, 10, 0, 0);
			}
			
			if(!player.RemoveMoney(cost))
			{
				player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.RechargerDialogResponse.NotMoney"), eChatType.CT_System, eChatLoc.CL_SystemWindow);
				return;
			}
            InventoryLogging.LogInventoryAction(player, this, eInventoryActionType.Merchant, cost);

			player.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.RechargerDialogResponse.GiveMoney",
                                   GetName(0, false), Money.GetString((long)cost)), eChatType.CT_System, eChatLoc.CL_SystemWindow);

			foreach (var spell in item.Spells.Where(x => x.MaxCharges > 0 && x.Charges < x.MaxCharges))
			{
				spell.Charges = spell.MaxCharges;
			}

			player.Out.SendInventoryItemsUpdate(new InventoryItem[] {item});
			SayTo(player, LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Recharger.RechargerDialogResponse.FullyCharged"));
			return;
		}

		#endregion Receive item
	}
}