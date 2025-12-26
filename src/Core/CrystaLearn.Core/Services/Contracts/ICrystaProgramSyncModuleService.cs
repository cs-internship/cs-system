using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Services.Contracts;

public interface ICrystaProgramSyncModuleService
{
    Task<List<CrystaProgramSyncModule>> GetSyncModulesAsync(CancellationToken cancellationToken);

    // Save or update a sync module (persist SyncInfo changes)
    Task UpdateSyncModuleAsync(CrystaProgramSyncModule module, CancellationToken cancellationToken = default);
}
