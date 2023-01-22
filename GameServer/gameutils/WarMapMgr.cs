using System;
using System.Reflection;
using System.Collections.Generic;
using DOL.GS;
using log4net;

namespace DOL.GS;

public class WarMapMgr
{
    private const long REMAIN_IN_CACHE = 2 * 60 * 1000; // 2min
    private const long REFRESH_INTERVAL = 10 * 1000; // 10sec
    private const int FIGHTS_RATIO = 4; // 4 players = small fight, *2 = normal, *3 = big, *4 = huge
    private const int GROUPS_RATIO = 8; // 8 players = small icon, etc

    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private static long LastCalcul = 0;
    private static Dictionary<long, Fight> m_fights = new();
    private static Dictionary<string, Dictionary<long, Group>> m_groups = new();

    private static Dictionary<byte, Dictionary<byte, Dictionary<byte, Dictionary<byte, int>>>> b_fights = new();

    private static Dictionary<byte, Dictionary<byte, Dictionary<byte, Dictionary<byte, int>>>> b_groups = new();

    private static List<List<byte>> w_fights = new();
    private static List<List<byte>> w_groups = new();

    public static long NFTime => WorldMgr.GetRegion(163).Time;

    public class Fight
    {
        public byte Zone;

        //    _0_1_2_3_   X / Y
        // 0 | x x x x |
        // 1 | x x x x |
        // 2 | x x x x |
        // 3 |_x_x_x_x_|
        public byte X; // 0..3
        public byte Y; // 0..3
        public byte Realm1;
        public byte Realm2;
    }

    public class Group
    {
        public byte Zone;
        public byte X; // 0..3
        public byte Y; // 0..3
        public byte Realm;
    }

    public static bool AddFight(byte zoneid, int x, int y, byte realm1, byte realm2)
    {
        if (!ServerProperties.Properties.ENABLE_WARMAPMGR)
            return false;

        var zone = WorldMgr.GetZone(zoneid);
        if (zone == null) return false;
        var time = NFTime;
        var fight = new Fight();
        fight.Zone = zoneid;
        fight.X = (byte) ((x - zone.XOffset) >> 14);
        fight.Y = (byte) ((y - zone.YOffset) >> 14);
        fight.Realm1 = realm1;
        fight.Realm2 = realm2;
        lock (m_fights)
        {
            while (m_fights.ContainsKey(time)) time++;
            m_fights.Add(time, fight);
        }

        return true;
    }

    public static bool AddGroup(byte zoneid, int x, int y, string name, byte realm)
    {
        if (!ServerProperties.Properties.ENABLE_WARMAPMGR)
            return false;

        lock (m_groups)
        {
            if (m_groups.ContainsKey(name)) return false;
            m_groups.Add(name, new Dictionary<long, Group>());
            var zone = WorldMgr.GetZone(zoneid);
            if (zone == null) return false;
            var time = NFTime;
            var group = new Group();
            group.Zone = zoneid;
            group.X = (byte) ((x - zone.XOffset) >> 14);
            group.Y = (byte) ((y - zone.YOffset) >> 14);
            group.Realm = realm;
            while (m_groups[name].ContainsKey(time)) time++;
            m_groups[name].Add(time, group);
        }

        return true;
    }

