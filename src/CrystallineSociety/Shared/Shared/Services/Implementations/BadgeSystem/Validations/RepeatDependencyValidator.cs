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
            var requiredBadgesCode = GetAllBadgeDependenciesCode(badgeBundle);

            var repeatedItems = requiredBadgesCode
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            return repeatedItems.Select(repeatedItem =>
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
        private static IEnumerable<string> GetAllBadgeDependenciesCode(BadgeBundleDto badgeBundle)
        {
            var requiredBadgesCode = new List<string>();

            foreach (var badge in badgeBundle.Badges)
            {
                if (badge.AppraisalMethods == null)
                    continue;

                foreach (var appraisalMethod in badge.AppraisalMethods)
                {
                    appraisalMethod.BadgeRequirements.ForEach(x => requiredBadgesCode.Add(x.RequirementStr));
                }
            }

            return requiredBadgesCode;
        }

    }
}
