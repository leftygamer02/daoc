using System;
using DOL.Database.Attributes;

namespace DOL.Database;

[DataTable(TableName = "KeepCaptureLog")]
public class KeepCaptureLog : DataObject
{
    private long m_ID;
    private DateTime m_dateTaken = DateTime.Now;
    private string m_keepName;
    private string m_keepType;
    private int m_numEnemies;
    private int m_rpReward;
    private int m_bpReward;
    private long m_xpReward;
    private long m_moneyReward;
    private int m_combatTime;
    private string m_capturedBy;
    private string m_rpGainerList = string.Empty;

    public KeepCaptureLog()
        : base()
    {
    }

    [PrimaryKey(AutoIncrement = true)]
    public long ID
    {
        get => m_ID;
        set
        {
            Dirty = true;
            m_ID = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public DateTime DateTaken
    {
        get => m_dateTaken;
        set
        {
            Dirty = true;
            m_dateTaken = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string KeepName
    {
        get => m_keepName;
        set
        {
            Dirty = true;
            m_keepName = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string KeepType
    {
        get => m_keepType;
        set
        {
            Dirty = true;
            m_keepType = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int NumEnemies
    {
        get => m_numEnemies;
        set
        {
            Dirty = true;
            m_numEnemies = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int CombatTime
    {
        get => m_combatTime;
        set
        {
            Dirty = true;
            m_combatTime = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int RPReward
    {
        get => m_rpReward;
        set
        {
            Dirty = true;
            m_rpReward = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int BPReward
    {
        get => m_bpReward;
        set
        {
            Dirty = true;
            m_bpReward = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public long XPReward
    {
        get => m_xpReward;
        set
        {
            Dirty = true;
            m_xpReward = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public long MoneyReward
    {
        get => m_moneyReward;
        set
        {
            Dirty = true;
            m_moneyReward = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string CapturedBy
    {
        get => m_capturedBy;
        set
        {
            Dirty = true;
            m_capturedBy = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string RPGainerList
    {
        get => m_rpGainerList;
        set
        {
            Dirty = true;
            m_rpGainerList = value;
        }
    }
}