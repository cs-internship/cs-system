using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Api.Services.Implementations;

public partial class BadgeSystemSyncService : IBadgeSystemSyncService
{
    [AutoInject]
    protected AppDbContext AppDbContext = default;

    [AutoInject]
    protected IGitHubBadgeService GitHubBadgeService { get; set; }

    public async Task SyncBadgeSystemAsync(EducationProgram educationProgram, CancellationToken cancellationToken)
    {
        var oldBadges = AppDbContext.Badges.Where(b => b.EducationProgram.Code == educationProgram.Code);
        AppDbContext.RemoveRange(oldBadges);
        await AppDbContext.SaveChangesAsync(cancellationToken);

        var badgeDtos = await GitHubBadgeService.GetBadgesAsync(educationProgram.BadgeSystemUrl);
        var newBadges = new List<Badge>();

        foreach (var badgeDto in badgeDtos)
        {
            newBadges.Add(new Badge()
            {
                // ToDo: Handle exception in case of Code or Title being null
                Id = new Guid(),
                Code = badgeDto.Code ?? throw new Exception("Code in badgeDto is null!"),
                Title = badgeDto.Title ?? throw new Exception("Title in badgeDto is null!"),
                EducationProgram = educationProgram,
                Description = badgeDto.Description
                //Prerequisites = badgeDto.
            });
        }

        AppDbContext.AddRange(newBadges);
        await AppDbContext.SaveChangesAsync(cancellationToken);
    }


}
