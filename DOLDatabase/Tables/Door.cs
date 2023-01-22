/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */

using DOL.Database.Attributes;

namespace DOL.Database;

/// <summary>
/// DBDoor is database of door with state of door and X,Y,Z
/// </summary>
[DataTable(TableName = "Door")]
public class DBDoor : DataObject
{
    private int m_xpos;
    private int m_ypos;
    private int m_zpos;
    private int m_heading;
    private string m_name;
    private int m_type;
    private int m_internalID;
    private byte m_level;
    private byte m_realm;
    private string m_guild;
    private uint m_flags;
    private int m_locked;
    private int m_health;
    private int m_maxHealth; // Unused
    private bool m_isPostern;
    private int m_state; // DOL.GS.eDoorState

    /// <summary>
    /// Name of door
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string Name
    {
        get => m_name;
        set
        {
            Dirty = true;
            m_name = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int Type
    {
        get => m_type;
        set
        {
            Dirty = true;
            m_type = value;
        }
    }

    /// <summary>
    /// Z position of door
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public int Z
    {
        get => m_zpos;
        set
        {
            Dirty = true;
            m_zpos = value;
        }
    }

    /// <summary>
    /// Y position of door
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public int Y
    {
        get => m_ypos;
        set
        {
            Dirty = true;
            m_ypos = value;
        }
    }

    /// <summary>
    /// X position of door
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public int X
    {
        get => m_xpos;
        set
        {
            Dirty = true;
            m_xpos = value;
        }
    }

    /// <summary>
    /// Heading of door
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public int Heading
    {
        get => m_heading;
        set
        {
            Dirty = true;
            m_heading = value;
        }
    }

    /// <summary>
    /// Internal index of Door
    /// </summary>
    [DataElement(AllowDbNull = false, Index = true)]
    public int InternalID
    {
        get => m_internalID;
        set
        {
            Dirty = true;
            m_internalID = value;
        }
    }

    [DataElement(AllowDbNull = true)]
    public string Guild
    {
        get => m_guild;
        set
        {
            Dirty = true;
            m_guild = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public byte Level
    {
        get => m_level;
        set
        {
            Dirty = true;
            m_level = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public byte Realm
    {
        get => m_realm;
        set
        {
            Dirty = true;
            m_realm = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public uint Flags
    {
        get => m_flags;
        set
        {
            Dirty = true;
            m_flags = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int Locked
    {
        get => m_locked;
        set
        {
            Dirty = true;
            m_locked = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int Health
    {
        get => m_health;
        set
        {
            Dirty = true;
            m_health = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int MaxHealth
    {
        get => m_maxHealth;
        set
        {
            Dirty = true;
            m_maxHealth = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public bool IsPostern
    {
        get => m_isPostern;
        set
        {
            Dirty = true;
            m_isPostern = value;
        }
    }

    [DataElement(AllowDbNull = false)]
    public int State
    {
        get => m_state;
        set
        {
            Dirty = true;
            m_state = value;
        }
    }
}