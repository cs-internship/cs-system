using System.Text.Json;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Json.Converters;

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
                BadgeCode = badgeCount[0],
                BadgeCount = int.Parse(badgeCount.ElementAtOrDefault(1) ?? "1")
            }
        ).ToDictionary(o => o.BadgeCode, o => o.BadgeCount);

        return new BadgeRequirement(text, requirements);
    }

    public override void Write(Utf8JsonWriter writer, BadgeRequirement value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.RequirementStr);
    }
}