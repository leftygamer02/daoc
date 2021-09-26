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
using System.IO;
using Atlas.DataLayer.Models;
using DOL.Events;
using DOL.GS.PacketHandler;
using log4net;

namespace DOL.GS.Scripts
{
	/// <summary>
	/// Generates an XML version of the web ui
	/// </summary>
	public class XMLWebUIGenerator
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		[ScriptLoadedEvent]
		public static void OnScriptLoaded(DOLEvent e, object sender, EventArgs args)
		{
			//Uncomment the following line to enable the WebUI
			//Start();
		}

		[ScriptUnloadedEvent]
		public static void OnScriptUnloaded(DOLEvent e, object sender, EventArgs args)
		{
			//Uncomment the following line to enable the WebUI
			//Stop();
		}


		private static System.Timers.Timer m_timer = null;

		/// <summary>
		/// Reads in the template and generates the appropriate html
		/// </summary>
		public static void Generate()
		{
			try
			{				
				ServerInfo si = new ServerInfo();

				si.Time = DateTime.Now.ToString();
				si.ServerName = GameServer.Instance.Configuration.ServerName;
				si.NumClients = GameServer.Instance.ClientCount;
				si.NumAccounts = GameServer.Database.Accounts.Count();
				si.NumMobs = GameServer.Database.SpawnPoints.Count();
				si.NumInventoryItems = GameServer.Database.InventoryItems.Count();
				si.NumPlayerChars = GameServer.Database.Characters.Count();
				si.NumMerchantItems = GameServer.Database.MerchantItems.Count();
				si.NumItemTemplates = GameServer.Database.ItemTemplates.Count();
				si.NumWorldObjects = GameServer.Database.WorldObjects.Count();
				si.ServerType = GameServer.Instance.Configuration.ServerType.ToString();
				si.ServerStatus = GameServer.Instance.ServerStatus.ToString();
				si.AAC = GameServer.Instance.Configuration.AutoAccountCreation ? "enabled" : "disabled";

				GameServer.Instance.SaveDataObject(si);

				PlayerInfo pi = new PlayerInfo();

				foreach (GameClient client in WorldMgr.GetAllPlayingClients())
				{
					GamePlayer plr = client.Player;

					pi.Name = plr.Name;
					pi.LastName = plr.LastName;
					pi.Class = plr.CharacterClass.Name;
					pi.Race = plr.RaceName;
					pi.Guild = plr.GuildName;
					pi.Level = plr.Level;
					pi.Alive = plr.IsAlive ? "yes" : "no";
					pi.Realm = ((eRealm) plr.Realm).ToString();
					pi.Region = plr.CurrentRegion.Name;
					pi.X = plr.X;
					pi.Y = plr.Y;
				}

				if (log.IsInfoEnabled)
					log.Info("WebUI Generation initialized");
			}
			catch (Exception e)
			{
				if (log.IsErrorEnabled)
					log.Error("WebUI Generation: ", e);
			}
		}

		/// <summary>
		/// Starts the timer to generate the web ui
		/// </summary>
		public static void Start()
		{
			if (m_timer != null)
			{
				Stop();
			}

			m_timer = new System.Timers.Timer(60000.0); //1 minute
			m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
			m_timer.AutoReset = true;
			m_timer.Start();

			if (log.IsInfoEnabled)
				log.Info("Web UI generation started...");
		}

		/// <summary>
		/// Stops the timer that generates the web ui
		/// </summary>
		public static void Stop()
		{
			if (m_timer != null)
			{
				m_timer.Stop();
				m_timer.Close();
				m_timer.Elapsed -= new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
				m_timer = null;
			}

			Generate();

			if (log.IsInfoEnabled)
				log.Info("Web UI generation stopped...");
		}

		/// <summary>
		/// The timer proc that generates the web ui every X milliseconds
		/// </summary>
		/// <param name="sender">Caller of this function</param>
		/// <param name="e">Info about the timer</param>
		private static void m_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Generate();
		}
	}
}