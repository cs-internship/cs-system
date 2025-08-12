using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services.Contracts;

public interface IAzureBoardSyncService
{
    Task<SyncResult> SyncAsync(CrystaProgramSyncModule module);

}
