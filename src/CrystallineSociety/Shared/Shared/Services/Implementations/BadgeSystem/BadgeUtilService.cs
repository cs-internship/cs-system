using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Json.Converters;
using CrystallineSociety.Shared.Utils;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem
{
    public partial class BadgeUtilService : IBadgeUtilService
    {
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

        public BadgeDto ParseBadge(string specJson)
        {
            var badge = JsonSerializer.Deserialize<BadgeDto>(specJson, BadgeSerializerOptions);


            if (badge is null)
                throw new InvalidOperationException("Can not create badge from spec.");

            badge.Title ??= badge.Code;

           return badge;
        }

        public string SerializeBadge(BadgeDto badge)
        {
            return JsonSerializer.Serialize(badge, BadgeSerializerOptions);
        }

        
    }
}
