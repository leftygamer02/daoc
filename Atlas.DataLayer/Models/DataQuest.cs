using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class DataQuest : DataObjectBase
    {
        public string Name { get; set; }
        public int StartType { get; set; }
        public string StartName { get; set; }
        public int StartRegionID { get; set; }
        public string AcceptText { get; set; }
        public string Description { get; set; }
        public string SourceName { get; set; }
        public string SourceText { get; set; }
        public string StepType { get; set; }
        public string StepText { get; set; }
        public string StepItemTemplates { get; set; }
        public string AdvanceText { get; set; }
        public string TargetName { get; set; }
        public string TargetText { get; set; }
        public string CollectItemTemplate { get; set; }
        public int MaxCount { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public string RewardMoney { get; set; }
        public string RewardXP { get; set; }
        public string RewardCLXP { get; set; }
        public string RewardRP { get; set; }
        public string RewardBP { get; set; }
        public string OptionalRewardItemTemplates { get; set; }
        public string FinalRewardItemTemplates { get; set; }
        public string FinishText { get; set; }
        public string QuestDependency { get; set; }
        public string AllowedClasses { get; set; }
        public string ClassType { get; set; }
    }
}
