using CrystallineSociety.Shared.Dtos.BadgeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations
{
    public class RepeatedAppraisalMethodValidator: BadgeSystemValidator
    {
        public override List<BadgeSystemValidationDto> ValidateBundle(BadgeBundleDto badgeBundle)
        {
            var repeatedAppraisalMethods = GetRepeatedAppraisalMethods(badgeBundle);

            return repeatedAppraisalMethods.Select(repeatedItem =>
                               BadgeSystemValidationDto.Error($"Repeated appraisal method: {repeatedItem}",
                                                      $"Appraisal methods should be unique",
                                                                             refBadge: repeatedItem))
                .ToList();
        }

        private static IEnumerable<string> GetRepeatedAppraisalMethods(BadgeBundleDto badgeBundle)
        {
            var appraisalMethods = new List<string>();

            foreach (var badge in badgeBundle.Badges)
            {
                if (badge.AppraisalMethods == null)
                    continue;

                var repeatedItems = badge.AppraisalMethods
                    .GroupBy(x => x.Title)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                repeatedItems.ForEach(x => appraisalMethods.Add(x));
            }

            return appraisalMethods;
        }
    }
}
