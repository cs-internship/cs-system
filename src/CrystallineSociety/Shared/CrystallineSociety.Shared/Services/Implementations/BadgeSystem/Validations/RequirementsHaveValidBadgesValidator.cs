﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations
{
    public class RequirementsHaveValidBadgesValidator : BadgeSystemValidator
    {
        public override List<BadgeSystemValidationDto> ValidateBadge(BadgeDto badge, BadgeBundleDto containingBundle)
        {
            var list = new List<BadgeSystemValidationDto>();

            foreach (var appraisalMethod in badge.AppraisalMethods)
            {
                foreach (var badgeRequirement in appraisalMethod.BadgeRequirements)
                {
                    foreach (var badgeStr in badgeRequirement.RequirementOptions.Keys)
                    {
                        if (!containingBundle.BadgeExists(badgeStr))
                            list.Add(
                                BadgeSystemValidationDto.Error(
                                    $"Badge does not exist: '{badgeStr}'",
                                    description: $"In the badge requirements of '{badge.Code}', this is invalid: '{badgeRequirement.RequirementStr}'. No badge found with code: {badgeStr} ",
                                    refBadge: badge.Code)
                            );
                    }
                }

                foreach (var badgeRequirement in appraisalMethod.ApprovingSteps.SelectMany(o => o.ApproverRequiredBadges))
                {
                    foreach (var badgeStr in badgeRequirement.RequirementOptions.Keys)
                    {
                        if (!containingBundle.BadgeExists(badgeStr))
                            list.Add(
                                BadgeSystemValidationDto.Error(
                                    $"Badge does not exist: '{badgeStr}'",
                                    description: $"In the approval badge requirements of '{badge.Code}', this is invalid: '{badgeRequirement.RequirementStr}'. No badge found with code: '{badgeStr}' ",
                                    refBadge: badge.Code)
                            );
                    }
                }
            }

            return list;
        }
    }
}
