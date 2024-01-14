using AutoMapper;
using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Services.Contracts;

namespace CrystallineSociety.Server.Services.Implementations;

public partial class OrganizationService : IOrganizationService
{
    [AutoInject]
    protected AppDbContext AppDbContext = default!;

    [AutoInject] protected IMapper Mapper = default!;

    [AutoInject]
    protected IGitHubBadgeService GitHubBadgeService { get; set; }

    public async Task SyncBadgeSystemAsync(string educationProgramCode, CancellationToken cancellationToken)
    {
        var organization = await AppDbContext.Organizations.FirstOrDefaultAsync(ep => ep.Code == educationProgramCode, cancellationToken);

        if (organization == null)
        {
            throw new Exception($"Education program with code {educationProgramCode} not found!");
        }

        var oldBadges = AppDbContext.Badges.Where(b => b.Organization.Code == educationProgramCode);
        await AppDbContext.Badges.ExecuteDeleteAsync(cancellationToken);
        await AppDbContext.SaveChangesAsync(cancellationToken);

        var badgeDtos = await GitHubBadgeService.GetBadgesAsync(organization.BadgeSystemUrl);
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
                OrganizationId = organization.Id,
                Description = badgeDto.Description,
                SpecJsonSourceUrl = badgeDto.Url,
                SpecJson = badgeDto.SpecJson,
                Level = badgeDto.Level,
            });
        }

        await AppDbContext.Badges.AddRangeAsync(newBadges, cancellationToken);
        await AppDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Organization?>> GetAllOrganizationAsync(CancellationToken cancellationToken)
    {
        return await AppDbContext.Organizations.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsOrganizationExistAsync(string educationProgramCode, CancellationToken cancellationToken)
    {
        return await AppDbContext.Organizations.AnyAsync(ep => ep != null && ep.Code == educationProgramCode, cancellationToken);
    }

    public async Task AddOrganizationAsync(Organization? organization, CancellationToken cancellationToken)
    {
        await AppDbContext.Organizations.AddAsync(organization, cancellationToken);
        await AppDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Organization?> GetOrganizationAsync(string organizationCode,CancellationToken cancellationToken)
    {
        return await AppDbContext.Organizations.FirstOrDefaultAsync(o=>o.Code == organizationCode, cancellationToken);
    }
}
