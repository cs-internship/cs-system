using System.Text.Json;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Json.Converters;

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