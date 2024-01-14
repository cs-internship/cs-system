using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Contracts;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Client.Core.Controllers;

[Route("api/[controller]/[action]")]
public interface IBadgeSystemController : IAppController
{
    /// <summary>
    /// Retrieves the default badge bundle.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The default badge bundle.</returns>
    [HttpGet]
    Task<BadgeBundleDto> GetDefaultBadgeBundle(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a badge bundle from a GitHub repository.
    /// </summary>
    /// <param name="repositoryUrl">The URL of the GitHub repository.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The badge bundle from the GitHub repository.</returns>
    [HttpGet]
    Task<BadgeBundleDto> GetBadgeBundleFromGitHub(string repositoryUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all badges.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All badges.</returns>
    [HttpGet]
    Task<IEnumerable<BadgeDto>> GetBadges(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all badge system validations.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All badge system validations.</returns>
    [HttpGet]
    Task<IEnumerable<BadgeSystemValidationDto>> GetBadgeValidations(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all earned badges for a given user.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>All earned badges for the user.</returns>
    [HttpGet]
    Task<List<BadgeCountDto>> GetEarnedBadgesAsync(string username);
}
