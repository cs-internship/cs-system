using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.BadgeSystem.Validations;
using CrystallineSociety.Shared.Json.Converters;
using CrystallineSociety.Shared.Utils;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem
{
    public partial class BadgeService : IBadgeService
    {
        [AutoInject]
        public IEnumerable<IBadgeSystemValidator> BadgeValidations { get; set; }

        private static JsonSerializerOptions BadgeSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = KebabCaseNamingPolicy.Instance,
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(),
                new BadgeRequirementConverter(),
                new ActivityRequirementConverter(),
            }
        };

        public BadgeDto? ParseBadge(string specJson)
        {
            var badge = JsonSerializer.Deserialize<BadgeDto>(specJson, BadgeSerializerOptions);
            return badge;
        }

        public string SerializeBadge(BadgeDto badge)
        {
            return JsonSerializer.Serialize(badge, BadgeSerializerOptions);
        }

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
