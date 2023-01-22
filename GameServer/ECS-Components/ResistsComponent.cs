namespace DOL.GS;

public struct ResistsComponent
{
    private int Body;
    private int Cold;
    private int Crush;
    private int Energy;
    private int Heat;
    private int Matter;
    private int Natural;
    private int Slash;
    private int Spirit;
    private int Thrust;


    /// <summary>
    /// Cap for player cast resist buffs.
    /// </summary>
    public static int BuffBonusCap => 24;

    /// <summary>
    /// Hard cap for resists.
    /// </summary>
    public static int HardCap => 70;

    public int GetResist(eResist resist)
    {
        switch (resist)
        {
            case eResist.Body:
                return Body;
            case eResist.Cold:
                return Cold;
            case eResist.Crush:
                return Crush;
            case eResist.Energy:
                return Energy;
            case eResist.Heat:
                return Heat;
            case eResist.Matter:
                return Matter;
            case eResist.Natural:
                return Natural;
            case eResist.Slash:
                return Slash;
            case eResist.Spirit:
                return Spirit;
            case eResist.Thrust:
                return Thrust;
            default:
                return 0;
        }
    }

    public void SetResist(eResist resist, int value)
    {
        switch (resist)
        {
            case eResist.Body:
                Body = value;
                break;
            case eResist.Cold:
                Cold = value;
                break;
            case eResist.Crush:
                Crush = value;
                break;
            case eResist.Energy:
                Energy = value;
                break;
            case eResist.Heat:
                Heat = value;
                break;
            case eResist.Matter:
                Matter = value;
                break;
            case eResist.Natural:
                Natural = value;
                break;
            case eResist.Slash:
                Slash = value;
                break;
            case eResist.Spirit:
                Spirit = value;
                break;
            case eResist.Thrust:
                Thrust = value;
                break;
            default:
                break;
        }
    }

    public int IncreaseResist(eResist resist, int valueToIncreaseBy)
    {
        switch (resist)
        {
            case eResist.Body:
                Body += valueToIncreaseBy;
                return Body;
            case eResist.Cold:
                Cold += valueToIncreaseBy;
                return Cold;
            case eResist.Crush:
                Crush += valueToIncreaseBy;
                return Crush;
            case eResist.Energy:
                Energy += valueToIncreaseBy;
                return Energy;
            case eResist.Heat:
                Heat += valueToIncreaseBy;
                return Heat;
            case eResist.Matter:
                Matter += valueToIncreaseBy;
                return Matter;
            case eResist.Natural:
                Natural += valueToIncreaseBy;
                return Natural;
            case eResist.Slash:
                Slash += valueToIncreaseBy;
                return Slash;
            case eResist.Spirit:
                Spirit += valueToIncreaseBy;
                return Spirit;
            case eResist.Thrust:
                Thrust += valueToIncreaseBy;
                return Thrust;
            default:
                return 0;
        }
    }

    public int DecreaseStat(eResist resist, int valueToDecreaseBy)
    {
        switch (resist)
        {
            case eResist.Body:
                Body += valueToDecreaseBy;
                return Body;
            case eResist.Cold:
                Cold += valueToDecreaseBy;
                return Cold;
            case eResist.Crush:
                Crush += valueToDecreaseBy;
                return Crush;
            case eResist.Energy:
                Energy += valueToDecreaseBy;
                return Energy;
            case eResist.Heat:
                Heat += valueToDecreaseBy;
                return Heat;
            case eResist.Matter:
                Matter += valueToDecreaseBy;
                return Matter;
            case eResist.Natural:
                Natural += valueToDecreaseBy;
                return Natural;
            case eResist.Slash:
                Slash += valueToDecreaseBy;
                return Slash;
            case eResist.Spirit:
                Spirit += valueToDecreaseBy;
                return Spirit;
            case eResist.Thrust:
                Thrust += valueToDecreaseBy;
                return Thrust;
            default:
                return 0;
        }
    }
}