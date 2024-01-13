using CrystallineSociety.Server.Services.Contracts;
using CrystallineSociety.Shared;
using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Server.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public partial class OrganizationController : AppControllerBase
{

    /// <summary>
    /// Education program service for syncing badge systems with education programs.
    /// </summary>
    [AutoInject]
    public IOrganizationService OrganizationService { get; set; }

    /// <summary>
    /// Syncs the badge system with an education program.
    /// </summary>
    /// <param name="organization">The education program to sync.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost]
    public async Task SyncOrganizationBadgesAsync(OrganizationDto organization, CancellationToken cancellationToken)
    {
        await OrganizationService.SyncBadgeSystemAsync(organization.Code, cancellationToken);
    }

    /// <summary>
    /// Retrieves all education programs.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All education programs.</returns>
    [HttpGet]
    public async Task<List<OrganizationDto>> GetOrganizationsAsync(CancellationToken cancellationToken)
    {
        return Mapper.Map<List<OrganizationDto>>(await OrganizationService.GetAllOrganizationAsync(cancellationToken));
    }

    /// <summary>
    /// Retrieves an organization by its code.
    /// </summary>
    /// <param name="code">The code of the organization to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The organization with the specified code.</returns>
    [HttpGet]
    public async Task<OrganizationDto> GetOrganizationByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return Mapper.Map<OrganizationDto>(await OrganizationService.GetOrganizationAsync(code, cancellationToken));
    }
}
