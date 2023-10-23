using CrystallineSociety.Server.Api.Models;

namespace CrystallineSociety.Server.Api.Services.Contracts;

public interface IEducationProgramService
{
    /// <summary>
    /// Sync current badge system associated with the given education program with source (GitHub)
    /// </summary>
    /// <param name="educationProgramCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SyncBadgeSystemAsync(string educationProgramCode,
        CancellationToken cancellationToken);

    Task<List<EducationProgram>> GetAllEducationProgramsAsync(CancellationToken cancellationToken);

    Task<bool> IsEducationExistAsync(string educationProgramCode,
        CancellationToken cancellationToken);

    Task AddEducationAsync(EducationProgram educationProgram,
        CancellationToken cancellationToken);
}
