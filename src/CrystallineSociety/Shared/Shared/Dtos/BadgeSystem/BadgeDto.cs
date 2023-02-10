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
        public string Code { get; set; }
        public string Description { get; set; }
        public BadgeLevel Level { get; set; }
        public List<AppraisalMethod>? AppraisalMethods { get; set; }
        public override string ToString()
        {
            return Code;
        }
    }

    public class AppraisalMethod
    {
        public string Title { get; set; }
        [JsonIgnore]
        public List<BadgeRequirement> BadgeRequirements { get; set; } = new();
        [JsonIgnore]
        public List<ActivityRequirement> ActivityRequirements { get; set; } = new();
        public List<ApprovingStep> ApprovingSteps { get; set; } = new();
    }

    public class ApprovingStep
    {
        public int Step { get; set; }
        public required string Title { get; set; }
        [JsonIgnore]
        public List<BadgeRequirement> ApproverRequiredBadges { get; set; } = new();
        public int RequiredApprovalCount { get; set; }

    }

    public class ActivityRequirement
    {
        public ActivityRequirement(string requiredActivityStr, string requiredCount)
        {
            RequiredActivityStr = requiredActivityStr;
            RequiredCount = requiredCount;
        }

        public string RequiredActivityStr { get; set; }
        public string RequiredCount { get; set; }
    }

    public class BadgeRequirement
    {
        public BadgeRequirement(string requiredBadgeStr, string requiredCount)
        {
            RequiredBadgeStr = requiredBadgeStr;
            RequiredCount = requiredCount;
        }

        public string RequiredBadgeStr { get; set; }
        public string RequiredCount { get; set; }
    }

    public enum BadgeLevel
    {
        Bronze, Silver, Gold
    }
}
