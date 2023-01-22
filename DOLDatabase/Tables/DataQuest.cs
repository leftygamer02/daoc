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

using System;
using DOL.Database.Attributes;

namespace DOL.Database;

/// <summary>
/// Holds all the DataQuests available
/// </summary>
[DataTable(TableName = "DataQuest")]
public class DBDataQuest : DataObject
{
    private int m_id;
    private string m_name;
    private byte m_startType;
    private string m_startName;
    private ushort m_startRegionID;
    private string m_acceptText;
    private string m_description;
    private string m_sourceName;
    private string m_sourceText;
    private string m_stepType;
    private string m_stepText;
    private string m_stepItemTemplates;
    private string m_advanceText;
    private string m_targetName;
    private string m_targetText;
    private string m_collectItemTemplate;
    private short m_maxCount;
    private byte m_minLevel;
    private byte m_maxLevel;
    private string m_rewardMoney;
    private string m_rewardXP;
    private string m_rewardCLXP;
    private string m_rewardRP;
    private string m_rewardBP;
    private string m_optionalRewardItemTemplates;
    private string m_finalRewardItemTemplates;
    private string m_finishText;
    private string m_questDependency;
    private string m_allowedClasses;
    private string m_classType;


    public DBDataQuest()
    {
    }

    [PrimaryKey(AutoIncrement = true)]
    public int ID
    {
        get => m_id;
        set => m_id = value;
    }

    /// <summary>
    /// The name of this quest
    /// </summary>
    [DataElement(Varchar = 255, AllowDbNull = false)]
    public string Name
    {
        get => m_name;
        set
        {
            m_name = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// The start type of this quest (eStartType)
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public byte StartType
    {
        get => m_startType;
        set
        {
            m_startType = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// The name of the object that starts this quest
    /// </summary>
    [DataElement(Varchar = 100, AllowDbNull = false)]
    public string StartName
    {
        get => m_startName;
        set
        {
            m_startName = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// The region id where this quest starts
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public ushort StartRegionID
    {
        get => m_startRegionID;
        set
        {
            m_startRegionID = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// The whisper text that will start this quest
    /// </summary>
    [DataElement(Varchar = 100, AllowDbNull = true)]
    public string AcceptText
    {
        get => m_acceptText;
        set
        {
            m_acceptText = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Description to show to start quest
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string Description
    {
        get => m_description;
        set
        {
            m_description = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Who to talk to for each step
    /// Format: SourceName;RegionID|SourceName;RegionID
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string SourceName
    {
        get => m_sourceName;
        set
        {
            m_sourceName = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// The text for each source
    /// Format:  Step 1 Source text|Step 2 Source text
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string SourceText
    {
        get => m_sourceText;
        set
        {
            m_sourceText = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Type of each step (kill, give, collect, etc)
    /// Format: Step1Type|Step2Type
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string StepType
    {
        get => m_stepType;
        set
        {
            m_stepType = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Description text for each step
    /// Format: Step 1 Text|Step 2 Text
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string StepText
    {
        get => m_stepText;
        set
        {
            m_stepText = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Items given to the player at a step
    /// Format: id_nb|idnb
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string StepItemTemplates
    {
        get => m_stepItemTemplates;
        set
        {
            m_stepItemTemplates = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Text required to advance to the next step
    /// Format: Step 1 Text|Step 2 Text
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string AdvanceText
    {
        get => m_advanceText;
        set
        {
            m_advanceText = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Name of the target for each step
    /// Format: TargetName;RegionID|TargetName;RegionID
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string TargetName
    {
        get => m_targetName;
        set
        {
            m_targetName = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Text for each target
    /// Format:  Step 1 Target text|Step 2 Target text| ...
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string TargetText
    {
        get => m_targetText;
        set
        {
            m_targetText = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// ItemTemplate id_nb to be collected to finish the current step
    /// Format: id_nb|id_nb||  steps with no item to collect should be blank
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string CollectItemTemplate
    {
        get => m_collectItemTemplate;
        set
        {
            m_collectItemTemplate = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Max number of times a player can do this quest
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public short MaxCount
    {
        get => m_maxCount;
        set
        {
            m_maxCount = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Minimum level a player has to be to start this quest
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public byte MinLevel
    {
        get => m_minLevel;
        set
        {
            m_minLevel = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Max level a player can be and still do this quest
    /// </summary>
    [DataElement(AllowDbNull = false)]
    public byte MaxLevel
    {
        get => m_maxLevel;
        set
        {
            m_maxLevel = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Reward Money to give at each step, 0 for none
    /// Format: 111|222|0|333
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string RewardMoney
    {
        get => m_rewardMoney;
        set
        {
            m_rewardMoney = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Reward XP to give at each step, 0 for none
    /// Format: 123456789|99876543|0|10000000
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string RewardXP
    {
        get => m_rewardXP;
        set
        {
            m_rewardXP = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Reward CLXP to give at each step, 0 for none
    /// Format: 123456789|99876543|0|10000000
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string RewardCLXP
    {
        get => m_rewardCLXP;
        set
        {
            m_rewardCLXP = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Reward RP to give at each step, 0 for none
    /// Format: 123456789|99876543|0|10000000
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string RewardRP
    {
        get => m_rewardRP;
        set
        {
            m_rewardRP = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Reward BP to give at each step, 0 for none
    /// Format: 123456789|99876543|0|10000000
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string RewardBP
    {
        get => m_rewardBP;
        set
        {
            m_rewardBP = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// The ItemTemplate id_nb(s) to give as a optional rewards
    /// Format:  #id_nb1|id_nb2 with first character being the number of choices
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string OptionalRewardItemTemplates
    {
        get => m_optionalRewardItemTemplates;
        set
        {
            m_optionalRewardItemTemplates = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// The ItemTemplate id_nb(s) to give as a final reward
    /// Format:  id_nb1|id_nb2
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string FinalRewardItemTemplates
    {
        get => m_finalRewardItemTemplates;
        set
        {
            m_finalRewardItemTemplates = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Text to show the user once the quest is finished.  Can be null of no text.
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string FinishText
    {
        get => m_finishText;
        set
        {
            m_finishText = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// The name or names of other quests that need to be done before this quest can be offered.
    /// Name Quest One|Name Quest Two... Can be null if no dependency
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string QuestDependency
    {
        get => m_questDependency;
        set
        {
            m_questDependency = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Player classes that can do this quest.  Null for all.
    /// </summary>
    [DataElement(AllowDbNull = true, Varchar = 200)]
    public string AllowedClasses
    {
        get => m_allowedClasses;
        set
        {
            m_allowedClasses = value;
            Dirty = true;
        }
    }

    /// <summary>
    /// Code that can be used for various quest activities
    /// Can be null, currently not used
    /// </summary>
    [DataElement(AllowDbNull = true)]
    public string ClassType
    {
        get => m_classType;
        set
        {
            m_classType = value;
            Dirty = true;
        }
    }
}