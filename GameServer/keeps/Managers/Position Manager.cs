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

using Atlas.DataLayer;
using Atlas.DataLayer.Models;
using DOL.GS;
using DOL.GS.Movement;
using DOL.GS.PacketHandler;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DOL.GS.Keeps
{
	/// <summary>
	/// Class to manage the guards Positions
	/// </summary>
	public class PositionMgr
	{
		/// <summary>
		/// Gets the most usable position directly from the database
		/// </summary>
		/// <param name="guard">The guard object</param>
		/// <returns>The position object</returns>
		public static KeepPosition GetUsablePosition(GameKeepGuard guard)
		{
			return GameServer.Database.KeepPositions.Where(x => x.ClassType != "DOL.GS.Keeps.Banner" &&
																x.TemplateID == guard.TemplateID &&
																x.ComponentSkin == guard.Component.Skin &&
																x.Height <= guard.Component.Height)
													.OrderByDescending(x=> x.Height)
													.FirstOrDefault();
		}

		/// <summary>
		/// Gets the most usuable position for a banner directly from the database
		/// </summary>
		/// <param name="b">The banner object</param>
		/// <returns>The position object</returns>
		public static KeepPosition GetUsablePosition(GameKeepBanner b)
		{
			return GameServer.Database.KeepPositions.Where(x => x.ClassType != "DOL.GS.Keeps.Banner" &&
																x.TemplateID == b.TemplateID &&
																x.ComponentSkin == b.Component.Skin &&
																x.Height <= b.Component.Height)
													.OrderByDescending(x => x.Height)
													.FirstOrDefault();
		}

		/// <summary>
		/// Gets the position at the exact entry from the database
		/// </summary>
		/// <param name="guard">The guard object</param>
		/// <returns>The position object</returns>
		public static KeepPosition GetPosition(GameKeepGuard guard)
		{
			return GameServer.Database.KeepPositions.FirstOrDefault(x => x.TemplateID == guard.TemplateID &&
																		 x.ComponentSkin == guard.Component.Skin &&
																		 x.Height <= guard.Component.Height);
		}


		public static void LoadGuardPosition(KeepPosition pos, GameKeepGuard guard)
		{
			LoadKeepItemPosition(pos, guard);

            guard.SpawnPoint.X = guard.X;
            guard.SpawnPoint.Y = guard.Y;
            guard.SpawnPoint.Z = guard.Z;
			guard.SpawnHeading = guard.Heading;
		}

		public static void LoadKeepItemPosition(KeepPosition pos, IKeepItem item)
		{
			item.CurrentRegionID = item.Component.CurrentRegionID;
			int x, y;
			LoadXY(item.Component, pos.XOff, pos.YOff, out x, out y);
			item.X = x;
			item.Y = y;

			item.Z = item.Component.Keep.Z + pos.ZOff;

			item.Heading = (ushort)(item.Component.Heading + pos.HOff);

			item.Position = pos;
		}

		/// <summary>
		/// Calculates X and Y based on component rotation and offset
		/// </summary>
		/// <param name="component">The assigned component object</param>
		/// <param name="inX">The argument X</param>
		/// <param name="inY">The argument Y</param>
		/// <param name="outX">The result X</param>
		/// <param name="outY">The result Y</param>
		public static void LoadXY(GameKeepComponent component, int inX, int inY, out int outX, out int outY)
		{
			double angle = component.Keep.Heading * ((Math.PI * 2) / 360); // angle*2pi/360;
			double C = Math.Cos(angle);
			double S = Math.Sin(angle);
			switch (component.ComponentHeading)
			{
				case 0:
					{
						outX = (int)(component.X + C * inX + S * inY);
						outY = (int)(component.Y - C * inY + S * inX);
						break;
					}
				case 1:
					{
						outX = (int)(component.X + C * inY - S * inX);
						outY = (int)(component.Y + C * inX + S * inY);
						break;
					}
				case 2:
					{
						outX = (int)(component.X - C * inX - S * inY);
						outY = (int)(component.Y + C * inY - S * inX);
						break;
					}
				case 3:
					{
						outX = (int)(component.X - C * inY + S * inX);
						outY = (int)(component.Y - C * inX - S * inY);
						break;
					}
				default:
					{
						outX = 0;
						outY = 0;
						break;
					}
			}
		}

		/// <summary>
		/// Saves X and Y offsets
		/// </summary>
		/// <param name="component">The assigned component object</param>
		/// <param name="inX">The argument X</param>
		/// <param name="inY">The argument Y</param>
		/// <param name="outX">The result X</param>
		/// <param name="outY">The result Y</param>
		public static void SaveXY(GameKeepComponent component, int inX, int inY, out int outX, out int outY)
		{
			double angle = component.Keep.Heading * ((Math.PI * 2) / 360); // angle*2pi/360;
			int gx = inX - component.X;
			int gy = inY - component.Y;
			double C = Math.Cos(angle);
			double S = Math.Sin(angle);
			switch (component.ComponentHeading)
			{
				case 0:
					{
						outX = (int)(gx * C + gy * S);
						outY = (int)(gx * S - gy * C);
						break;
					}
				case 1:
					{
						outX = (int)(gy * C - gx * S);
						outY = (int)(gx * C + gy * S);
						break;
					}
				case 2:
					{
						outX = (int)((gx * C + gy * S) / (-C * C - S * S));
						outY = (int)(gy * C - gx * S);
						break;
					}
				case 3:
					{
						outX = (int)(gx * S - gy * C);
						outY = (int)((gx * C + gy * S) / (-C * C - S * S));
						break;
					}
				default:
					{
						outX = 0;
						outY = 0;
						break;
					}
			}
		}

		/// <summary>
        /// Creates a position
		/// </summary>
		/// <param name="type"></param>
		/// <param name="height"></param>
		/// <param name="player"></param>
		/// <param name="guardID"></param>
		/// <param name="component"></param>
		/// <returns></returns>
		public static KeepPosition CreatePosition(Type type, int height, GamePlayer player, string guardID, GameKeepComponent component)
		{
			KeepPosition pos = CreatePosition(guardID, component, player);
			pos.Height = height;
			pos.ClassType = type.ToString();
			GameServer.Instance.SaveDataObject(pos);
			return pos;
		}

		/// <summary>
		/// Creates a guard patrol position
		/// </summary>
		/// <param name="guardID">The guard ID</param>
		/// <param name="component">The component object</param>
		/// <param name="player">The player object</param>
		/// <returns>The position object</returns>
		public static KeepPosition CreatePatrolPosition(string guardID, GameKeepComponent component, GamePlayer player, AbstractGameKeep.eKeepType keepType)
		{
			KeepPosition pos = CreatePosition(guardID, component, player);
			pos.Height = 0;
			pos.ClassType = "DOL.GS.Keeps.Patrol";
			pos.KeepType = (int)keepType;
			GameServer.Instance.SaveDataObject(pos);
			return pos;
		}

		/// <summary>
		/// Creates a position
		/// </summary>
		/// <param name="templateID">The template ID</param>
		/// <param name="component">The component object</param>
		/// <param name="player">The creating player object</param>
		/// <returns>The position object</returns>
		public static KeepPosition CreatePosition(string templateID, GameKeepComponent component, GamePlayer player)
		{
			KeepPosition pos = new KeepPosition();
			pos.ComponentSkin = component.Skin;
			pos.ComponentRotation = component.ComponentHeading;
			pos.TemplateID = templateID;
			int x, y;

			SaveXY(component, player.X, player.Y, out x, out y);
			pos.XOff = x;
			pos.YOff = y;

			pos.ZOff = player.Z - component.Z;

			pos.HOff = player.Heading - component.Heading;
			return pos;
		}

		public static void AddPosition(KeepPosition position)
		{
			foreach (AbstractGameKeep keep in GameServer.KeepManager.GetAllKeeps())
			{
				foreach (GameKeepComponent component in keep.KeepComponents)
				{
					KeepPosition[] list = component.Positions[position.TemplateID] as KeepPosition[];
					if (list == null)
					{
						list = new KeepPosition[4];
						component.Positions[position.TemplateID] = list;
					}
					//list.SetValue(position, position.Height);
					list[position.Height] = position;
				}
			}
		}

		public static void RemovePosition(KeepPosition position)
		{
			foreach (AbstractGameKeep keep in GameServer.KeepManager.GetAllKeeps())
			{
				foreach (GameKeepComponent component in keep.KeepComponents)
				{
					KeepPosition[] list = component.Positions[position.TemplateID] as KeepPosition[];
					if (list == null)
					{
						list = new KeepPosition[4];
						component.Positions[position.TemplateID] = list;
					}
					//list.SetValue(position, position.Height);
					list[position.Height] = null;
				}
			}
			GameServer.Instance.DeleteDataObject(position);
		}

		public static void FillPositions()
		{
			foreach (AbstractGameKeep keep in GameServer.KeepManager.GetAllKeeps())
			{
				foreach (GameKeepComponent component in keep.KeepComponents)
				{
					component.LoadPositions();
					component.FillPositions();
				}
			}
		}

		/// <summary>
		/// Method to retrieve the Patrol Path from the Patrol ID and Component
		/// 
		/// We need this because we store this all using our offset system
		/// </summary>
		/// <param name="pathname">The path name, which is the Patrol template ID</param>
		/// <param name="component">The Component object</param>
		/// <returns>The Patrol path</returns>
		public static PathPoint LoadPatrolPath(string pathName, GameKeepComponent component)
		{
			var Path = GameServer.Database.Paths.Include(x => x.PathPoints).FirstOrDefault(x => x.PathName == pathName);

			return LoadPatrolPath(Path, component);
		}

		/// <summary>
		/// Method to retrieve the Patrol Path from the Patrol ID and Component
		/// 
		/// We need this because we store this all using our offset system
		/// </summary>
		/// <param name="pathID">The path ID, which is the Patrol ID</param>
		/// <param name="component">The Component object</param>
		/// <returns>The Patrol path</returns>
		public static PathPoint LoadPatrolPath(int pathID, GameKeepComponent component)
		{
			var Path = GameServer.Database.Paths.Include(x => x.PathPoints).FirstOrDefault(x => x.Id == pathID);

			return LoadPatrolPath(Path, component);
		}

		private static PathPoint LoadPatrolPath(Path path, GameKeepComponent component)
        {
			SortedList sorted = new SortedList();			
			IList<PathPoints> pathpoints = null;
			ePathType pathType = ePathType.Once;

			if (path != null)
			{
				pathType = (ePathType)path.PathType;
			}
			if (pathpoints == null)
			{
				pathpoints = path.PathPoints.ToList();
			}

			foreach (var point in pathpoints)
			{
				sorted.Add(point.Step, point);
			}
			PathPoint prev = null;
			PathPoint first = null;
			for (int i = 0; i < sorted.Count; i++)
			{
				PathPoint pp = (PathPoint)sorted.GetByIndex(i);
				PathPoint p = new PathPoint(pp.X, pp.Y, pp.Z, pp.MaxSpeed, pathType);

				int x, y;
				LoadXY(component, pp.X, pp.Y, out x, out y);
				p.X = x;
				p.Y = y;
				p.Z = component.Keep.Z + p.Z;

				p.WaitTime = pp.WaitTime;

				if (first == null)
				{
					first = p;
				}
				p.Prev = prev;
				if (prev != null)
				{
					prev.Next = p;
				}
				prev = p;
			}
			return first;
		}

		/// <summary>
        /// Method to save the Patrol Path using the Patrol ID and the Component
		/// </summary>
		/// <param name="pathID"></param>
		/// <param name="path"></param>
		/// <param name="component"></param>
		public static void SavePatrolPath(int pathID, PathPoint path, GameKeepComponent component)
		{
			if (path == null)
				return;


			GameServer.Instance.DeleteDataObject(GameServer.Database.Paths.Find(pathID));
			PathPoint root = MovementMgr.FindFirstPathPoint(path);

			//Set the current pathpoint to the rootpoint!
			path = root;
			Path dbp = new Path()
			{
				PathType = (int)ePathType.Loop
			};
			GameServer.Instance.SaveDataObject(dbp);

			int i = 1;
			do
			{
				var dbpp = new PathPoints()
				{
					X = path.X,
					Y = path.Y,
					Z = path.Z,
					MaxSpeed = path.MaxSpeed
				};
				int x, y;
				SaveXY(component, dbpp.X, dbpp.Y, out x, out y);
				dbpp.X = x;
				dbpp.Y = y;
				dbpp.Z = dbpp.Z - component.Z;

				dbpp.Step = i++;
				dbpp.PathID = pathID;
				dbpp.WaitTime = path.WaitTime;
				GameServer.Instance.SaveDataObject(dbpp);
				path = path.Next;
			} while (path != null && path != root);
		}

		public static void CreateDoor(int doorID, GamePlayer player)
		{
			int ownerKeepId = (doorID / 100000) % 1000;
			int towerNum = (doorID / 10000) % 10;
			int keepID = ownerKeepId + towerNum * 256;
			int componentID = (doorID / 100) % 100;
			int doorIndex = doorID % 10;

			AbstractGameKeep keep = GameServer.KeepManager.GetKeepByID(keepID);
			if (keep == null)
			{
				player.Out.SendMessage("Cannot create door as keep is null!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
				return;
			}
			GameKeepComponent component = null;
			foreach (GameKeepComponent c in keep.KeepComponents)
			{
				if (c.ID == componentID)
				{
					component = c;
					break;
				}
			}
			if (component == null)
			{
				player.Out.SendMessage("Cannot create door as component is null!", eChatType.CT_System, eChatLoc.CL_SystemWindow);
				return;
			}
			KeepPosition pos = new KeepPosition();
			pos.ClassType = "DOL.GS.Keeps.GameKeepDoor";
			pos.TemplateType = doorIndex;
			pos.ComponentSkin = component.Skin;
			pos.ComponentRotation = component.ComponentHeading;
			pos.TemplateID = Guid.NewGuid().ToString();
			int x, y;

			SaveXY(component, player.X, player.Y, out x, out y);
			pos.XOff = x;
			pos.YOff = y;

			pos.ZOff = player.Z - component.Z;

			pos.HOff = player.Heading - component.Heading;

			GameServer.Instance.SaveDataObject(pos);

			player.Out.SendMessage("Added door as a position to keep.  A server restart will be required to load this position.", eChatType.CT_System, eChatLoc.CL_SystemWindow);
		}
	}
}
