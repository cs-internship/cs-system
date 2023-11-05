using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Api.Services.Implementations;

public partial class EducationProgramService : IEducationProgramService
{
    [AutoInject]
    protected AppDbContext AppDbContext = default;

    [AutoInject] protected IMapper Mapper = default!;

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
        await AppDbContext.Badges.ExecuteDeleteAsync(cancellationToken);
        await AppDbContext.SaveChangesAsync(cancellationToken);

        var badgeDtos = await GitHubBadgeService.GetBadgesAsync(educationProgram.BadgeSystemUrl);
        var newBadges = new List<Badge>();

        //Mapper.Map(badgeDtos, newBadges);
        foreach (var badgeDto in badgeDtos)
        {
            newBadges.Add(new Badge()
            {
                // ToDo: Handle exception in case of Code or Title being null
                Id = Guid.NewGuid(),
                Code = badgeDto.Code ?? throw new Exception("Code in badgeDto is null!"),
                Title = badgeDto.Title ?? throw new Exception("Badge title is null!"),
                EducationProgramId = educationProgram.Id,
                Description = badgeDto.Description,
                Level = badgeDto.Level,
            });
        }

        await AppDbContext.Badges.AddRangeAsync(newBadges, cancellationToken);
        await AppDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<EducationProgram>> GetAllEducationProgramsAsync(CancellationToken cancellationToken)
    {
        return await AppDbContext.EducationPrograms.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsEducationExistAsync(string educationProgramCode, CancellationToken cancellationToken)
    {
        return await AppDbContext.EducationPrograms.AnyAsync(ep => ep.Code == educationProgramCode, cancellationToken);
    }

    public async Task AddEducationAsync(EducationProgram educationProgram, CancellationToken cancellationToken)
    {
        await AppDbContext.EducationPrograms.AddAsync(educationProgram, cancellationToken);
        await AppDbContext.SaveChangesAsync(cancellationToken);
    }
}
