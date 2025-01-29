using System.Collections.Generic;

namespace CrystallineSociety.Shared.Dtos.BadgeSystem
{
    public class BadgeDto
    {
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public BadgeLevel Level { get; set; }
        public Dictionary<string, string>? Info { get; set; } = new();
        public List<AppraisalMethod>? AppraisalMethods { get; set; } = new();
        public string? SpecJson { get; set; }

        public string? Sha { get; set; }
        public string? Url { get; set; }
        public long? RepoId { get; set; }
        public override string ToString()
        {
            return Code ?? "";
        }
    }

    public class AppraisalMethod
    {
        public string Title { get; set; } = "Default";
        public List<BadgeRequirement> BadgeRequirements { get; set; } = new();
        public List<ActivityRequirement> ActivityRequirements { get; set; } = new();
        public List<ApprovingStep> ApprovingSteps { get; set; } = new();
    }

    public class ApprovingStep : IRequirement
    {
        public int Step { get; set; }
        public string Title { get; set; } = default!;
        public List<BadgeRequirement> ApproverRequiredBadges { get; set; } = new();
        public int RequiredApprovalCount { get; set; }
        public string RequirementStr { get; set; } = default!;
    }

    public class ActivityRequirement : IRequirement
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

    public class BadgeRequirement : IRequirement
    {
        public BadgeRequirement(string requirementStr, Dictionary<string, int> requirementOptions)
        {
            RequirementStr = requirementStr;
            RequirementOptions = requirementOptions;
        }

        public string RequirementStr { get; set; }
        [JsonIgnore]
        public Dictionary<string, int> RequirementOptions { get; set; } = new();
    }

    public enum BadgeLevel
    {
        Bronze, Silver, Gold
    }
}
public interface IRequirement
{
    string RequirementStr { get; }
}
