using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Client.Core.Controllers;

[Route("api/[controller]/[action]")]
public interface IOrganizationController : IAppController
{
    [HttpPost]
     Task SyncOrganizationBadges(OrganizationDto organization, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all education programs.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All education programs.</returns>
    [HttpGet]
    Task<List<OrganizationDto>> GetOrganizations(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an organization by its code.
    /// </summary>
    /// <param name="code">The code of the organization to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The organization with the specified code.</returns>
    [HttpGet]
     Task<OrganizationDto> GetOrganizationByCode(string code, CancellationToken cancellationToken = default);
}
