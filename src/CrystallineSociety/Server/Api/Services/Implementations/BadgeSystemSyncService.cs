using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Api.Services.Implementations;

public partial class BadgeSystemSyncService : IBadgeSystemSyncService
{
    [AutoInject]
    protected AppDbContext AppDbContext = default;

    [AutoInject]
    protected IGitHubBadgeService GitHubBadgeService { get; set; }

    public async Task SyncBadgeSystemAsync(string educationProgramCode, CancellationToken cancellationToken)
    {
        var educationProgram = await AppDbContext.EducationPrograms.FirstOrDefaultAsync(ep => ep.Code == educationProgramCode, cancellationToken);

        if (educationProgram == null)
        {
            throw new Exception($"Education program with code {educationProgramCode} not found!");
        }

        var oldBadges = AppDbContext.Badges.Where(b => b.EducationProgram.Code == educationProgramCode);
        AppDbContext.RemoveRange(oldBadges);
        await AppDbContext.SaveChangesAsync(cancellationToken);

        var badgeDtos = await GitHubBadgeService.GetBadgesAsync(educationProgram.BadgeSystemUrl);
        var newBadges = new List<Badge>();

        foreach (var badgeDto in badgeDtos)
        {
            newBadges.Add(new Badge()
            {
                // ToDo: Handle exception in case of Code or Title being null
                Id = Guid.NewGuid(),
                Code = badgeDto.Code ?? throw new Exception("Code in badgeDto is null!"),
                Title = badgeDto.Code,
                EducationProgramId = educationProgram.Id,
                Description = badgeDto.Description
                //Prerequisites = badgeDto.
            });
        }

        AppDbContext.Set<Badge>().AddRange(newBadges);
        await AppDbContext.SaveChangesAsync(cancellationToken);
    }


}
