using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Services.Contracts;

public interface ICrystaProgramRepository
{
    Task<List<CrystaProgram>> GetCrystaProgramsAsync(CancellationToken cancellationToken);
    Task<CrystaProgram?> GetCrystaProgramByCodeAsync(string code, CancellationToken cancellationToken);
}
