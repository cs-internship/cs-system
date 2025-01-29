﻿using CrystallineSociety.Client.Core.Controllers;
using CrystallineSociety.Server.Services.Contracts;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Server.Controllers;

/// <summary>
/// Controller for managing the badge system.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public partial class BadgeSystemController : AppControllerBase, IBadgeSystemController
{
    /// <summary>
    /// Badge system factory for creating new badge systems.
    /// </summary>
    [AutoInject]
    public BadgeSystemFactory BadgeSystemFactory { get; set; }

    /// <summary>
    /// GitHub badge service for retrieving badges from GitHub.
    /// </summary>
    [AutoInject]
    public IGitHubBadgeService GitHubBadgeService { get; set; }

    /// <summary>
    /// Education program service for syncing badge systems with education programs.
    /// </summary>
    [AutoInject]
    public IOrganizationService OrganizationService { get; set; }

    /// <summary>
    /// Live badge system for managing badges.
    /// </summary>
    public IBadgeSystemService LiveBadgeSystemService => BadgeSystemFactory.Default();

    /// <summary>
    /// Retrieves the default badge bundle.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The default badge bundle.</returns>
    [HttpGet]
    public async Task<BadgeBundleDto> GetDefaultBadgeBundle(CancellationToken cancellationToken)
    {
        return LiveBadgeSystemService.BadgeBundle;
    }

    /// <summary>
    /// Retrieves a badge bundle from a GitHub repository.
    /// </summary>
    /// <param name="repositoryUrl">The URL of the GitHub repository.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The badge bundle from the GitHub repository.</returns>
    [HttpGet]
    public async Task<BadgeBundleDto> GetBadgeBundleFromGitHub(string repositoryUrl, CancellationToken cancellationToken)
    {
        var badges = await GitHubBadgeService.GetBadgesAsync(repositoryUrl);
        var bundle = new BadgeBundleDto(badges);
        var githubBadgeSystem = BadgeSystemFactory.CreateNew(bundle);

        return githubBadgeSystem.BadgeBundle;
    }

    /// <summary>
    /// Retrieves all badges.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All badges.</returns>
    [HttpGet]
    public async Task<IEnumerable<BadgeDto>> GetBadges(CancellationToken cancellationToken)
    {
        var badgeService = BadgeSystemFactory.Default();
        return await Task.FromResult(badgeService.Badges);
    }

    /// <summary>
    /// Retrieves all badge system validations.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All badge system validations.</returns>
    [HttpGet]
    public async Task<IEnumerable<BadgeSystemValidationDto>> GetBadgeValidations(CancellationToken cancellationToken)
    {
        var badgeService = BadgeSystemFactory.Default();
        return await Task.FromResult(badgeService.Validations);
    }

    /// <summary>
    /// Retrieves all earned badges for a given user.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>All earned badges for the user.</returns>
    [HttpGet]
    public async Task<List<BadgeCountDto>> GetEarnedBadgesAsync(string username)
    {
        return await LiveBadgeSystemService.GetEarnedBadgesAsync(username);
    }
}
