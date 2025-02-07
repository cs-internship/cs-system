using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services.Contracts;

public interface ICrystaProgramSyncService
{
    Task<SyncResult> SyncAzureBoardAsync(CrystaProgramSyncModule module);
    Task SyncAsync(CrystaProgramSyncModule module);
}
