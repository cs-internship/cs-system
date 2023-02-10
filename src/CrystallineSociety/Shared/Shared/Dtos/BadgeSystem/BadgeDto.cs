using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Dtos.BadgeSystem
{
    public class BadgeDto
    {
        public required string Code { get; set; }
        public string? Description { get; set; }
        public BadgeLevel Level { get; set; }
        public Dictionary<string, string> Info { get; set; }
        public List<AppraisalMethod>? AppraisalMethods { get; set; }
        
        public override string ToString()
        {
            return Code;
        }
    }

    public class AppraisalMethod
    {
        public string Title { get; set; }

        //[JsonPropertyName("badge-requirements")]
        //public List<string> BadgeRequirementNodes { get; set; } = new();
        //[JsonIgnore]
        public List<BadgeRequirement> BadgeRequirements { get; set; } = new();

        [JsonPropertyName("activity-requirements")]
        public List<string> ActivityRequirementNodes { get; set; } = new();
        [JsonIgnore]
        public List<ActivityRequirement> ActivityRequirements { get; set; } = new();
        public List<ApprovingStep> ApprovingSteps { get; set; } = new();
    }

    public class ApprovingStep
    {
        public int Step { get; set; }
        public required string Title { get; set; }

        public List<BadgeRequirement> ApproverRequiredBadges { get; set; } = new();
        public int RequiredApprovalCount { get; set; }

    }

    public class ActivityRequirement
    {
        public ActivityRequirement(string requirementStr, Dictionary<string, int> requirementOptions)
        {
            RequirementStr = requirementStr;
            RequirementOptions = requirementOptions;

        }

        public string RequirementStr { get; set; }
        [JsonIgnore]
        public Dictionary<string, int> RequirementOptions { get; set; }
    }

    public class BadgeRequirement
    {
        public BadgeRequirement(string requirementStr, Dictionary<string, int> requirementOptions)
        {
            RequirementStr = requirementStr;
            RequirementOptions = requirementOptions;
        }

        public string RequirementStr { get; set; }
        [JsonIgnore]
        public Dictionary<string, int> RequirementOptions { get; set; }
    }

    public enum BadgeLevel
    {
        Bronze, Silver, Gold
    }
}
