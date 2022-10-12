using System;
using System.Collections.Generic;
using DOL.GS;

namespace DOL.GS;

public class Bindstones
{
    private List<BindstoneLocation> AvailableBindstones;

    public Bindstones()
    {
        AvailableBindstones = new List<BindstoneLocation>();
        //albion
        AvailableBindstones.Add(new BindstoneLocation(1, 560633, 511913, 2280)); //cotswold
        //midgard
        AvailableBindstones.Add(new BindstoneLocation(100, 804732, 724037, 4680)); //mularn
        //hibernia
        AvailableBindstones.Add(new BindstoneLocation(200, 345972, 490734, 5200)); //mag mell
    }

    public BindstoneLocation GetRandomBindstone()
    {
        int index = Util.Random(AvailableBindstones.Count - 1);
        Console.WriteLine($"index: {index} region {AvailableBindstones[index].Region}");
        return AvailableBindstones[index];
    }
}

public class BindstoneLocation
{
    public int Region;
    public int X;
    public int Y;
    public int Z;

    public BindstoneLocation(int region, int x, int y, int z)
    {
        Region = region;
        X = x;
        Y = y;
        Z = z;
    }
}

public static class BindstoneManager
{
    public static Bindstones BindstoneList;

    static BindstoneManager()
    {
        BindstoneList = new Bindstones();
    }
}