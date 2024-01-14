using CrystallineSociety.Shared.Dtos.BadgeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations
{
    public class RepeatedActivityRequirementValidator : BadgeSystemValidator
    {
        /// <summary>
        /// Checks for any duplicated titles in the Activity requirements of the badge bundle.
        /// </summary>
        /// <param name="badgeBundle"></param>
        /// <returns>A list of BadgeSystemValidationDto</returns>
        public override List<BadgeSystemValidationDto> ValidateBundle(BadgeBundleDto badgeBundle)
        {
            var repeatedTitlesInActivityRequirementsForAllBadges = GetRepeatedTitlesInActivityRequirementsForAllBadges(badgeBundle);

            return repeatedTitlesInActivityRequirementsForAllBadges.Select(repeatedItem =>
                               BadgeSystemValidationDto.Error(
                                   $"Repeated titles in activity requirements of {repeatedItem.Code} badge found. Activity requirements are not unique.",
                                   $"Repeated titles in the activity requirements are: {string.Join(", ", repeatedItem.Titles)}",
                                   refBadge: repeatedItem.Code))
                           .ToList();
        }

        private static IEnumerable<(string Code, List<string> Titles)> GetRepeatedTitlesInActivityRequirementsForAllBadges(
                       BadgeBundleDto badgeBundle)
        {
            List<(string Code, List<string> Titles)> repeatedBadgeTitles = new();

            foreach (var badge in badgeBundle.Badges)
            {
                if (badge.AppraisalMethods == null)
                    continue;

                foreach (var appraisalMethod in badge.AppraisalMethods)
                {
                    var repeatedItems = appraisalMethod.ActivityRequirements
                        .GroupBy(step => step.RequirementStr)
                        .Where(group => group.Count() > 1)
                        .Select(group => (Code: badge.Code, Titles: group.Select(step => step.RequirementStr).ToList()))
                        .ToList();

                    repeatedBadgeTitles.AddRange(repeatedItems);
                }
            }

            return repeatedBadgeTitles;
        }
    }
}
