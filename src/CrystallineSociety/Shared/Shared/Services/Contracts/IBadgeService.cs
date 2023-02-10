using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Contracts;

public interface IBadgeService
{
    BadgeDto? Parse(string specJson);
}