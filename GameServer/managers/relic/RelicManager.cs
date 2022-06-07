using System.Collections.Generic;
using System.Linq;
using DOL.GS.Keeps;


namespace DOL.GS;

public class RelicManager

{
    private static int[] albKeeps = {50, 51, 53};
    private static int[] midKeeps = {75, 76, 79};
    private static int[] hibKeeps = {100, 101, 103};
    
    // we're storing the keep and its spawn status in a dictionary
    private static Dictionary<AbstractGameKeep, bool> monitoredKeeps = new Dictionary<AbstractGameKeep, bool>();

    public RelicManager()
    {
        Init();
    }
    private static void Init()
    {
        foreach (var keep in albKeeps)
        {
            monitoredKeeps.Add(GameServer.KeepManager.GetKeepByID(keep), false);
        }
        
        // foreach (var keep in midKeeps)
        // {
        //     monitoredKeeps.Add(GameServer.KeepManager.GetKeepByID(keep), false);
        // }
        //
        // foreach (var keep in hibKeeps)
        // {
        //     monitoredKeeps.Add(GameServer.KeepManager.GetKeepByID(keep), false);
        // }
    }
    private static int GetGuardsNumber(eRealm realm)
    {
        var relics = RelicMgr.getNFRelics();
        var numRelics = relics.Cast<GameRelic>().Count(relic => relic.Realm == realm);
        var numGuards = 0;
        if (numRelics < 2)
        {
            numGuards = 4;
        }
        else
        {
            numGuards = (int)(4 * (1 - 0.25 * (numRelics - 2)));
        }

        return numGuards;
    }

    public static void MonitorKeeps()
    {
        foreach (var keep in monitoredKeeps)
        {
            if (keep.Key.Realm == keep.Key.OriginalRealm)
            {
                if(!keep.Value)
                    SpawnKeepGuards(keep.Key.KeepID, GetGuardsNumber(keep.Key.OriginalRealm));
                else
                    UpdateKeepGuards(keep.Key.KeepID);
                
            }
            else
            {
                if(keep.Value)
                    DespawnKeepGuards(keep.Key.KeepID);
            }
        }
    }

    private static void SpawnKeepGuards(ushort keepID, int numGuards)
    {
        var keep = GameServer.KeepManager.GetKeepByID(keepID);
        
        var strSpawn = GetStrengthSpawnPoint(keepID);
        var magSpawn = GetMagicSpawnPoint(keepID);
        
        for (int i = 0; i < numGuards; i++)
        {
            var strGuard = new RelicGuard();
            strGuard.X = strSpawn.X + Util.Random(-100, 100);
            strGuard.Y = strSpawn.Y + Util.Random(-100, 100);
            strGuard.Z = strSpawn.Z;
            strGuard.CurrentRegionID = keep.Region;
            strGuard.Realm = keep.OriginalRealm;

            strGuard.LoadedFromScript = false;
            foreach (var area in strGuard.CurrentAreas)
            {
                if (area is not KeepArea) continue;
                var areaKeep = (area as KeepArea).Keep;
                strGuard.Component = new GameKeepComponent();
                strGuard.Component.Keep = areaKeep;
                break;
            }
            strGuard.AddToWorld();
            strGuard.RefreshTemplate();
            strGuard.Name = $"{keep.Name} Defender";
            if (keep.Guild?.Name != "")
            {
                strGuard.GuildName = keep.Guild?.Name;
            }
            
            var magGuard = new RelicGuard();
            magGuard.X = magSpawn.X + Util.Random(-100, 100);
            magGuard.Y = magSpawn.Y + Util.Random(-100, 100);
            magGuard.Z = magSpawn.Z;
            magGuard.CurrentRegionID = keep.Region;
            magGuard.Realm = keep.OriginalRealm;

            magGuard.LoadedFromScript = false;
            foreach (var area in magGuard.CurrentAreas)
            {
                if (area is not KeepArea) continue;
                var areaKeep = (area as KeepArea).Keep;
                magGuard.Component = new GameKeepComponent();
                magGuard.Component.Keep = areaKeep;
                break;
            }
            magGuard.AddToWorld();
            magGuard.RefreshTemplate();
            magGuard.Name = $"{keep.Name} Defender";
            if (keep.Guild?.Name != "")
            {
                magGuard.GuildName = keep.Guild?.Name;
            }
        }
        
        monitoredKeeps[keep] = true;
    }
    
    private static void UpdateKeepGuards(ushort keepID)
    {
        var keep = GameServer.KeepManager.GetKeepByID(keepID);
        
        foreach(var npc in WorldMgr.GetNPCsFromRegion(keep.Region))
        {
            if (npc is not RelicGuard)
            {
                continue;
            }

            if (!npc.Name.Contains(keep.Name)) continue;
            if (keep.Guild?.Name != "")
            {
                npc.GuildName = keep.Guild?.Name;
            }
        }
    }

    private static Point3D GetStrengthSpawnPoint(ushort keepID)
    {
        var keep = GameServer.KeepManager.GetKeepByID(keepID);
        var point = new Point3D();

        switch (keep.OriginalRealm)
        {
            case eRealm.Albion:
                point = keep.KeepID switch
                {
                    50 => new Point3D(507580, 309122, 6832),
                    51 => new Point3D(507580, 309122, 6832),
                    53 => new Point3D(507580, 309122, 6832),
                    _ => point
                };
                break;
            
            case eRealm.Midgard:
                point = keep.KeepID switch
                {
                    54 => new Point3D(507580, 309122, 6832),
                    55 => new Point3D(507580, 309122, 6832),
                    56 => new Point3D(507580, 309122, 6832),
                    _ => point
                };
                break;
            
            case eRealm.Hibernia:
                point = keep.KeepID switch
                {
                    54 => new Point3D(507580, 309122, 6832),
                    55 => new Point3D(507580, 309122, 6832),
                    56 => new Point3D(507580, 309122, 6832),
                    _ => point
                };
                break;
            
        }

        return point;
    }


    private static Point3D GetMagicSpawnPoint(ushort keepID)
    {
        var keep = GameServer.KeepManager.GetKeepByID(keepID);
        var point = new Point3D();

        switch (keep.OriginalRealm)
        {
            case eRealm.Albion:
                point = keep.KeepID switch
                {
                    50 => new Point3D(507580, 309122, 6832),
                    51 => new Point3D(507580, 309122, 6832),
                    53 => new Point3D(507580, 309122, 6832),
                    _ => point
                };
                break;
            
            case eRealm.Midgard:
                point = keep.KeepID switch
                {
                    54 => new Point3D(507580, 309122, 6832),
                    55 => new Point3D(507580, 309122, 6832),
                    56 => new Point3D(507580, 309122, 6832),
                    _ => point
                };
                break;
            
            case eRealm.Hibernia:
                point = keep.KeepID switch
                {
                    54 => new Point3D(507580, 309122, 6832),
                    55 => new Point3D(507580, 309122, 6832),
                    56 => new Point3D(507580, 309122, 6832),
                    _ => point
                };
                break;
            
        }

        return point;
    }
    
    private static void DespawnKeepGuards(ushort keepID)
    {
        var keep = GameServer.KeepManager.GetKeepByID(keepID);
        
        foreach(var npc in WorldMgr.GetNPCsFromRegion(keep.Region))
        {
            if (npc is not RelicGuard)
            {
                continue;
            }
            if (npc.Name.Contains(keep.Name))
            {
                npc.Delete();
            }
        }
        monitoredKeeps[keep] = false;
    }

}