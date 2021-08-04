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
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Linq;
using Atlas.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DOL.GS
{
	/// <summary>
	/// Alliance are the alliance between guild in game
	/// </summary>
	public class Alliance
	{
		protected ArrayList m_guilds;
		protected GuildAlliance m_dballiance;
		public Alliance()
		{
			m_dballiance = null;
			m_guilds = new ArrayList(2);
		}
		public ArrayList Guilds
		{
			get
			{
				return m_guilds;
			}
			set
			{
				m_guilds = value;
			}
		}
		public GuildAlliance Dballiance
		{
			get
			{
				return m_dballiance;
			}
			set
			{
				m_dballiance = value;
			}
		}

		#region IList
		public void AddGuild(Guild myguild)
		{
			lock (Guilds.SyncRoot)
			{
				myguild.alliance = this;
				Guilds.Add(myguild);
				myguild.AllianceId = m_dballiance.Id;
				m_dballiance.Guilds = null;
				//sirru 23.12.06 Add the new object instead of trying to save it
				GameServer.Instance.SaveDataObject(m_dballiance);

				m_dballiance = GameServer.Database.GuildAlliances.Include(x => x.Guilds).FirstOrDefault(x => x.Id == m_dballiance.Id);

				//sirru 23.12.06 save changes to db for each guild
				SaveIntoDatabase();
				SendMessageToAllianceMembers(myguild.Name + " has joined the alliance of " + m_dballiance.AllianceName, PacketHandler.eChatType.CT_System, PacketHandler.eChatLoc.CL_SystemWindow);
			}
		}
		public void RemoveGuild(Guild myguild)
		{
			lock (Guilds.SyncRoot)
			{
				myguild.alliance = null;
				myguild.AllianceId = 0;
                Guilds.Remove(myguild);
                if (myguild.GuildID == m_dballiance.LeaderGuildID)
                {
                    SendMessageToAllianceMembers(myguild.Name + " has disbanded the alliance of " + m_dballiance.AllianceName, PacketHandler.eChatType.CT_System, PacketHandler.eChatLoc.CL_SystemWindow);
                    ArrayList mgl = new ArrayList(Guilds);
                    foreach (Guild mg in mgl)
                    {
                        try
                        {
                            RemoveGuild(mg);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    GameServer.Instance.DeleteDataObject(m_dballiance);
                }
                else
                {
                    m_dballiance.Guilds = null;
                    GameServer.Instance.SaveDataObject(m_dballiance);

					m_dballiance = GameServer.Database.GuildAlliances.Include(x => x.Guilds).FirstOrDefault(x => x.Id == m_dballiance.Id);
				}
				//sirru 23.12.06 save changes to db for each guild
				myguild.SaveIntoDatabase();
                myguild.SendMessageToGuildMembers(myguild.Name + " has left the alliance of " + m_dballiance.AllianceName, PacketHandler.eChatType.CT_System, PacketHandler.eChatLoc.CL_SystemWindow);
                SendMessageToAllianceMembers(myguild.Name + " has left the alliance of " + m_dballiance.AllianceName, PacketHandler.eChatType.CT_System, PacketHandler.eChatLoc.CL_SystemWindow);
			}
		}
		public void Clear()
		{
			lock (Guilds.SyncRoot)
			{
				foreach (Guild guild in Guilds)
				{
					guild.alliance = null;
					guild.AllianceId = 0;
					//sirru 23.12.06 save changes to db
					guild.SaveIntoDatabase();
				}
				Guilds.Clear();
			}
		}
		public bool Contains(Guild myguild)
		{
			lock (Guilds.SyncRoot)
			{
				return Guilds.Contains(myguild);
			}
		}

		#endregion

		/// <summary>
		/// send message to all member of alliance
		/// </summary>
		public void SendMessageToAllianceMembers(string msg, PacketHandler.eChatType type, PacketHandler.eChatLoc loc)
		{
			lock (Guilds.SyncRoot)
			{
				foreach (Guild guild in Guilds)
				{
					guild.SendMessageToGuildMembers(msg, type, loc);
				}
			}
		}

		/// <summary>
		/// Loads this alliance from an alliance table
		/// </summary>
		/// <param name="obj"></param>
		public void LoadFromDatabase(DataObjectBase obj)
		{
			if (!(obj is GuildAlliance))
				return;

			m_dballiance = (GuildAlliance)obj;
		}

		/// <summary>
		/// Saves this alliance to database
		/// </summary>
		public void SaveIntoDatabase()
		{
			GameServer.Instance.SaveDataObject(m_dballiance);
			lock (Guilds.SyncRoot)
			{
				foreach (Guild guild in Guilds)
				{
					guild.SaveIntoDatabase();
				}
			}
		}
	}
}
