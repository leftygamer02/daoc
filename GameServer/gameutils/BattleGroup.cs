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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DOL.Database;
using DOL.GS.Housing;
using DOL.GS.PacketHandler;
using DOL.Language;

namespace DOL.GS
{
	/// <summary>
	/// Battlegroups
	/// </summary>
	public class BattleGroup
	{
		public const string BATTLEGROUP_PROPERTY="battlegroup";
		/// <summary>
		/// This holds all players inside the battlegroup
		/// </summary>
		protected HybridDictionary m_battlegroupMembers = new HybridDictionary();
        protected GameLiving m_battlegroupLeader;
        protected List<GamePlayer> m_battlegroupModerators = new List<GamePlayer>();

        bool battlegroupLootType = false;
        public BGVault bgVault = null;
        GamePlayer battlegroupTreasurer = null;
        int battlegroupLootTypeThreshold = 0;

		/// <summary>
		/// constructor of battlegroup
		/// </summary>
		public BattleGroup()
		{
            battlegroupLootType = false;
            battlegroupTreasurer = null;
            bgVault = null;
            m_battlegroupLeader = null;
		}

        public GameLiving Leader
        {
            get { return m_battlegroupLeader; }
        }

		public HybridDictionary Members
		{
			get{return m_battlegroupMembers;}
			set{m_battlegroupMembers=value;}
		}
		
		public List<GamePlayer> Moderators
		{
			get{return m_battlegroupModerators;}
			set{m_battlegroupModerators=value;}
		}

		private bool listen=false;
		public bool Listen
		{
			get{return listen;}
			set{listen = value;}
		}

		private bool ispublic=true;
		public bool IsPublic
		{
			get{return ispublic;}
			set{ispublic = value;}
		}

		private string password="";
		public string Password
		{
			get{return password;}
			set{password = value;}
		}

		/// <summary>
		/// Adds a player to the chatgroup
		/// </summary>
		/// <param name="player">GamePlayer to be added to the group</param>
		/// <param name="leader"></param>
		/// <returns>true if added successfully</returns>
		public virtual bool AddBattlePlayer(GamePlayer player,bool leader) 
		{
			if (player == null) return false;
			lock (m_battlegroupMembers)
			{
				if (m_battlegroupMembers.Contains(player))
					return false;
				player.TempProperties.setProperty(BATTLEGROUP_PROPERTY, this);
                player.Out.SendMessage("You join the battle group.", eChatType.CT_System, eChatLoc.CL_SystemWindow);
				foreach(GamePlayer member in Members.Keys)
				{
                    member.Out.SendMessage(player.Name + " has joined the battle group.", eChatType.CT_System, eChatLoc.CL_SystemWindow);
				}
				m_battlegroupMembers.Add(player,leader);

                player.isInBG = true; //Xarik: Player is in BG
			}
			return true;
		}

        public virtual bool IsInTheBattleGroup(GamePlayer player)
        {
            lock (m_battlegroupMembers) // Mannen 10:56 PM 10/30/2006 - Fixing every lock(this)
            {
                return m_battlegroupMembers.Contains(player);
            }
        }

        public bool GetBGLootType()
        {
            return battlegroupLootType;
        }

        public GamePlayer GetBGTreasurer()
        {
            return battlegroupTreasurer;
        }

        public GameLiving GetBGLeader()
        {
            return m_battlegroupLeader;
        }

        public bool SetBGLeader(GameLiving living)
        {
            if (living != null)
            {
                m_battlegroupLeader = living;
                return true;
            }

            return false;
        }
        public bool IsBGTreasurer(GameLiving living)
        {
            if (battlegroupTreasurer != null && living != null)
            {
                return battlegroupTreasurer == living;
            }

            return false;
        }
        public bool IsBGLeader(GameLiving living)
        {
            if (m_battlegroupLeader != null && living != null)
            {
                return m_battlegroupLeader == living;
            }

            return false;
        }
        