    public static void Calcul()
    {
        try
        {
            var nftime = NFTime;

            #region CalculFights

            lock (m_fights)
            {
                IList<long> remove = new List<long>();
                foreach (var time in m_fights.Keys)
                    if (time + REMAIN_IN_CACHE < nftime)
                        remove.Add(time);

                foreach (var time in remove)
                    if (m_fights.ContainsKey(time))
                        m_fights.Remove(time);

                lock (b_fights)
                {
                    b_fights.Clear();
                    foreach (var fight in m_fights.Values)
                    {
                        if (!b_fights.ContainsKey(fight.Zone))
                            b_fights.Add(fight.Zone,
                                new Dictionary<byte, Dictionary<byte, Dictionary<byte, int>>>());
                        if (!b_fights[fight.Zone].ContainsKey(fight.X))
                            b_fights[fight.Zone].Add(fight.X, new Dictionary<byte, Dictionary<byte, int>>());
                        if (!b_fights[fight.Zone][fight.X].ContainsKey(fight.Y))
                            b_fights[fight.Zone][fight.X].Add(fight.Y, new Dictionary<byte, int>(3));
                        if (!b_fights[fight.Zone][fight.X][fight.Y].ContainsKey(1))
                            b_fights[fight.Zone][fight.X][fight.Y].Add(1, 0);
                        if (!b_fights[fight.Zone][fight.X][fight.Y].ContainsKey(2))
                            b_fights[fight.Zone][fight.X][fight.Y].Add(2, 0);
                        if (!b_fights[fight.Zone][fight.X][fight.Y].ContainsKey(3))
                            b_fights[fight.Zone][fight.X][fight.Y].Add(3, 0);

                        b_fights[fight.Zone][fight.X][fight.Y][fight.Realm1]++;
                        b_fights[fight.Zone][fight.X][fight.Y][fight.Realm2]++;
                    }
                }
            }

            lock (w_fights)
            {
                w_fights.Clear();
                lock (b_fights)
                {
                    foreach (var zone in b_fights.Keys)
                    foreach (var x in b_fights[zone].Keys)
                    foreach (var y in b_fights[zone][x].Keys)
                    {
                        var alb = b_fights[zone][x][y][1];
                        var mid = b_fights[zone][x][y][2];
                        var hib = b_fights[zone][x][y][3];
                        byte realm = 0x00;
                        if ((alb >= mid && mid >= hib) || (mid >= alb && alb >= hib)) realm = 0x01;
                        else if ((alb >= hib && hib >= mid) || (hib >= alb && alb >= mid)) realm = 0x02;
                        else /*if ((hib >= mid && mid >= alb) || (mid >= hib && hib >= alb))*/
                            realm = 0x03;
                        var size = (alb + hib + mid) / FIGHTS_RATIO;
                        if (size > 3) size = 3;
                        if (size >= 1)
                            w_fights.Add(new List<byte> {zone, x, y, realm, (byte) size});
                    }
                }
            }

            #endregion CalculFights

            #region CalculGroups

            lock (m_groups)
            {
                IList<string> remove = new List<string>();
                foreach (var name in m_groups.Keys)
                foreach (var time in m_groups[name].Keys)
                    if (time + REMAIN_IN_CACHE < nftime)
                        remove.Add(name);

                foreach (var name in remove)
                    if (m_groups.ContainsKey(name))
                        m_groups.Remove(name);

                lock (b_groups)
                {
                    b_groups.Clear();
                    foreach (var name in m_groups.Keys)
                    foreach (var group in m_groups[name].Values)
                    {
                        if (!b_groups.ContainsKey(group.Zone))
                            b_groups.Add(group.Zone,
                                new Dictionary<byte, Dictionary<byte, Dictionary<byte, int>>>());
                        if (!b_groups[group.Zone].ContainsKey(group.X))
                            b_groups[group.Zone].Add(group.X, new Dictionary<byte, Dictionary<byte, int>>());
                        if (!b_groups[group.Zone][group.X].ContainsKey(group.Y))
                            b_groups[group.Zone][group.X].Add(group.Y, new Dictionary<byte, int>(3));
                        if (!b_groups[group.Zone][group.X][group.Y].ContainsKey(1))
                            b_groups[group.Zone][group.X][group.Y].Add(1, 0);
                        if (!b_groups[group.Zone][group.X][group.Y].ContainsKey(2))
                            b_groups[group.Zone][group.X][group.Y].Add(2, 0);
                        if (!b_groups[group.Zone][group.X][group.Y].ContainsKey(3))
                            b_groups[group.Zone][group.X][group.Y].Add(3, 0);

                        b_groups[group.Zone][group.X][group.Y][group.Realm]++;
                    }
                }
            }

            lock (w_groups)
            {
                w_groups.Clear();
                lock (b_groups)
                {
                    foreach (var zone in b_groups.Keys)
                    foreach (var x in b_groups[zone].Keys)
                    foreach (var y in b_groups[zone][x].Keys)
                    {
                        var alb = b_groups[zone][x][y][1];
                        var mid = b_groups[zone][x][y][2];
                        var hib = b_groups[zone][x][y][3];
                        byte realm = 0x00;
                        var size = 0;
                        if (alb >= mid && alb >= hib)
                        {
                            realm = 0x01;
                            size = alb;
                        }
                        else if (mid >= hib && mid >= alb)
                        {
                            realm = 0x02;
                            size = mid;
                        }
                        else /*if ((hib >= mid && mid >= alb) || (mid >= hib && hib >= alb))*/
                        {
                            realm = 0x03;
                            size = hib;
                        }

                        size /= GROUPS_RATIO;
                        if (size > 3) size = 3;
                        if (size >= 1)
                            w_groups.Add(new List<byte> {zone, x, y, realm, (byte) size});
                    }
                }
            }

            #endregion CalculGroups

            LastCalcul = nftime;
        }
        catch (Exception e)
        {
            log.Error("WarMapMgr.Calcul: " + e);
        }
    }

    public static void SendFightInfo(GameClient client)
    {
        if (!ServerProperties.Properties.ENABLE_WARMAPMGR)
            return;

        if (client == null || client.Player == null) return;

        if (LastCalcul + REFRESH_INTERVAL < NFTime) Calcul();

        client.Out.SendWarmapDetailUpdate(w_fights, w_groups);
    }
}