namespace DOL.GS;

public struct StatsComponent
{
    private int Strength;
    private int Dexterity;
    private int Constitution;
    private int Quickness;
    private int Intelligence;
    private int Piety;
    private int Empathy;
    private int Charisma;

    public int GetStat(eStat stat)
    {
        switch (stat)
        {
            case eStat.STR:
                return Strength;
            case eStat.DEX:
                return Dexterity;
            case eStat.CON:
                return Constitution;
            case eStat.QUI:
                return Quickness;
            case eStat.INT:
                return Intelligence;
            case eStat.PIE:
                return Piety;
            case eStat.EMP:
                return Empathy;
            case eStat.CHR:
                return Charisma;
            default:
                return 0;
        }
    }

    public void SetStat(eStat stat, int value)
    {
        switch (stat)
        {
            case eStat.STR:
                Strength = value;
                break;
            case eStat.DEX:
                Dexterity = value;
                break;
            case eStat.CON:
                Constitution = value;
                break;
            case eStat.QUI:
                Quickness = value;
                break;
            case eStat.INT:
                Intelligence = value;
                break;
            case eStat.PIE:
                Piety = value;
                break;
            case eStat.EMP:
                Empathy = value;
                break;
            case eStat.CHR:
                Charisma = value;
                break;
            default:
                break;
        }
    }

    public int IncreaseStat(eStat stat, int valueToIncreaseBy)
    {
        switch (stat)
        {
            case eStat.STR:
                Strength += valueToIncreaseBy;
                return Strength;
            case eStat.DEX:
                Dexterity += valueToIncreaseBy;
                return Dexterity;
            case eStat.CON:
                Constitution += valueToIncreaseBy;
                return Constitution;
            case eStat.QUI:
                Quickness += valueToIncreaseBy;
                return Quickness;
            case eStat.INT:
                Intelligence += valueToIncreaseBy;
                return Intelligence;
            case eStat.PIE:
                Piety += valueToIncreaseBy;
                return Piety;
            case eStat.EMP:
                Empathy += valueToIncreaseBy;
                return Empathy;
            case eStat.CHR:
                Charisma += valueToIncreaseBy;
                return Charisma;
            default:
                return 0;
        }
    }

    public int DecreaseStat(eStat stat, int valueToDecreaseBy)
    {
        switch (stat)
        {
            case eStat.STR:
                Strength -= valueToDecreaseBy;
                return Strength;
            case eStat.DEX:
                Dexterity -= valueToDecreaseBy;
                return Dexterity;
            case eStat.CON:
                Constitution -= valueToDecreaseBy;
                return Constitution;
            case eStat.QUI:
                Quickness -= valueToDecreaseBy;
                return Quickness;
            case eStat.INT:
                Intelligence -= valueToDecreaseBy;
                return Intelligence;
            case eStat.PIE:
                Piety -= valueToDecreaseBy;
                return Piety;
            case eStat.EMP:
                Empathy -= valueToDecreaseBy;
                return Empathy;
            case eStat.CHR:
                Charisma -= valueToDecreaseBy;
                return Charisma;
            default:
                return 0;
        }
    }
}