using ECS.Debug;

namespace DOL.GS;

public class RelicService
{
    private const string ServiceName = "Bounty Service";

    private static RelicManager _relicManager;

    // private static long _updateInterval = 10000; // 10secs
    private static long _updateInterval = ServerProperties.Properties.BOUNTY_CHECK_INTERVAL * 1000;

    private static long _lastUpdate;

    static RelicService()
    {
        EntityManager.AddService(typeof(BountyService));
        _relicManager = new RelicManager();
    }

    public static void Tick(long tick)
    {
        Diagnostics.StartPerfCounter(ServiceName);

        if (tick - _lastUpdate > _updateInterval)
        {
            _lastUpdate = tick;
            RelicManager.MonitorKeeps();
        }

        Diagnostics.StopPerfCounter(ServiceName);
    }
}