using CrystallineSociety.Server.Api.Models;

namespace CrystallineSociety.Server.Api.Services.Contracts;


/// <summary>
/// Interface for managing education programs.
/// </summary>
public interface IEducationProgramService
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
    Task<List<EducationProgram>> GetAllEducationProgramsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether an education program exists with the specified code.
    /// </summary>
    /// <param name="educationProgramCode">The code of the education program to check.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<bool> IsEducationExistAsync(string educationProgramCode,
        CancellationToken cancellationToken);

    /// <summary>
    /// Adds an education program.
    /// </summary>
    /// <param name="educationProgram">The education program to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddEducationAsync(EducationProgram educationProgram,
        CancellationToken cancellationToken);
}
