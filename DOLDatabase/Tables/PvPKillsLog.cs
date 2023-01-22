using System;
using DOL.Database.Attributes;

namespace DOL.Database;

[DataTable(TableName = "PvPKillsLog")]
public class PvPKillsLog : DataObject
{
    private long m_ID;
    private DateTime m_dateKilled = DateTime.Now;
    private string m_killedName;
    private string m_killerName;
    private string m_killerIP;
    private string m_killedIP;
    private string m_killerRealm;
    private string m_killedRealm;
    private int m_rpReward;
    private byte m_sameIP = 0;
    private string m_regionName = string.Empty;
    private bool m_isInstance = false;

    public PvPKillsLog()
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
    public DateTime DateKilled
    {
        get => m_dateKilled;
        set
        {
            Dirty = true;
            m_dateKilled = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string KilledName
    {
        get => m_killedName;
        set
        {
            Dirty = true;
            m_killedName = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string KillerName
    {
        get => m_killerName;
        set
        {
            Dirty = true;
            m_killerName = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string KillerIP
    {
        get => m_killerIP;
        set
        {
            Dirty = true;
            m_killerIP = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string KilledIP
    {
        get => m_killedIP;
        set
        {
            Dirty = true;
            m_killedIP = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string KilledRealm
    {
        get => m_killedRealm;
        set
        {
            Dirty = true;
            m_killedRealm = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public string KillerRealm
    {
        get => m_killerRealm;
        set
        {
            Dirty = true;
            m_killerRealm = value;
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
    public byte SameIP
    {
        get => m_sameIP;
        set
        {
            Dirty = true;
            m_sameIP = value;
        }
    }

    [DataElement(AllowDbNull = true)]
    public string RegionName
    {
        get => m_regionName;
        set
        {
            Dirty = true;
            m_regionName = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public bool IsInstance
    {
        get => m_isInstance;
        set
        {
            Dirty = true;
            m_isInstance = value;
        }
    }
}