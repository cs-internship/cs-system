using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Contracts;

public interface IBadgeUtilService
{
    BadgeDto ParseBadge(string specJson);
    string SerializeBadge(BadgeDto badge);
}