        public bool IsBGModerator(GamePlayer living)
        {
	        if (m_battlegroupModerators != null && living != null)
	        {
		        var ismod = m_battlegroupModerators.Contains(living);
		        return ismod;
	        }

	        return false;
        }

        public int GetBGLootTypeThreshold()
        {
            return battlegroupLootTypeThreshold;
        }

        public bool SetBGLootTypeThreshold(int thresh)
        {
            battlegroupLootTypeThreshold = thresh;

            if (thresh < 0 || thresh > 50)
            {
                battlegroupLootTypeThreshold = 0;
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SetBGLootType(bool type)
        {
            battlegroupLootType = type;
            if (battlegroupLootType == false)
            {
                // Within bounds, continue
            }
            if (battlegroupLootType == true)
            {
                ItemTemplate vaultItem = GetDummyVaultItem((GamePlayer)Leader);
                bgVault = new BGVault((GamePlayer)Leader, $"{Leader.Name}_{Leader.Realm}_BGVault",0, vaultItem);
                // Within bounds, continue
            }
            else
            {
                // Data has entered here that should not be, set to normal = 0
                battlegroupLootType = false;
            }
        }
        
        private static ItemTemplate GetDummyVaultItem(GamePlayer player)
        {
            ItemTemplate vaultItem = new ItemTemplate();
            vaultItem.Object_Type = (int)eObjectType.HouseVault;
            vaultItem.Name = "Vault";
            vaultItem.ObjectId = player.Client.Account.Name + "_" + player.Realm.ToString();
            switch (player.Realm)
            {
                case eRealm.Albion:
                    vaultItem.Id_nb = "housing_alb_vault";
                    vaultItem.Model = 1489;
                    break;
                case eRealm.Hibernia:
                    vaultItem.Id_nb = "housing_hib_vault";
                    vaultItem.Model = 1491;
                    break;
                case eRealm.Midgard:
                    vaultItem.Id_nb = "housing_mid_vault";
                    vaultItem.Model = 1493;
                    break;
            }

            return vaultItem;
        }

        public bool SetBGTreasurer(GamePlayer treasurer)
        {
            battlegroupTreasurer = treasurer;
            if (battlegroupTreasurer == null)
            {
                // Do not set treasurer
                return false;
            }

            if (battlegroupTreasurer != null)
            {
                // Good input, got a treasurer, continue
                return true;
            }
            else
            {
                // Bad input, fix with null
                battlegroupTreasurer = null;
                return false;
            }
        }

        public virtual void SendMessageToBattleGroupMembers(string msg, eChatType type, eChatLoc loc)
        {
            lock (m_battlegroupMembers) // Mannen 10:56 PM 10/30/2006 - Fixing every lock(this)
            {
                foreach (GamePlayer player in m_battlegroupMembers.Keys)
                {
                    player.Out.SendMessage(msg, type, loc);
                }
            }
        }

        public int PlayerCount
        {
            get { return m_battlegroupMembers.Count; }
        }
        /// <summary>
		/// Removes a player from the group
		/// </summary>
		/// <param name="player">GamePlayer to be removed</param>
		/// <returns>true if removed, false if not</returns>
		public virtual bool RemoveBattlePlayer(GamePlayer player)
		{
			if (player == null) return false;
			lock (m_battlegroupMembers)
			{
				if (!m_battlegroupMembers.Contains(player))
					return false;
				var leader = IsBGLeader(player);
				m_battlegroupMembers.Remove(player);
				player.TempProperties.removeProperty(BATTLEGROUP_PROPERTY);
				player.isInBG = false; //Xarik: Player is no more in the BG
                player.Out.SendMessage("You leave the battle group.", eChatType.CT_System, eChatLoc.CL_SystemWindow);
				foreach(GamePlayer member in Members.Keys)
				{
                    member.Out.SendMessage(player.Name + " has left the battle group.", eChatType.CT_System, eChatLoc.CL_SystemWindow);
				}
				if (m_battlegroupMembers.Count == 1)
				{
					ArrayList lastPlayers = new ArrayList(m_battlegroupMembers.Count);
					lastPlayers.AddRange(m_battlegroupMembers.Keys);
					foreach (GamePlayer plr in lastPlayers)
					{
						RemoveBattlePlayer(plr);
					}
				} else if (leader && m_battlegroupMembers.Count >= 2)
				{
					var bgPlayers = new ArrayList(m_battlegroupMembers.Count);
					bgPlayers.AddRange(m_battlegroupMembers.Keys);
					var randomPlayer = bgPlayers[Util.Random(bgPlayers.Count) - 1] as GamePlayer;
					if (randomPlayer == null) return false;
					SetBGLeader(randomPlayer);
					m_battlegroupMembers[randomPlayer] = true;
					foreach(GamePlayer member in Members.Keys)
					{
						member.Out.SendMessage(randomPlayer.Name + " is the new leader of the battle group.", eChatType.CT_BattleGroupLeader, eChatLoc.CL_SystemWindow);
					}
				}

			}
			return true;
		}
	}
	    public class BGVault : GameHouseVault
    {
        public const int SIZE = 100;
        public const int Last_Used_FIRST_SLOT = 1600;
        public const int FIRST_SLOT = 2500;
        private GamePlayer m_player;
        private GameNPC m_vaultNPC;
        private string m_vaultOwner;
        private int m_vaultNumber = 0;

        private readonly object _vaultLock = new object();

        /// <summary>
        /// An account vault that masquerades as a house vault to the game client
        /// </summary>
        /// <param name="player">Player who owns the vault</param>
        /// <param name="vaultNPC">NPC controlling the interaction between player and vault</param>
        /// <param name="vaultOwner">ID of vault owner (can be anything unique, if it's the account name then all toons on account can access the items)</param>
        /// <param name="vaultNumber">Valid vault IDs are 0-3</param>
        /// <param name="dummyTemplate">An ItemTemplate to satisfy the base class's constructor</param>
        public BGVault(GamePlayer player, string vaultOwner, int vaultNumber, ItemTemplate dummyTemplate)
            : base(dummyTemplate, vaultNumber)
        {
            m_player = player;
            m_vaultOwner = vaultOwner;
            m_vaultNumber = vaultNumber;

            DBHouse dbh = new DBHouse();
            dbh.AllowAdd = false;
            dbh.GuildHouse = false;
            dbh.HouseNumber = player.ObjectID;
            dbh.Name = "Account Vault";
            //dbh.Name = "Maison de " + player.Name;
            dbh.OwnerID = player.Client.Account.Name + "_" + player.Realm.ToString();
            dbh.RegionID = player.CurrentRegionID;
            CurrentHouse = new House(dbh);
        }

        public override bool Interact(GamePlayer player)
        {
            if (!CanView(player))
            {
                player.Out.SendMessage("You don't have permission to view this vault!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                return false;
            }

            if (player.ActiveInventoryObject != null)
            {
                player.ActiveInventoryObject.RemoveObserver(player);
            }

            lock (_vaultLock)
            {
                if (!_observers.ContainsKey(player.Name))
                {
                    _observers.Add(player.Name, player);
                }
            }

            player.ActiveInventoryObject = this;
            player.Out.SendInventoryItemsUpdate(GetClientInventory(player), eInventoryWindowType.HouseVault);
            return true;
        }

        public override Dictionary<int, InventoryItem> GetClientInventory(GamePlayer player)
        {

            var items = new Dictionary<int, InventoryItem>();
            int slotOffset = -FirstDBSlot + (int)(eInventorySlot.HousingInventory_First);
            foreach (InventoryItem item in DBItems(player))
            {
                if (item != null)
                {
                    if (!items.ContainsKey(item.SlotPosition + slotOffset))
                    {
                        items.Add(item.SlotPosition + slotOffset, item);
                    }
                    else
                    {
                       //log.ErrorFormat("GAMEACCOUNTVAULT: Duplicate item {0}, owner {1}, position {2}", item.Name, item.OwnerID, (item.SlotPosition + slotOffset));
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Is this a move request for a housing vault?
        /// </summary>
        /// <param name="player"></param>
        /// <param name="fromSlot"></param>
        /// <param name="toSlot"></param>
        /// <returns></returns>
        public override bool CanHandleMove(GamePlayer player, ushort fromSlot, ushort toSlot)
        {
            if (player == null || player.ActiveInventoryObject != this)
                return false;

            bool canHandle = false;

            // House Vaults and GameConsignmentMerchant Merchants deliver the same slot numbers
            if (fromSlot >= (ushort)eInventorySlot.HousingInventory_First &&
                fromSlot <= (ushort)eInventorySlot.HousingInventory_Last)
            {
                canHandle = true;
            }
            else if (toSlot >= (ushort)eInventorySlot.HousingInventory_First &&
                toSlot <= (ushort)eInventorySlot.HousingInventory_Last)
            {
                canHandle = true;
            }

            return canHandle;
        }

        /// <summary>
        /// Move an item from, to or inside a house vault.  From IGameInventoryObject
        /// </summary>
        public override bool MoveItem(GamePlayer player, ushort fromSlot, ushort toSlot)
        {
            if (GetOwner(player) != m_vaultOwner)
                return false;
            if (fromSlot == toSlot)
            {
                return false;
            }

            bool fromAccountVault = (fromSlot >= (ushort)eInventorySlot.HousingInventory_First && fromSlot <= (ushort)eInventorySlot.HousingInventory_Last);
            bool toAccountVault = (toSlot >= (ushort)eInventorySlot.HousingInventory_First && toSlot <= (ushort)eInventorySlot.HousingInventory_Last);

            if (fromAccountVault == false && toAccountVault == false)
            {
                return false;
            }

            //Prevent exploit shift+clicking quiver exploit
            if (fromAccountVault)
            {
                if (fromSlot < (ushort)eInventorySlot.HousingInventory_First || fromSlot > (ushort) eInventorySlot.HousingInventory_Last) return false;
            }

            GameVault gameVault = player.ActiveInventoryObject as GameVault;
            if (gameVault == null)
            {
                player.Out.SendMessage("You are not actively viewing a vault!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                player.Out.SendInventoryItemsUpdate(null);
                return false;
            }

            if (toAccountVault)
            {
                InventoryItem item = player.Inventory.GetItem((eInventorySlot)toSlot);
                if (item != null)
                {
                    if (gameVault.CanRemoveItems(player) == false)
                    {
                        player.Out.SendMessage("You don't have permission to remove items!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                        return false;
                    }
                }
                if (gameVault.CanAddItems(player) == false)
                {
                    player.Out.SendMessage("You don't have permission to add items!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                    return false;
                }
            }

            if (fromAccountVault && gameVault.CanRemoveItems(player) == false)
            {
                player.Out.SendMessage("You don't have permission to remove items!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                return false;
            }

            InventoryItem itemInFromSlot = player.Inventory.GetItem((eInventorySlot)fromSlot);
            InventoryItem itemInToSlot = player.Inventory.GetItem((eInventorySlot)toSlot);

            // Check for a swap to get around not allowing non-tradables in a housing vault - Tolakram
            if (fromAccountVault && itemInToSlot != null && itemInToSlot.IsTradable == false)
            {
                player.Out.SendMessage("You cannot swap with an untradable item!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                //log.DebugFormat("GameVault: {0} attempted to swap untradable item {2} with {1}", player.Name, itemInFromSlot.Name, itemInToSlot.Name);
                player.Out.SendInventoryItemsUpdate(null);
                return false;
            }

            // Allow people to get untradables out of their house vaults (old bug) but 
            // block placing untradables into housing vaults from any source - Tolakram
            if (toAccountVault && itemInFromSlot != null && itemInFromSlot.IsTradable == false)
            {
                if (itemInFromSlot.Id_nb != "token_many")
                {
                    player.Out.SendMessage("You can not put this item into an Account Vault!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                    player.Out.SendInventoryItemsUpdate(null);
                    return false;
                }
            }

            // let's move it

            lock (m_vaultSync)
            {
                if (fromAccountVault)
                {
                    if (toAccountVault)
                    {
                        NotifyObservers(player, this.MoveItemInsideObject(player, (eInventorySlot)fromSlot, (eInventorySlot)toSlot));
                    }
                    else
                    {
                        NotifyObservers(player, this.MoveItemFromObject(player, (eInventorySlot)fromSlot, (eInventorySlot)toSlot));
                    }
                }
                else if (toAccountVault)
                {
                    NotifyObservers(player, this.MoveItemToObject(player, (eInventorySlot)fromSlot, (eInventorySlot)toSlot));
                }
            }

            return true;
        }
        /// <summary>
        /// Whether or not this player can view the contents of this vault.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool CanView(GamePlayer player)
        {
            if (GetOwner(player) == m_vaultOwner)
                return true;

            return false;
        }

        /// <summary>
        /// Whether or not this player can move items inside the vault
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool CanAddItems(GamePlayer player)
        {
            if (GetOwner(player) == m_vaultOwner)
                return true;

            return false;
        }

        /// <summary>
        /// Whether or not this player can move items inside the vault
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool CanRemoveItems(GamePlayer player)
        {
            if (GetOwner(player) == m_vaultOwner)
                return true;

            return false;
        }


        public override string GetOwner(GamePlayer player)
        {
            return player.Client.Account.Name + "_" + player.Realm.ToString();
        }

        /// <summary>
        /// List of items in the vault.
        /// </summary>
        private new IList<InventoryItem> DBItems(GamePlayer player = null)
        {
            string sqlQuery = String.Format("OwnerID = '{0}' and SlotPosition >= {1} and SlotPosition <= {2}", GetOwner(player), FirstDBSlot, LastDBSlot);
            
            return GameServer.Database.SelectObjects<InventoryItem>(sqlQuery);
        }

        public override int FirstDBSlot
        {
            get
            {
                return 2700;
            }
        }

        public override int LastDBSlot
        {
            get 
            { 
                return 2799;
            }
        }
    }
    public class BGTreasurer : GameNPC
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        public override bool Interact(GamePlayer player)
        {
            if (!base.Interact(player))
                return false;
            
            var mybattlegroup = player.TempProperties.getProperty<BattleGroup>(BattleGroup.BATTLEGROUP_PROPERTY, null);
            if (mybattlegroup == null)
            {
                player.Client.Out.SendMessage(LanguageMgr.GetTranslation(player.Client.Account.Language, "Scripts.Players.Battlegroup.InBattleGroup"), eChatType.CT_System, eChatLoc.CL_SystemWindow);
                return false;
            }

            if (mybattlegroup.bgVault == null)
            {
                player.Client.Out.SendMessage("This BG has no vault yet", eChatType.CT_System, eChatLoc.CL_SystemWindow);
                return false;
            }

            string message = $"Greetings {player.Name}, nice meeting you.\n";
            
            message += $"I am happy to show you the current loot for {mybattlegroup.Leader.Name} BG.\n\n";

            player.Out.SendMessage(message, eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            
            player.ActiveInventoryObject = mybattlegroup.bgVault;
            player.Out.SendInventoryItemsUpdate(mybattlegroup.bgVault.GetClientInventory(player), eInventoryWindowType.HouseVault);
            return true;
        }

        public override bool WhisperReceive(GameLiving source, string text)
        {
            return false;
        }
    }
}
