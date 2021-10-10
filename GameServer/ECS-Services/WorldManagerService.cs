using System;
using ECS.Debug;

namespace DOL.GS
{
    public static class WorldManagerService 
        {
        private const string ServiceName = "WorldManagerService";
        static WorldManagerService()
        {
            EntityManager.AddService(typeof(CastingService));
        }

        public static void Tick(long tick)
        {
            Diagnostics.StartPerfCounter(ServiceName);

            WorldUpdateThread.OnTick();

            Diagnostics.StopPerfCounter(ServiceName);
        }


        //Parrellel Thread does this
        private static void HandleTick(long tick)
        {
            
        }
    }
}