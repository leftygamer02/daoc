using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class DataQuestRewardQuest : DataObjectBase
    {
        public string QuestName { get; set; }
        public string StartNPC { get; set; }
        public int StartRegionID { get; set; }
        public string StoryText { get; set; }
        public string Summary { get; set; }
        public string AcceptText { get; set; }
        public string QuestGoals { get; set; }
        public string GoalType { get; set; }
        public string GoalRepeatNo { get; set; }
        public string GoalTargetName { get; set; }
        public string GoalTargetText { get; set; }
        public int StepCount { get; set; }
        public string FinishNPC { get; set; }
        public string AdvanceText { get; set; }
        public string CollectItemTemplate { get; set; }
        public int MaxCount { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public long RewardMoney { get; set; }
        public long RewardXP { get; set; }
        public long RewardCLXP { get; set; }
        public long RewardRP { get; set; }
        public long RewardBP { get; set; }
        public string OptionalRewardItemTemplates { get; set; }
        public string FinalRewardItemTemplates { get; set; }
        public string FinishText { get; set; }
        public string QuestDependency { get; set; }
        public string AllowedClasses { get; set; }
        public string ClassType { get; set; }
        public string XOffset { get; set; }
        public string YOffset { get; set; }
        public int ZoneID { get; set; }

        public virtual Region StartRegion { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
