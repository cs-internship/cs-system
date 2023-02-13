using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.BadgeSystem.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem
{
    public partial class BadgeSystemService : IBadgeSystemService
    {
        [AutoInject]
        public IEnumerable<IBadgeSystemValidator> BadgeValidators { get; set; }

        public List<BadgeSystemValidationDto> Validate(BadgeSystemDto badgeSystem)
        {
            var validations = new List<BadgeSystemValidationDto>();

            foreach (var badge in badgeSystem.Badges)
            {
                foreach (var validator in BadgeValidators)
                {
                    var list = validator.ValidateBadge(badge, badgeSystem);
                    validations.AddRange(list);
                }
            }

            foreach (var validation in BadgeValidators)
            {
                var list = validation.ValidateSystem(badgeSystem);
                validations.AddRange(list);
            }

            return validations;
        }

        public void Build(BadgeSystemDto badgeSystem)
        {
            var validations = Validate(badgeSystem);
            badgeSystem.Validations = validations;
        }
    }
}
