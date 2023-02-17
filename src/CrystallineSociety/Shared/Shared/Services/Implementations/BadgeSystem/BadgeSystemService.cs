using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations;
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
        public BadgeBundleDto? BadgeBundle { get; set; }

        public List<BadgeDto> Badges => BadgeBundle?.Badges ?? throw new Exception("BadgeSystem is not built yet.");
        public List<BadgeSystemValidationDto> Validations => BadgeBundle?.Validations ?? throw new Exception("BadgeSystem is not built yet.");
        public List<BadgeSystemValidationDto> Errors => Validations.Where(v => v.Level == BadgeSystemValidationLevel.Error).ToList();
        public List<BadgeSystemValidationDto> Validate()
        {
            var validations = new List<BadgeSystemValidationDto>();

            if (BadgeBundle is null)
                return validations;

            foreach (var badge in BadgeBundle.Badges)
            {
                foreach (var validator in BadgeValidators)
                {
                    var list = validator.ValidateBadge(badge, BadgeBundle);
                    validations.AddRange(list);
                }
            }

            foreach (var validation in BadgeValidators)
            {
                var list = validation.ValidateSystem(BadgeBundle);
                validations.AddRange(list);
            }

            return validations;
        }

        public void Build(BadgeBundleDto bundle)
        {
            BadgeBundle = bundle;

            var validations = Validate();
            bundle.Validations = validations;
        }
    }
}
