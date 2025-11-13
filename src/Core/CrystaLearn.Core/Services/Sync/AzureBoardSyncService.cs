using CrystaLearn.Core.Extensions;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.AzureBoard;
using CrystaLearn.Core.Services.Contracts;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace CrystaLearn.Core.Services.Sync;

public partial class AzureBoardSyncService : IAzureBoardSyncService
{
    [AutoInject] private IAzureBoardService AzureBoardService { get; set; } = default!;
    [AutoInject] private IConfiguration Configuration { get; set; } = default!;
    [AutoInject] private ICrystaTaskRepository CrystaTaskRepository { get; set; } = default!;

    public async Task<SyncResult> SyncAsync(CrystaProgramSyncModule module)
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
                 [Changed Date] Asc
             """;

        await foreach (var workItems in AzureBoardService.EnumerateWorkItemsQueryAsync(config, query, top: 200))
        {
            var tasks = workItems
                        .Select(ToCrystaTask)
                        .ToList();
            
            await SyncWorkItemsAsync(tasks);
            await SyncUpdatesAsync(tasks);
            await SyncCommentsAsync(tasks);
            await SyncRevisionsAsync(tasks);
        }

        return new SyncResult
        {
            AddCount = 0,
            UpdateCount = 0,
            SameCount = 0
        };
    }

    private async Task<SyncResult> SyncUpdatesAsync(List<CrystaTask> tasks)
    {
        return new SyncResult
        {
            AddCount = 0,
            UpdateCount = 0,
            SameCount = 0
        };
    }

    private async Task<SyncResult> SyncCommentsAsync(List<CrystaTask> tasks)
    {
        return new SyncResult
        {
            AddCount = 0,
            UpdateCount = 0,
            SameCount = 0
        };
    }

    private async Task<SyncResult> SyncRevisionsAsync(List<CrystaTask> tasks)
    {
        return new SyncResult
        {
            AddCount = 0,
            UpdateCount = 0,
            SameCount = 0
        };
    }

    private async Task<SyncResult> SyncWorkItemsAsync(List<CrystaTask> tasks)
    {
        var boardSyncItems = tasks
            .Select(task => new SyncItem 
            { 
                SyncInfo = task.WorkItemSyncInfo 
            })
            .ToList();

        var ids = boardSyncItems.Select(wi => wi.SyncInfo.SyncId).ToList();

        var existingWorkItemSyncItems = await CrystaTaskRepository.GetWorkItemSyncItemsAsync(ids);

        var toAddList = boardSyncItems
                        .Where(board => existingWorkItemSyncItems.All(existing => board.SyncInfo.SyncId != existing.SyncInfo.SyncId))
                        .ToList();

        var remainedList = boardSyncItems.Except(toAddList).ToList();

        var toUpdateList = remainedList
                           .Where(board =>
                               existingWorkItemSyncItems.Any(existing => board.SyncInfo.SyncId == existing.SyncInfo.SyncId && board.SyncInfo.SyncHash != existing.SyncInfo.SyncHash))
                           .ToList();

        var sameList = remainedList.Except(toUpdateList).ToList();

        // ToDo: do the real sync

        return new SyncResult
        {
            AddCount = toAddList.Count,
            UpdateCount = toUpdateList.Count,
            SameCount = sameList.Count
        };
    }

    private CrystaTask ToCrystaTask(WorkItem workItem)
    {
        var json = JsonSerializer.Serialize(workItem.Fields);
        var hash = json.Sha();

        var syncInfo = new SyncInfo
        {
            SyncId = workItem.Id.ToString() ?? throw new Exception("Work Item with no ID from board is not valid."),
            SyncHash = hash,
            LastSyncDateTime = DateTimeOffset.Now,
            LastSyncOffset = workItem.Id.ToString()
        };

        var status = workItem.Fields["System.State"]?.ToString() switch
        {
            "New" or "To Do" => CrystaTaskStatus.New,
            "Approved" or "In Progress" or "Committed"
                => CrystaTaskStatus.InProgress,
            "Done" or "Closed" 
                => CrystaTaskStatus.Done,
            "Canceled" or "Removed" 
                => CrystaTaskStatus.Canceled,
            _ 
                => throw new Exception($"Invalid status for mapping: {workItem.Fields["System.State"]?.ToString()}")
        };

        var title = workItem.Fields["System.Title"]?.ToString();

        var description = workItem.Fields.GetValueOrDefault("System.Description")?.ToString();

        var taskCreateDateTime = workItem.Fields["System.CreatedDate"]?.ToString() switch
        {
            string s => DateTimeOffset.Parse(s),
            _ => throw new Exception("Invalid date")
        };

        //var taskDoneDateTime = workItem.Fields["System.FinishedDate"]?.ToString() switch
        //{
        //    string s => DateTimeOffset.Parse(s),
        //    _ => throw new Exception("Invalid date")
        //};

        //var taskAssignDateTime = workItem.Fields["System.AssignedDate"]?.ToString() switch
        //{
        //    string s => DateTimeOffset.Parse(s),
        //    _ => throw new Exception("Invalid date")
        //};

        var assignedToText = workItem.Fields.GetValueOrDefault("System.AssignedTo") switch {
            IdentityRef identityRef => $"{identityRef.DisplayName} ({identityRef.UniqueName})",
            _ => ""
        };
        
        var task = new CrystaTask
        {
            ProviderTaskId = workItem.Id ?? 0,
            Title = title,
            //Description = description,
            //DescriptionHtml = description,
            Status = status,
            TaskCreateDateTime = taskCreateDateTime,
            ProviderTaskUrl = workItem.Url,
            WorkItemSyncInfo = syncInfo,
            AssignedToText = assignedToText,
        };


        return task;
    }
}
