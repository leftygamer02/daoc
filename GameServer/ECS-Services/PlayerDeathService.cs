using System;
using System.Collections.Generic;
using ECS.Debug;

namespace DOL.GS {
    public static class PlayerDeathService {

        private static Dictionary<GamePlayer, GameObject> m_playersToKill;

        private const string ServiceName = "PlayerDeathService";
        static PlayerDeathService()
        {
            m_playersToKill = new Dictionary<GamePlayer, GameObject>();
        }

        public static bool RegisterDeath(GamePlayer player, GameObject killer)
        {
            m_playersToKill.Add(player, killer);
            return true;
        }

        public static void Tick(long tick)
        {
            Diagnostics.StartPerfCounter(ServiceName);

            if(m_playersToKill.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<GamePlayer, GameObject> pl in m_playersToKill)
            {
                pl.Key.ServiceDie(pl.Key, pl.Value);
                m_playersToKill.Remove(pl.Key);
            }

            Diagnostics.StopPerfCounter(ServiceName);
        }

    }
}