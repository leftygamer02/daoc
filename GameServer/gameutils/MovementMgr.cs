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
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Atlas.DataLayer;
using Atlas.DataLayer.Models;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace DOL.GS.Movement
{
    /// <summary>
    /// TODO: instead movement manager we need AI when npc should travel on path and attack 
    /// enemies if they are near and after that return to pathing for example.
    /// this current implementation is incomplete but usable for horses
    /// </summary>
    public class MovementMgr
    {
        /// <summary>
        /// Defines a logger for this class.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static Dictionary<string, Path> m_pathCache = new Dictionary<string, Path>();
		private static object LockObject = new object();
		/// <summary>
		/// Cache all the paths and pathpoints
		/// </summary>
		private static void FillPathCache()
		{
			IList<Path> allPaths = GameServer.Database.Paths.Include(x => x.PathPoints).ToList();
			foreach (Path path in allPaths)
			{
				m_pathCache.Add(path.PathName, path);
			}

			int duplicateCount = 0;

            if (duplicateCount > 0)
                log.ErrorFormat("{0} duplicate steps ignored while loading paths.", duplicateCount);
			log.InfoFormat("Path cache filled with {0} paths.", m_pathCache.Count);
		}

		public static void UpdatePathInCache(string pathName)
		{
			log.DebugFormat("Updating path {0} in path cache.", pathName);

			var Path = GameServer.Database.Paths.Include(x => x.PathPoints).FirstOrDefault(x => x.PathName == pathName);
			if (Path != null)
			{
				if (m_pathCache.ContainsKey(pathName))
				{
					m_pathCache[pathName] = Path;
				}
				else
				{
					m_pathCache.Add(Path.PathName, Path);
				}
			}
		}

        /// <summary>
        /// loads a path from the cache
        /// </summary>
        /// <param name="pathName">path to load</param>
        /// <returns>first pathpoint of path or null if not found</returns>
        public static PathPoint LoadPath(string pathName)
        {
        	lock(LockObject)
        	{
	        	if (m_pathCache.Count == 0)
				{
					FillPathCache();
				}
	
				Path Path = null;
	
				if (m_pathCache.ContainsKey(pathName))
				{
					Path = m_pathCache[pathName];
				}
	
				// even if path entry not found see if pathpoints exist and try to use it
	
	            ePathType pathType = ePathType.Once;
	
	            if (Path != null)
	            {
	                pathType = (ePathType)Path.PathType;
	            }
	
	            PathPoint prev = null;
	            PathPoint first = null;
	
				foreach (var pp in Path.PathPoints)
				{
					PathPoint p = new PathPoint(pp.X, pp.Y, pp.Z, pp.MaxSpeed, pathType);
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
        }

		/// <summary>
		/// Saves the path into the database
		/// </summary>
		/// <param name="pathName">The path to save</param>
		/// <param name="path">The path waypoint</param>
		public static void SavePath(string pathName, PathPoint pathPoint)
        {
            if (pathPoint == null)
                return;

			// First delete any path with this pathID from the database

			var path = GameServer.Database.Paths.FirstOrDefault(x => x.PathName == pathName);
			if (path != null)
			{
				GameServer.Instance.DeleteDataObject(path);
			}

			// Now add this path and iterate through the PathPoint linked list to add all the path points

            PathPoint root = FindFirstPathPoint(pathPoint);

            //Set the current pathpoint to the rootpoint!
            pathPoint = root;
			path = new Path()
			{
				PathName = pathName,
				PathType = (int)root.Type
			};
            GameServer.Instance.SaveDataObject(path);

            int i = 1;
            do
            {
				var dbpp = new PathPoints()
				{
					X = pathPoint.X,
					Y = pathPoint.Y,
					Z = pathPoint.Z,
					MaxSpeed = pathPoint.MaxSpeed
				};
                dbpp.Step = i++;
                dbpp.PathID = path.Id;
                dbpp.WaitTime = pathPoint.WaitTime;
                GameServer.Instance.SaveDataObject(dbpp);
				pathPoint = pathPoint.Next;
            }
			while (pathPoint != null && pathPoint != root);

			UpdatePathInCache(pathName);
        }

        /// <summary>
        /// Searches for the first point in the waypoints chain
        /// </summary>
        /// <param name="path">One of the pathpoints</param>
        /// <returns>The first pathpoint in the chain or null</returns>
        public static PathPoint FindFirstPathPoint(PathPoint path)
        {
            PathPoint root = path;
            // avoid circularity
            int iteration = 50000;
            while (path.Prev != null && path.Prev != root)
            {
                path = path.Prev;
                iteration--;
                if (iteration <= 0)
                {
                    if (log.IsErrorEnabled)
                        log.Error("Path cannot be saved, it seems endless");
                    return null;
                }
            }
            return path;
        }
    }
}