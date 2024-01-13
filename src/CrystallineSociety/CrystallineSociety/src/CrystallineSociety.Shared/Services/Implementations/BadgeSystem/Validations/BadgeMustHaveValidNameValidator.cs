using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations
{
    public class BadgeMustHaveValidNameValidator : BadgeSystemValidator
    {
        private static readonly string[] InvalidCharacters = { "*", "%", "+", "/", "?", "|", "!", ",", "^" };

        public override List<BadgeSystemValidationDto> ValidateBadge(BadgeDto badge, BadgeBundleDto containingBundle)
        {
            var validations = new List<BadgeSystemValidationDto>();
            if (InvalidCharacters.Any(c => badge.Code?.Contains(c) ?? false))
            {
                validations.Add(BadgeSystemValidationDto.Error(
                    $"Invalid badge name: {badge.Code}",
                    $"Badge names should contain these characters: {string.Join(",", InvalidCharacters)}",
                    refBadge: badge.Code
                    ));
            }

            return validations;
        }
    }
}
