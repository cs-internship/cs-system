using CrystallineSociety.Server.Api.Models.TodoItem;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.TodoItem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Server.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public partial class BadgeSystemController : AppControllerBase
{
    [AutoInject]
    public BadgeSystemFactory BadgeSystemFactory { get; set; }

    [AutoInject]
    public IGitHubBadgeService GitHubBadgeService { get; set; }

    public IBadgeSystemService BadgeSystemService => BadgeSystemFactory.Default();

    [HttpGet]
    public async Task<BadgeBundleDto> GetDefaultBadgeBundle(CancellationToken cancellationToken)
    {
        return BadgeSystemService.BadgeBundle;
    }

    [HttpGet]
    public async Task<BadgeBundleDto> GetBadgeBundleFromGitHub(string url, CancellationToken cancellationToken)
    {
        var badges = await GitHubBadgeService.GetBadgesAsync(url);
        return new BadgeBundleDto(badges);
    }

    [HttpGet]
    public IEnumerable<BadgeDto> GetBadges(CancellationToken cancellationToken)
    {
        var badgeService = BadgeSystemFactory.Default();
        return badgeService.Badges;
    }

    [HttpGet]
    public IEnumerable<BadgeSystemValidationDto> GetBadgeValidations(CancellationToken cancellationToken)
    {
        var badgeService = BadgeSystemFactory.Default();
        return badgeService.Validations;
    }

    [HttpGet]
    public async Task<List<BadgeCountDto>> GetEarnedBadgesAsync(string username)
    {
        return await BadgeSystemService.GetEarnedBadgesAsync(username);
    }
}
