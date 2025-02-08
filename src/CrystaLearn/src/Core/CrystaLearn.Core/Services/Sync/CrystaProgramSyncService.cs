using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.AzureBoard;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services.Sync;

public partial class CrystaProgramSyncService : ICrystaProgramSyncService
{
    [AutoInject] private IAzureBoardService AzureBoardService { get; set; } = default!;
    [AutoInject] private IConfiguration Configuration { get; set; } = default!;

    public async Task SyncAsync(CrystaProgramSyncModule module)
    {
        switch (module.ModuleType)
        {
            case SyncModuleType.AzureBoard:
                await SyncAzureBoardAsync(module);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task<SyncResult> SyncAzureBoardAsync(CrystaProgramSyncModule module)
    {
        if (module.ModuleType != SyncModuleType.AzureBoard)
        {
            throw new InvalidOperationException("Invalid module type");
        }

        if (string.IsNullOrWhiteSpace(module.SyncConfig))
        {
            throw new ArgumentNullException(nameof(module.CrystaProgram));
        }

        var config = JsonSerializer.Deserialize<AzureBoardSyncConfig>(module.SyncConfig)
            ?? throw new Exception("Invalid sync config");

        var project = config.Project ?? throw new Exception("No project provided");
        var lastSyncDateTime = 
            module.SyncInfo.LastSyncDateTime
            ?? config.WorkItemChangedFromDateTime
            ?? throw new Exception("No WorkItemChangedFromDateTime date provided");

        if (!int.TryParse(module.SyncInfo.LastSyncOffset, out var lastWorkItemId))
        {
            lastWorkItemId = 0;
        }

        var query =
            $"""
             Select 
                 [Id] 
             From 
                 WorkItems 
             Where 
                 [Changed Date] >= '{lastSyncDateTime:yyyy/MM/dd}'
                 And [System.TeamProject] = '{project}' 
                 And [System.State] <> 'Closed' 
             Order By 
                 [State] Asc, 
                 [Changed Date] Desc
             """;
        
        var workItems = await AzureBoardService.GetWorkItemsRawQueryAsync(config, query);

        return new SyncResult
        {
            AddCount = 0,
            UpdateCount = 0,
            SameCount = 0
        };
    }


}
