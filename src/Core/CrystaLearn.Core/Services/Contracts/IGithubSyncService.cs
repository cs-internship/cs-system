using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services.Contracts;

public interface IGitHubSyncService
{
    Task<SyncResult> SyncAsync(CrystaProgramSyncModule module, CancellationToken cancellationToken = default);
}
