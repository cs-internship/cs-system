using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations
{
    public class RepeatDependencyValidator : BadgeSystemValidator
    {
        public override List<BadgeSystemValidationDto> ValidateBundle(BadgeBundleDto badgeBundle)
        {
            var repeatedRequiredBadgesCode = GetAllBadgeRepeatedDependenciesCode(badgeBundle);

            return repeatedRequiredBadgesCode.Select(repeatedItem =>
                    BadgeSystemValidationDto.Error($"Repeating dependency badge: {repeatedItem}",
                        $"Badge names should be unique",
                        refBadge: repeatedItem))
                .ToList();
        }

        /// <summary>
        /// Returns all the required badges code.
        /// </summary>
        /// <param name="badgeBundle">The bundle of badges to iterate through.</param>
        /// <returns>An enumerable list of the required badges code.</returns>
        private static IEnumerable<string> GetAllBadgeRepeatedDependenciesCode(BadgeBundleDto badgeBundle)
        {
            var requiredBadgesCode = new List<string>();

            foreach (var badge in badgeBundle.Badges)
            {
                if (badge.AppraisalMethods == null)
                    continue;

                foreach (var t in badge.AppraisalMethods)
                {
                    var badgesForEach = t.BadgeRequirements;

                    var repeatedItems = badgesForEach
                        .GroupBy(x => x.RequirementStr)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();

                    repeatedItems.ForEach(x => requiredBadgesCode.Add(x));
                }
            }

            return requiredBadgesCode;
        }
    }
}