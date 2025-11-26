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
            throw new ArgumentNullException(nameof(module.SyncConfig));
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

        var totalResult = new SyncResult { AddCount = 0, UpdateCount = 0, SameCount = 0, DeleteCount = 0 };

        // Collect all active work item IDs from Azure Board
        var activeWorkItemIds = new HashSet<string>();

        await foreach (var workItems in AzureBoardService.EnumerateWorkItemsQueryAsync(config, query))
        {
            var tasks = workItems
                        .Select(w => ToCrystaTask(w,config))
                        .ToList();

            // Collect active work item IDs
            foreach (var task in tasks)
            {
                if (!string.IsNullOrEmpty(task.WorkItemSyncInfo.SyncId))
                {
                    activeWorkItemIds.Add(task.WorkItemSyncInfo.SyncId);
                }
            }

            var workItemResult = await SyncWorkItemsAsync(tasks);
            var updatesResult = await SyncUpdatesAsync(config, tasks);
            var commentsResult = await SyncCommentsAsync(config, tasks);
            var revisionsResult = await SyncRevisionsAsync(config, tasks);

            // Aggregate results
            totalResult.AddCount += workItemResult.AddCount + updatesResult.AddCount + commentsResult.AddCount + revisionsResult.AddCount;
            totalResult.UpdateCount += workItemResult.UpdateCount + updatesResult.UpdateCount + commentsResult.UpdateCount + revisionsResult.UpdateCount;
            totalResult.SameCount += workItemResult.SameCount + updatesResult.SameCount + commentsResult.SameCount + revisionsResult.SameCount;
        }

        // Handle deletions - find tasks in our DB that are no longer in Azure Board
        var allLocalWorkItemIds = await CrystaTaskRepository.GetAllWorkItemSyncIdsAsync(project);
        var deletedWorkItemIds = allLocalWorkItemIds.Except(activeWorkItemIds).ToList();

        if (deletedWorkItemIds.Count > 0)
        {
            var deleteCount = await CrystaTaskRepository.MarkCrystaTasksAsDeletedAsync(deletedWorkItemIds);
            totalResult.DeleteCount += deleteCount;
        }

        // Update module sync info
        module.SyncInfo.LastSyncDateTime = DateTimeOffset.Now;
        module.SyncInfo.SyncStatus = SyncStatus.Success;

        return totalResult;
    }

    /// <summary>
    /// Sync CrystaTaskUpdate
    /// </summary>
    /// <param name="config"></param>
    /// <param name="tasks"></param>
    /// <returns></returns>
    private async Task<SyncResult> SyncUpdatesAsync(AzureBoardSyncConfig config, List<CrystaTask> tasks)
    {
        var allUpdates = new List<CrystaTaskUpdate>();

        foreach (var task in tasks)
        {
            if (string.IsNullOrEmpty(task.ProviderTaskId) || !int.TryParse(task.ProviderTaskId, out var workItemId))
            {
                continue;
            }

            var azureUpdates = await AzureBoardService.GetUpdatesAsync(config, workItemId);

            foreach (var update in azureUpdates)
            {
                var crystaUpdate = ToCrystaTaskUpdate(update, task.Id, workItemId);
                if (crystaUpdate != null)
                {
                    allUpdates.Add(crystaUpdate);
                }
            }
        }

        if (allUpdates.Count == 0)
        {
            return new SyncResult { AddCount = 0, UpdateCount = 0, SameCount = 0 };
        }

        var updateSyncItems = allUpdates
            .Select(u => new SyncItem
            {
                Id = u.Id,
                SyncInfo = u.SyncInfo ?? new SyncInfo()
            })
            .ToList();

        var ids = updateSyncItems.Select(u => u.SyncInfo.SyncId ?? "").Where(id => !string.IsNullOrEmpty(id)).ToList();
        var existingUpdateSyncItems = await CrystaTaskRepository.GetUpdatesSyncItemsAsync(ids);

        var toAddList = updateSyncItems
                        .Where(board => existingUpdateSyncItems.All(existing => board.SyncInfo.SyncId != existing.SyncInfo.SyncId))
                        .ToList();

        var remainedList = updateSyncItems.Except(toAddList).ToList();

        var toUpdateList = remainedList
                           .Where(board =>
                               existingUpdateSyncItems.Any(existing => board.SyncInfo.SyncId == existing.SyncInfo.SyncId && board.SyncInfo.ContentHash != existing.SyncInfo.ContentHash))
                           .ToList();

        var sameList = remainedList.Except(toUpdateList).ToList();

        var toAddOrUpdate = allUpdates
            .Where(u => toAddList.Any(s => s.Id == u.Id) || toUpdateList.Any(s => s.Id == u.Id))
            .ToList();

        await CrystaTaskRepository.AddOrUpdateCrystaTaskUpdatesAsync(toAddOrUpdate);

        return new SyncResult
        {
            AddCount = toAddList.Count,
            UpdateCount = toUpdateList.Count,
            SameCount = sameList.Count
        };
    }

    /// <summary>
    /// Sync CrystaTaskComment
    /// </summary>
    /// <param name="config"></param>
    /// <param name="tasks"></param>
    /// <returns></returns>
    private async Task<SyncResult> SyncCommentsAsync(AzureBoardSyncConfig config, List<CrystaTask> tasks)
    {
        var allComments = new List<CrystaTaskComment>();

        foreach (var task in tasks)
        {
            if (string.IsNullOrEmpty(task.ProviderTaskId) || !int.TryParse(task.ProviderTaskId, out var workItemId))
            {
                continue;
            }

            var azureComments = await AzureBoardService.GetCommentsAsync(config, workItemId);

            foreach (var comment in azureComments)
            {
                var crystaComment = ToCrystaTaskComment(comment, task.Id, workItemId);
                if (crystaComment != null)
                {
                    allComments.Add(crystaComment);
                }
            }
        }

        if (allComments.Count == 0)
        {
            return new SyncResult { AddCount = 0, UpdateCount = 0, SameCount = 0 };
        }

        var commentSyncItems = allComments
            .Select(c => new SyncItem
            {
                Id = c.Id,
                SyncInfo = c.SyncInfo ?? new SyncInfo()
            })
            .ToList();

        var ids = commentSyncItems.Select(c => c.SyncInfo.SyncId ?? "").Where(id => !string.IsNullOrEmpty(id)).ToList();
        var existingCommentSyncItems = await CrystaTaskRepository.GetCommentsSyncItemsAsync(ids);

        var toAddList = commentSyncItems
                        .Where(board => existingCommentSyncItems.All(existing => board.SyncInfo.SyncId != existing.SyncInfo.SyncId))
                        .ToList();

        var remainedList = commentSyncItems.Except(toAddList).ToList();

        var toUpdateList = remainedList
                           .Where(board =>
                               existingCommentSyncItems.Any(existing => board.SyncInfo.SyncId == existing.SyncInfo.SyncId && board.SyncInfo.ContentHash != existing.SyncInfo.ContentHash))
                           .ToList();

        var sameList = remainedList.Except(toUpdateList).ToList();

        var toAddOrUpdate = allComments
            .Where(c => toAddList.Any(s => s.Id == c.Id) || toUpdateList.Any(s => s.Id == c.Id))
            .ToList();

        await CrystaTaskRepository.AddOrUpdateCrystaTaskCommentsAsync(toAddOrUpdate);

        return new SyncResult
        {
            AddCount = toAddList.Count,
            UpdateCount = toUpdateList.Count,
            SameCount = sameList.Count
        };
    }


    /// <summary>
    /// Sync CrystaTaskRevision
    /// </summary>
    /// <param name="config"></param>
    /// <param name="tasks"></param>
    /// <returns></returns>
    private async Task<SyncResult> SyncRevisionsAsync(AzureBoardSyncConfig config, List<CrystaTask> tasks)
    {
        var allRevisions = new List<CrystaTaskRevision>();

        foreach (var task in tasks)
        {
            if (string.IsNullOrEmpty(task.ProviderTaskId) || !int.TryParse(task.ProviderTaskId, out var workItemId))
            {
                continue;
            }

            var azureRevisions = await AzureBoardService.GetRevisionsAsync(config, workItemId);

            foreach (var revision in azureRevisions)
            {
                var crystaRevision = ToCrystaTaskRevision(revision, task.Id, workItemId);
                if (crystaRevision != null)
                {
                    allRevisions.Add(crystaRevision);
                }
            }
        }

        if (allRevisions.Count == 0)
        {
            return new SyncResult { AddCount = 0, UpdateCount = 0, SameCount = 0 };
        }

        var revisionSyncItems = allRevisions
            .Select(r => new SyncItem
            {
                Id = r.Id,
                SyncInfo = new SyncInfo
                {
                    SyncId = $"{r.ProviderTaskId}-{r.Revision}",
                    ContentHash = r.RawJson?.Sha() ?? ""
                }
            })
            .ToList();

        var ids = revisionSyncItems.Select(r => r.SyncInfo.SyncId ?? "").Where(id => !string.IsNullOrEmpty(id)).ToList();
        var existingRevisionSyncItems = await CrystaTaskRepository.GetRevisionsSyncItemsAsync(ids);

        var toAddList = revisionSyncItems
                        .Where(board => existingRevisionSyncItems.All(existing => board.SyncInfo.SyncId != existing.SyncInfo.SyncId))
                        .ToList();

        var remainedList = revisionSyncItems.Except(toAddList).ToList();

        var toUpdateList = remainedList
                           .Where(board =>
                               existingRevisionSyncItems.Any(existing => board.SyncInfo.SyncId == existing.SyncInfo.SyncId && board.SyncInfo.ContentHash != existing.SyncInfo.ContentHash))
                           .ToList();

        var sameList = remainedList.Except(toUpdateList).ToList();

        var toAddOrUpdate = allRevisions
            .Where(r => toAddList.Any(s => s.Id == r.Id) || toUpdateList.Any(s => s.Id == r.Id))
            .ToList();

        await CrystaTaskRepository.AddOrUpdateCrystaTaskRevisionsAsync(toAddOrUpdate);

        return new SyncResult
        {
            AddCount = toAddList.Count,
            UpdateCount = toUpdateList.Count,
            SameCount = sameList.Count
        };
    }

    /// <summary>
    /// Sync CrystaTask
    /// </summary>
    /// <param name="tasks"></param>
    /// <returns></returns>
    private async Task<SyncResult> SyncWorkItemsAsync(List<CrystaTask> tasks)
    {
        var boardSyncItems = tasks
            .Select(task => new SyncItem
            {
                Id = task.Id,
                SyncInfo = task.WorkItemSyncInfo
            })
            .ToList();

        var ids = boardSyncItems.Select(wi => wi.SyncInfo.SyncId ?? "").Where(id => !string.IsNullOrEmpty(id)).ToList();

        var existingWorkItemSyncItems = await CrystaTaskRepository.GetWorkItemSyncItemsAsync(ids);

        var toAddList = boardSyncItems
                        .Where(board => existingWorkItemSyncItems.All(existing => board.SyncInfo.SyncId != existing.SyncInfo.SyncId))
                        .ToList();

        var remainedList = boardSyncItems.Except(toAddList).ToList();

        var toUpdateList = remainedList
                           .Where(board =>
                               existingWorkItemSyncItems.Any(existing => board.SyncInfo.SyncId == existing.SyncInfo.SyncId && board.SyncInfo.ContentHash != existing.SyncInfo.ContentHash))
                           .ToList();

        var sameList = remainedList.Except(toUpdateList).ToList();

        var toAddOrUpdate = tasks
            .Where(t => toAddList.Any(s => s.Id == t.Id) || toUpdateList.Any(s => s.Id == t.Id))
            .ToList();

        await CrystaTaskRepository.AddOrUpdateCrystaTasksAsync(toAddOrUpdate);

        return new SyncResult
        {
            AddCount = toAddList.Count,
            UpdateCount = toUpdateList.Count,
            SameCount = sameList.Count
        };
    }

    private CrystaTask ToCrystaTask(WorkItem workItem, AzureBoardSyncConfig config)
    {
        var json = JsonSerializer.Serialize(workItem);
        var hash = json.Sha();

        var syncInfo = new SyncInfo
        {
            SyncId = $"{config.Organization}/{config.Project}/{workItem.Id.ToString()}",
            ContentHash = hash,
            LastSyncDateTime = DateTimeOffset.Now,
            LastSyncOffset = workItem.Id.ToString(),
            SyncGroup = "SyncService"
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

        var areaPath = workItem.Fields["System.AreaPath"]?.ToString();
        var iterationPath = workItem.Fields["System.IterationPath"]?.ToString();
        DateTimeOffset.TryParse(workItem.Fields["System.CreatedDate"]?.ToString(), out var createdDate);

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

        var assignedToText = workItem.Fields.GetValueOrDefault("System.AssignedTo") switch
        {
            IdentityRef identityRef => $"{identityRef.DisplayName} ({identityRef.UniqueName})",
            _ => ""
        };

        var createdBy = workItem.Fields.GetValueOrDefault("System.CreatedBy") switch
        {
            IdentityRef identityRef => $"{identityRef.DisplayName} ({identityRef.UniqueName})",
            _ => ""
        };

        var task = new CrystaTask
        {
            ProviderTaskId = workItem.Id?.ToString(),
            Title = title,
            Description = description,
            DescriptionHtml = description,
            Status = status,
            TaskCreateDateTime = taskCreateDateTime,
            ProviderTaskUrl = workItem.Url,
            WorkItemSyncInfo = syncInfo,
            AssignedToText = assignedToText,
            Revision = workItem.Rev?.ToString() ?? string.Empty,
            RawJson = json,
            WorkItemType = workItem.Fields["System.WorkItemType"]?.ToString(),
            AreaPath = areaPath,
            IterationPath = iterationPath,
            CreatedDate = createdDate,
            CreatedByText = createdBy,
            Tags = workItem.Fields.GetValueOrDefault("System.Tags")?.ToString()
        };


        return task;
    }

    private CrystaTaskComment? ToCrystaTaskComment(WorkItemComment comment, Guid crystaTaskId, int workItemId)
    {
        if (comment == null)
        {
            return null;
        }

        var json = JsonSerializer.Serialize(comment);
        var hash = json.Sha();

        // WorkItemComment properties: Text, RenderedText
        return new CrystaTaskComment
        {
            CrystaTaskId = crystaTaskId,
            ProviderTaskId = workItemId.ToString(),
            Text = comment.Text,
            FormattedText = comment.RenderedText,
            Revision = "0", // Will be populated from context if available
            CreatedDate = DateTimeOffset.Now,
            RawJson = json,
            SyncInfo = new SyncInfo
            {
                SyncId = $"{workItemId}-comment-{hash.Substring(0, 8)}",
                ContentHash = hash,
                LastSyncDateTime = DateTimeOffset.Now,
                SyncStatus = SyncStatus.Success
            }
        };
    }

    private CrystaTaskUpdate? ToCrystaTaskUpdate(WorkItemUpdate update, Guid crystaTaskId, int workItemId)
    {
        if (update == null || update.Fields == null)
        {
            return null;
        }

        var json = JsonSerializer.Serialize(update);
        var hash = json.Sha();

        // Get the first field change for simplicity - in production you might create multiple updates
        var firstFieldChange = update.Fields.FirstOrDefault();

        return new CrystaTaskUpdate
        {
            CrystaTaskId = crystaTaskId,
            ProviderTaskId = workItemId.ToString(),
            ProviderUpdateId = update.Id.ToString(),
            Revision = update.Rev.ToString(),
            ChangedBy = update.RevisedBy?.DisplayName,
            ChangedById = update.RevisedBy?.Id.ToString(),
            ChangedDate = update.RevisedDate,
            FieldName = firstFieldChange.Key,
            OldValue = firstFieldChange.Value?.OldValue?.ToString(),
            NewValue = firstFieldChange.Value?.NewValue?.ToString(),
            RawJson = json,
            SyncInfo = new SyncInfo
            {
                SyncId = $"{workItemId}-update-{update.Id}",
                ContentHash = hash,
                LastSyncDateTime = DateTimeOffset.Now,
                SyncStatus = SyncStatus.Success
            }
        };
    }

    private CrystaTaskRevision? ToCrystaTaskRevision(WorkItem revision, Guid crystaTaskId, int workItemId)
    {
        if (revision == null)
        {
            return null;
        }

        var json = JsonSerializer.Serialize(revision);
        var hash = json.Sha();

        string revisionNumber = revision.Rev?.ToString() ?? "0";
        var title = revision.Fields?.GetValueOrDefault("System.Title")?.ToString();
        var description = revision.Fields?.GetValueOrDefault("System.Description")?.ToString();
        var state = revision.Fields?.GetValueOrDefault("System.State")?.ToString();
        var changedBy = revision.Fields?.GetValueOrDefault("System.ChangedBy") as IdentityRef;

        DateTimeOffset changedDate = DateTimeOffset.Now;
        if (revision.Fields?.GetValueOrDefault("System.ChangedDate") is string changedDateStr)
        {
            DateTimeOffset.TryParse(changedDateStr, out changedDate);
        }

        DateTimeOffset createdDate = DateTimeOffset.Now;
        if (revision.Fields?.GetValueOrDefault("System.CreatedDate") is string createdDateStr)
        {
            DateTimeOffset.TryParse(createdDateStr, out createdDate);
        }

        return new CrystaTaskRevision
        {
            CrystaTaskId = crystaTaskId,
            ProviderTaskId = workItemId.ToString(),
            Revision = revisionNumber,
            RawJson = json,
            Title = title,
            Description = description,
            State = state,
            ChangedBy = changedBy?.DisplayName,
            ChangedDate = changedDate,
            CreatedDate = createdDate,
            ProjectId = Guid.Empty, // Will be set from context if needed
            ProjectName = revision.Fields?.GetValueOrDefault("System.TeamProject")?.ToString()
        };
    }
}
