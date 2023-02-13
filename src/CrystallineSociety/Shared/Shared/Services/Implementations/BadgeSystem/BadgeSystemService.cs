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
        public IEnumerable<IBadgeSystemValidator> BadgeValidations { get; set; }

        public List<BadgeSystemValidationDto> ValidateBadgeSystem(BadgeSystemDto badgeSystem)
        {
            var logs = new List<BadgeSystemValidationDto>();

            foreach (var badge in badgeSystem.Badges)
            {
                foreach (var validation in BadgeValidations)
                {
                    var list = validation.ValidateBadge(badge, badgeSystem);
                    logs.AddRange(list);
                }
            }

            foreach (var validation in BadgeValidations)
            {
                var list = validation.ValidateSystem(badgeSystem);
                logs.AddRange(list);
            }

            return logs;
        }

        public void BuildBadgeSystem(BadgeSystemDto badgeSystem)
        {
            var logs = ValidateBadgeSystem(badgeSystem);
            badgeSystem.Validations = logs;
        }
    }
}
