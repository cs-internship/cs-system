using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations
{
    public class RepeatedApprovingStepsValidator : BadgeSystemValidator
    {
        /// <summary>
        /// Checks for any duplicated titles in the approval steps of the badge bundle.
        /// </summary>
        /// <param name="badgeBundle"></param>
        /// <returns>A list of BadgeSystemValidationDto</returns>
        public override List<BadgeSystemValidationDto> ValidateBundle(BadgeBundleDto badgeBundle)
        {
            var repeatedTitlesInApprovingStepsForAllBadges = GetRepeatedTitlesInApprovingStepsForAllBadges(badgeBundle);

            return repeatedTitlesInApprovingStepsForAllBadges.Select(repeatedItem =>
                    BadgeSystemValidationDto.Error(
                        $"Repeated titles in approving steps of {repeatedItem.Code} badge found. Approving steps are not unique.",
                        $"Repeated titles in the approving steps are: {string.Join(", ", repeatedItem.Titles)}",
                        refBadge: repeatedItem.Code))
                .ToList();
        }

        private static IEnumerable<(string Code, List<string> Titles)> GetRepeatedTitlesInApprovingStepsForAllBadges(
            BadgeBundleDto badgeBundle)
        {
            List<(string Code, List<string> Titles)> repeatedBadgeTitles = new();

            foreach (var badge in badgeBundle.Badges)
            {
                if (badge.AppraisalMethods == null)
                    continue;

                foreach (var appraisalMethod in badge.AppraisalMethods)
                {
                    var repeatedItems = appraisalMethod.ApprovingSteps
                        .GroupBy(step => step.Title)
                        .Where(group => group.Count() > 1)
                        .Select(group => (Code: badge.Code, Titles: group.Select(step => step.Title).ToList()))
                        .ToList();

                    repeatedBadgeTitles.AddRange(repeatedItems);
                }
            }

            return repeatedBadgeTitles;
        }
    }
}