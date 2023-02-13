using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Contracts;

public interface IBadgeService
{
    BadgeDto ParseBadge(string specJson);
    string SerializeBadge(BadgeDto badge);
}