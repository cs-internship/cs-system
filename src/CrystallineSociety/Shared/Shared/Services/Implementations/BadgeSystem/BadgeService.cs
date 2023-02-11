using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Utils;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem
{
    public class BadgeService : IBadgeService
    {
        private static JsonSerializerOptions BadgeOptions { get; set; } = new JsonSerializerOptions
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
            var badge = JsonSerializer.Deserialize<BadgeDto>(specJson, BadgeOptions);
            return badge;
        }

        public string SerializeBadge(BadgeDto badge)
        {
            return JsonSerializer.Serialize(badge, BadgeOptions);
        }
    }

    public class BadgeRequirementConverter : JsonConverter<BadgeRequirement>
    {
        public override BadgeRequirement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString();
            var requirements =
            (
                from part in text.Split("|")
                let badgeCount = part.Split("*")
                select new
                {
                    BadgeCode = badgeCount[0], BadgeCount = int.Parse(badgeCount.ElementAtOrDefault(1) ?? "1")
                }
            ).ToDictionary(o => o.BadgeCode, o => o.BadgeCount);

            return new BadgeRequirement(text, requirements);
        }

        public override void Write(Utf8JsonWriter writer, BadgeRequirement value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.RequirementStr);
        }
    }

    public class ActivityRequirementConverter : JsonConverter<ActivityRequirement>
    {
        public override ActivityRequirement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString();
            var requirements =
            (
                from part in text.Split("|")
                let badgeCount = part.Split("*")
                select new
                {
                    BadgeCode = badgeCount[0],
                    BadgeCount = int.Parse(badgeCount.ElementAtOrDefault(1) ?? "1")
                }
            ).ToDictionary(o => o.BadgeCode, o => o.BadgeCount);

            return new ActivityRequirement(text, requirements);
        }

        public override void Write(Utf8JsonWriter writer, ActivityRequirement value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.RequirementStr);
        }
    }
}
