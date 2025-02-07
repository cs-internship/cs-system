using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Services.Contracts;

public interface ICrystaProgramSyncModuleRepository
{
    Task<List<CrystaProgramSyncModule>> GetSyncModulesAsync(CancellationToken cancellationToken);
}
