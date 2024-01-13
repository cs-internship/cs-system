using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Services.Contracts;

public interface IBadgeSystemService
{
    void Build(BadgeBundleDto bundle);
    List<BadgeDto> Badges { get; }
    List<BadgeSystemValidationDto> Validations { get; }
    List<BadgeSystemValidationDto> Errors { get; }
    BadgeBundleDto BadgeBundle { get; set; }
    Task<List<BadgeCountDto>> GetEarnedBadgesAsync(string username);
    Task<List<LearnerDto>> GetLearnersHavingBadgeAsync(params BadgeCountDto[] requiredEarnedBadges);
}
