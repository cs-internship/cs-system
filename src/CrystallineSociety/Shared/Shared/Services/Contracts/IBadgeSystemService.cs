using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Contracts;

public interface IBadgeSystemService
{
    void BuildBadgeSystem(BadgeSystemDto badgeSystem);
}