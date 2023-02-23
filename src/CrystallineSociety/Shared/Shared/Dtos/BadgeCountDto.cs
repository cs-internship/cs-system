using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Dtos;

public class BadgeCountDto
{
    public required BadgeDto Badge { get; set; }
    public required int Count { get; set; }
}