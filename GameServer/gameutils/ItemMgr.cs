using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.GS {
    public static class ItemMgr {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly object _lock = new object();
        private static HashSet<GamePlayerInventory> _trackedChanges = new HashSet<GamePlayerInventory>();
        public static void TrackChange() {

        }

        public static int Save() {
            return 0;
        }

    }
}
