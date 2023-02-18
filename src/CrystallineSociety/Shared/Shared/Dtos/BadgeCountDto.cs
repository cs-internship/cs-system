using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Dtos;

public class BadgeCountDto
{
    /// <summary>
    /// Example: doc-guru*2
    /// </summary>
    /// <param name="badgeCount">Example: doc-guru*2</param>
    public BadgeCountDto(string badgeCount)
    {
        var parts = badgeCount.Split('*');
        Badge = parts[0];
        Count = int.Parse(parts.Length > 1 ? parts[1] : "1");
    }

    public string Badge { get; set; }
    public int Count { get; set; }
}