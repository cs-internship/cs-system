using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services.Sync;

public partial class CrystaProgramSyncService : ICrystaProgramSyncService
{
    private IAzureBoardSyncService AzureBoardSyncService { get; set; } = default!;

    public async Task SyncAsync(CrystaProgramSyncModule module)
    {
        switch (module.ModuleType)
        {
            case SyncModuleType.AzureBoard:
                await AzureBoardSyncService.SyncAsync(module);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
