using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Server.Api.Services.Crysta.Contracts;

public interface ICrystaProgramRepository
{
    Task<List<CrystaProgram>> GetCrystaProgramsAsync(CancellationToken cancellationToken);
    Task<CrystaProgram?> GetCrystaProgramByCodeAsync(string code, CancellationToken cancellationToken);
}
