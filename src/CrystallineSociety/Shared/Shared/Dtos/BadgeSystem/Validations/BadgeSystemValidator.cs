﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Dtos.BadgeSystem.Validations
{
    public abstract class BadgeSystemValidator : IBadgeSystemValidator
    {
        public virtual List<BadgeSystemValidationDto> ValidateSystem(BadgeBundleDto badgeBundle)
        {
            return new List<BadgeSystemValidationDto>();
        }

        public virtual List<BadgeSystemValidationDto> ValidateBadge(BadgeDto badge, BadgeBundleDto badgeBundle)
        {
            return new List<BadgeSystemValidationDto>();
        }
    }

    public interface IBadgeSystemValidator
    {
        List<BadgeSystemValidationDto> ValidateSystem(BadgeBundleDto badgeBundle);
        List<BadgeSystemValidationDto> ValidateBadge(BadgeDto badge, BadgeBundleDto badgeBundle);
    }
}
