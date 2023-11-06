using CrystallineSociety.Server.Api.Models;

namespace CrystallineSociety.Server.Api.Services.Contracts;


/// <summary>
/// Interface for managing education programs.
/// </summary>
public interface IOrganizationService
{
    /// <summary>
    /// Sync current badge system associated with the given education program with source (GitHub)
    /// </summary>
    /// <param name="educationProgramCode">The code of the education program to sync.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SyncBadgeSystemAsync(string educationProgramCode,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets all education programs.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<List<Organization>> GetAllOrganizationAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether an education program exists with the specified code.
    /// </summary>
    /// <param name="educationProgramCode">The code of the education program to check.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<bool> IsOrganizationExistAsync(string educationProgramCode,
        CancellationToken cancellationToken);

    /// <summary>
    /// Adds an education program.
    /// </summary>
    /// <param name="organization">The education program to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddOrganizationAsync(Organization organization,
        CancellationToken cancellationToken);

    Task<Organization> GetOrganizationAsync(string organizationCode,
        CancellationToken cancellationToken);
}
