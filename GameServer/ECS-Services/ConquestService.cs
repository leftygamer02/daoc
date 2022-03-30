using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOL.Database;
using DOL.GS.Quests;
using ECS.Debug;

namespace DOL.GS;

public class ConquestService
{
    private const string ServiceName = "Conquest Service";

    public static ConquestManager ConquestManager;


    static ConquestService()
    {
        EntityManager.AddService(typeof(ConquestService));
        ConquestManager = new ConquestManager();
    }

    public static void Tick(long tick)
    {
        Diagnostics.StartPerfCounter(ServiceName);
        
        if(ConquestManager.LastTaskRolloverTick + ServerProperties.Properties.MAX_CONQUEST_TASK_DURATION * 60000 < GameLoop.GameLoopTime) //multiply by 60k ms to accomodate minute input
        {
            ConquestManager.RotateKeeps();
        }else if((GameLoop.GameLoopTime - ConquestManager.LastTaskRolloverTick) % 300000 == 0) //every 5 minutes
        {
            foreach (var activeObjective in ConquestManager.GetActiveObjectives)
            {
                activeObjective.DoSubtaskRollover();
            }
        }
        //Console.WriteLine($"conquest heartbeat {GameLoop.GameLoopTime} countdown {GameLoop.GameLoopTime - (ConquestManager.LastTaskRolloverTick + ServerProperties.Properties.MAX_CONQUEST_TASK_DURATION * 10000)}");
        
        Diagnostics.StopPerfCounter(ServiceName);
    }
}