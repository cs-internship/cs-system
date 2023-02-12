using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Dtos.BadgeSystem.Validations
{
    public abstract class BadgeSystemValidator : IBadgeSystemValidator
    {
        public virtual List<BadgeSystemValidationDto> ValidateSystem(BadgeSystemDto badgeSystem)
        {
            return new List<BadgeSystemValidationDto>();
        }

        public virtual List<BadgeSystemValidationDto> ValidateBadge(BadgeDto badge, BadgeSystemDto badgeSystem)
        {
            return new List<BadgeSystemValidationDto>();
        }
    }

    public interface IBadgeSystemValidator
    {
        List<BadgeSystemValidationDto> ValidateSystem(BadgeSystemDto badgeSystem);
        List<BadgeSystemValidationDto> ValidateBadge(BadgeDto badge, BadgeSystemDto badgeSystem);
    }
}
