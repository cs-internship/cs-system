using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations
{
    public abstract class BadgeSystemValidator : IBadgeSystemValidator
    {
        public virtual List<BadgeSystemValidationDto> ValidateBundle(BadgeBundleDto badgeBundle)
        {
            return new List<BadgeSystemValidationDto>();
        }

        public virtual List<BadgeSystemValidationDto> ValidateBadge(BadgeDto badge, BadgeBundleDto containingBundle)
        {
            return new List<BadgeSystemValidationDto>();
        }
    }

    public interface IBadgeSystemValidator
    {
        List<BadgeSystemValidationDto> ValidateBundle(BadgeBundleDto badgeBundle);
        List<BadgeSystemValidationDto> ValidateBadge(BadgeDto badge, BadgeBundleDto containingBundle);
    }
}
