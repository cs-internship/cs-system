using CrystaLearn.Core.Extensions;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.AzureBoard;
using CrystaLearn.Core.Services.Contracts;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CrystaLearn.Core.Services.Sync;

public partial class AzureBoardSyncService : IAzureBoardSyncService
{
    [AutoInject] private IAzureBoardService AzureBoardService { get; set; } = default!;
    [AutoInject] private IConfiguration Configuration { get; set; } = default!;
    [AutoInject] private ICrystaTaskRepository CrystaTaskRepository { get; set; } = default!;
    [AutoInject] private ICrystaProgramSyncModuleService CrystaProgramSyncModuleRepository { get; set; } = default!;

    public async Task<SyncResult> SyncAsync(CrystaProgramSyncModule module, List<int>? workItemIds = null)
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

        string query;

        if (workItemIds != null && workItemIds.Count > 0)
        {
            query = $"Select [Id] From WorkItems Where [System.Id] In ({string.Join(",", workItemIds)})";
        }
        else
        {
            query =
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
        }
        var totalResult = new SyncResult { AddCount = 0, UpdateCount = 0, SameCount = 0, DeleteCount = 0 };

        // Collect all active work item IDs from Azure Board
        var activeWorkItemIds = new HashSet<string>();


        await foreach (var workItems in AzureBoardService.EnumerateWorkItemsQueryAsync(config, query))
        {
            Console.WriteLine($"Processing batch of {workItems.Count} work items...");
            
            module.SyncInfo.SyncStartDateTime = DateTimeOffset.Now;

            var tasks = workItems
                        .Select(w => ToCrystaTask(w, config))
                        .ToList();

            // Collect active work item IDs
            foreach (var task in tasks)
            {
                if (!string.IsNullOrEmpty(task.WorkItemSyncInfo.SyncId))
                {
                    activeWorkItemIds.Add(task.WorkItemSyncInfo.SyncId);
                }
            }

            var workItemResult = await SyncWorkItemsAsync(config, tasks);
            Console.WriteLine($"  WorkItems - Added: {workItemResult.AddCount}, Updated: {workItemResult.UpdateCount}, Unchanged: {workItemResult.SameCount}");
            //var updatesResult = await SyncUpdatesAsync(config, tasks);
            //Console.WriteLine($"  Updates   - Added: {updatesResult.AddCount}, Updated: {updatesResult.UpdateCount}, Unchanged: {updatesResult.SameCount}");
            var commentsResult = await SyncCommentsAsync(config, tasks);
            Console.WriteLine($"  Comments  - Added: {commentsResult.AddCount}, Updated: {commentsResult.UpdateCount}, Unchanged: {commentsResult.SameCount}");
            var revisionsResult = await SyncRevisionsAsync(config, tasks);
            Console.WriteLine($"  Revisions - Added: {revisionsResult.AddCount}, Updated: {revisionsResult.UpdateCount}, Unchanged: {revisionsResult.SameCount}");

            // Persist only tasks that got new SyncInfo values

            var tasksToUpdate = tasks
                .Where(t => t.UpdatesSyncInfo != null || t.CommentsSyncInfo != null || t.RevisionsSyncInfo != null)
                .ToList();

            if (tasksToUpdate.Count > 0)
                await CrystaTaskRepository.UpdateCrystaTasksAsync(tasksToUpdate);


            // Aggregate results
            totalResult.AddCount += workItemResult.AddCount  + commentsResult.AddCount + revisionsResult.AddCount;
            totalResult.UpdateCount += workItemResult.UpdateCount + commentsResult.UpdateCount + revisionsResult.UpdateCount;
            totalResult.SameCount += workItemResult.SameCount + commentsResult.SameCount + revisionsResult.SameCount;

            // Update module sync info for this batch using the largest 'Changed Date' among loaded work items
            DateTimeOffset? batchMaxChanged = null;
            foreach (var wi in workItems)
            {
                if (wi.Fields != null && wi.Fields.TryGetValue("System.ChangedDate", out var changedObj))
                {
                    var changedStr = changedObj?.ToString();
                    if (!string.IsNullOrEmpty(changedStr) && DateTimeOffset.TryParse(changedStr, out var changedDt))
                    {
                        if (!batchMaxChanged.HasValue || changedDt > batchMaxChanged.Value)
                            batchMaxChanged = changedDt;
                    }
                }
            }

            if (batchMaxChanged.HasValue)
            {
                module.SyncInfo.LastSyncDateTime = batchMaxChanged.Value;
                module.UpdatedAt = DateTimeOffset.Now;
                // Also update LastSyncOffset to the max work item id in this batch so subsequent queries can continue from there
                var maxId = workItems.Max(w => w.Id ?? 0);
                module.SyncInfo.LastSyncOffset = maxId.ToString();

                module.SyncInfo.SyncEndDateTime = DateTimeOffset.Now;
                config.WorkItemChangedFromDateTime = batchMaxChanged.Value;

                module.SyncConfig = JsonSerializer.Serialize<AzureBoardSyncConfig>(config);

                // Persist module after updating sync info for this batch
                try
                {
                    await CrystaProgramSyncModuleRepository.UpdateSyncModuleAsync(module);
                }
                catch
                {
                    throw;
                }
            }
        }

        //TODO: Handle deletions - find tasks in our DB that are no longer in Azure Board
       

        // Update module sync info
        
        module.SyncInfo.SyncStatus = SyncStatus.Success;

        // Persist final module sync info

        await CrystaProgramSyncModuleRepository.UpdateSyncModuleAsync(module);

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

        // Preload mapping of WorkItem SyncId -> CrystaTask.Id for existing tasks (used when task.Id == Guid.Empty)
        var taskSyncIds = tasks.Select(t => t.WorkItemSyncInfo.SyncId ?? "").Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
        var workItemSyncItems = taskSyncIds.Count > 0 ? await CrystaTaskRepository.GetWorkItemSyncItemsAsync(taskSyncIds) : new List<SyncItem>();
        var syncIdToGuidMap = workItemSyncItems.Where(s => s.SyncInfo != null && s.Id.HasValue)
            .ToDictionary(s => s.SyncInfo.SyncId ?? string.Empty, s => s.Id.Value, StringComparer.OrdinalIgnoreCase);

        foreach (var task in tasks)
        {
            if (string.IsNullOrEmpty(task.ProviderTaskId) || !int.TryParse(task.ProviderTaskId, out var workItemId))
            {
                continue;
            }

            var azureUpdates = await AzureBoardService.GetUpdatesAsync(config, workItemId);

            // collect per-task updates
            var taskUpdates = new List<CrystaTaskUpdate>();

            foreach (var update in azureUpdates)
            {
                // Resolve CrystaTaskId: prefer task.Id, otherwise try to lookup by SyncId
                var crystaTaskId = task.Id != Guid.Empty
                    ? task.Id
                    : (syncIdToGuidMap.TryGetValue(task.WorkItemSyncInfo?.SyncId ?? string.Empty, out var gid) ? gid : Guid.Empty);

                var crystaUpdates = ToCrystaTaskUpdate(update, crystaTaskId, workItemId, config);
                if (crystaUpdates != null && crystaUpdates.Count > 0)
                {
                    taskUpdates.AddRange(crystaUpdates);
                    allUpdates.AddRange(crystaUpdates);
                }
            }

            // compute and assign UpdatesSyncInfo for this task using only its own updates
            if (taskUpdates.Count > 0)
            {
                try
                {
                    var combined = string.Join("", taskUpdates.Select(u => u.RawJson ?? string.Empty));
                    var contentHash = combined.Sha();

                    int maxUpdateId = 0;
                    foreach (var u in taskUpdates)
                    {
                        if (int.TryParse(u.ProviderUpdateId, out var uid))
                        {
                            if (uid > maxUpdateId) maxUpdateId = uid;
                        }
                    }

                    task.UpdatesSyncInfo = new SyncInfo
                    {
                        SyncId = $"{config.Organization}/{config.Project}/{task.ProviderTaskId}/updates",
                        ContentHash = contentHash,
                        LastSyncDateTime = DateTimeOffset.Now,
                        LastSyncOffset = maxUpdateId > 0 ? maxUpdateId.ToString() : null,
                        SyncStatus = SyncStatus.Success,
                        SyncGroup = "SyncService"
                    };
                }
                catch
                {
                    // swallow per-task sync info errors
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
            .Where(u => toAddList.Any(s => s.SyncInfo.SyncId == u.SyncInfo.SyncId) || toUpdateList.Any(s => s.SyncInfo.SyncId == u.SyncInfo.SyncId))
            .ToList();

        // split to add and update
        var toAdd = toAddOrUpdate.Where(u => toAddList.Any(s => s.SyncInfo.SyncId == u.SyncInfo.SyncId)).ToList();
        var toUpdate = toAddOrUpdate.Where(u => toUpdateList.Any(s => s.SyncInfo.SyncId == u.SyncInfo.SyncId)).ToList();

        if (toAdd.Count > 0)
            await CrystaTaskRepository.AddCrystaTaskUpdatesAsync(toAdd);

        if (toUpdate.Count > 0)
            await CrystaTaskRepository.UpdateCrystaTaskUpdatesAsync(toUpdate);

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

        // Preload mapping of WorkItem SyncId -> CrystaTask.Id for existing tasks
        var taskSyncIds = tasks.Select(t => t.WorkItemSyncInfo.SyncId ?? "").Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
        var workItemSyncItems = taskSyncIds.Count > 0 ? await CrystaTaskRepository.GetWorkItemSyncItemsAsync(taskSyncIds) : new List<SyncItem>();
        var syncIdToGuidMap = workItemSyncItems.Where(s => s.SyncInfo != null && s.Id.HasValue)
            .ToDictionary(s => s.SyncInfo.SyncId ?? string.Empty, s => s.Id.Value, StringComparer.OrdinalIgnoreCase);

        foreach (var task in tasks)
        {
            if (string.IsNullOrEmpty(task.ProviderTaskId) || !int.TryParse(task.ProviderTaskId, out var workItemId))
            {
                continue;
            }

            var azureComments = await AzureBoardService.GetCommentsAsync(config, workItemId);

            var taskComments = new List<CrystaTaskComment>();

            foreach (var comment in azureComments)
            {
                // Resolve CrystaTaskId: prefer task.Id, otherwise try to lookup by SyncId
                var crystaTaskId = task.Id != Guid.Empty
                    ? task.Id
                    : (syncIdToGuidMap.TryGetValue(task.WorkItemSyncInfo?.SyncId ?? string.Empty, out var gid) ? gid : Guid.Empty);

                var crystaComment = ToCrystaTaskComment(comment, crystaTaskId, workItemId, config);
                if (crystaComment != null)
                {
                    taskComments.Add(crystaComment);
                    allComments.Add(crystaComment);
                }
            }

            if (taskComments.Count > 0)
            {
                try
                {
                    var combined = string.Join("", taskComments.Select(c => c.RawJson ?? string.Empty));
                    var contentHash = combined.Sha();

                    int maxRevision = 0;
                    foreach (var c in taskComments)
                    {
                        if (int.TryParse(c.Revision, out var rv))
                        {
                            if (rv > maxRevision) maxRevision = rv;
                        }
                    }

                    task.CommentsSyncInfo = new SyncInfo
                    {
                        SyncId = $"{config.Organization}/{config.Project}/{task.ProviderTaskId}",
                        ContentHash = contentHash,
                        LastSyncDateTime = taskComments.Max(f => f.SyncInfo.LastSyncDateTime),
                        LastSyncOffset = maxRevision > 0 ? maxRevision.ToString() : null,
                        SyncStatus = SyncStatus.Success,
                        SyncGroup = "SyncService"
                    };
                }
                catch
                {
                    // swallow per-task
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
            .Where(c => toAddList.Any(s => s.SyncInfo.SyncId == c.SyncInfo.SyncId) || toUpdateList.Any(s => s.SyncInfo.SyncId == c.SyncInfo.SyncId))
            .ToList();

        var toAdd = toAddOrUpdate.Where(c => toAddList.Any(s => s.SyncInfo.SyncId == c.SyncInfo.SyncId)).ToList();
        var toUpdate = toAddOrUpdate.Where(c => toUpdateList.Any(s => s.SyncInfo.SyncId == c.SyncInfo.SyncId)).ToList();

        if (toAdd.Count > 0)
            await CrystaTaskRepository.AddCrystaTaskCommentsAsync(toAdd);

        if (toUpdate.Count > 0)
            await CrystaTaskRepository.UpdateCrystaTaskCommentsAsync(toUpdate);

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

        // Preload mapping of WorkItem SyncId -> CrystaTask.Id for existing tasks
        var taskSyncIds = tasks.Select(t => t.WorkItemSyncInfo.SyncId ?? "").Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
        var workItemSyncItems = taskSyncIds.Count > 0 ? await CrystaTaskRepository.GetWorkItemSyncItemsAsync(taskSyncIds) : new List<SyncItem>();
        var syncIdToGuidMap = workItemSyncItems.Where(s => s.SyncInfo != null && s.Id.HasValue)
            .ToDictionary(s => s.SyncInfo.SyncId ?? string.Empty, s => s.Id.Value, StringComparer.OrdinalIgnoreCase);

        foreach (var task in tasks)
        {
            if (string.IsNullOrEmpty(task.ProviderTaskId) || !int.TryParse(task.ProviderTaskId, out var workItemId))
            {
                continue;
            }

            var azureRevisions = await AzureBoardService.GetRevisionsAsync(config, workItemId);

            var taskRevisions = new List<CrystaTaskRevision>();

            foreach (var revision in azureRevisions)
            {
                // Resolve CrystaTaskId: prefer task.Id, otherwise try to lookup by SyncId
                var crystaTaskId = task.Id != Guid.Empty
                    ? task.Id
                    : (syncIdToGuidMap.TryGetValue(task.WorkItemSyncInfo?.SyncId ?? string.Empty, out var gid) ? gid : Guid.Empty);

                var crystaRevision = ToCrystaTaskRevision(revision, crystaTaskId, workItemId, config);
                if (crystaRevision != null)
                {
                    taskRevisions.Add(crystaRevision);
                    allRevisions.Add(crystaRevision);
                }
            }

            if (taskRevisions.Count > 0)
            {
                try
                {
                    var combined = string.Join("", taskRevisions.Select(r => r.RawJson ?? string.Empty));
                    var contentHash = combined.Sha();

                    int maxRevision = 0;
                    foreach (var r in taskRevisions)
                    {
                        if (int.TryParse(r.Revision, out var rv))
                        {
                            if (rv > maxRevision) maxRevision = rv;
                        }
                    }

                    task.RevisionsSyncInfo = new SyncInfo
                    {
                        SyncId = $"{config.Organization}/{config.Project}/{task.ProviderTaskId}/revisions",
                        ContentHash = contentHash,
                        LastSyncDateTime = DateTimeOffset.Now,
                        LastSyncOffset = maxRevision > 0 ? maxRevision.ToString() : null,
                        SyncStatus = SyncStatus.Success,
                        SyncGroup = "SyncService"
                    };
                }
                catch
                {
                    // swallow per-task
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
                    SyncId = r.WorkItemSyncInfo.SyncId,
                    ContentHash = r.WorkItemSyncInfo.ContentHash
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
            .Where(r => toAddList.Any(s => s.SyncInfo.SyncId == r.WorkItemSyncInfo.SyncId) || toUpdateList.Any(s => s.SyncInfo.SyncId == r.WorkItemSyncInfo.SyncId))
            .ToList();

        var toAdd = toAddOrUpdate.Where(r => toAddList.Any(s => s.SyncInfo.SyncId == r.WorkItemSyncInfo.SyncId)).ToList();
        var toUpdate = toAddOrUpdate.Where(r => toUpdateList.Any(s => s.SyncInfo.SyncId == r.WorkItemSyncInfo.SyncId)).ToList();

        if (toAdd.Count > 0)
            await CrystaTaskRepository.AddCrystaTaskRevisionsAsync(toAdd);

        if (toUpdate.Count > 0)
            await CrystaTaskRepository.UpdateCrystaTaskRevisionsAsync(toUpdate);

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
    private async Task<SyncResult> SyncWorkItemsAsync(AzureBoardSyncConfig config, List<CrystaTask> tasks)
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
            .Where(t => toAddList.Any(s => s.SyncInfo.SyncId == t.WorkItemSyncInfo.SyncId) || toUpdateList.Any(s => s.SyncInfo.SyncId == t.WorkItemSyncInfo.SyncId))
            .ToList();

        var toAdd = toAddOrUpdate.Where(t => toAddList.Any(s => s.SyncInfo.SyncId == t.WorkItemSyncInfo.SyncId)).ToList();
        var toUpdate = toAddOrUpdate.Where(t => toUpdateList.Any(s => s.SyncInfo.SyncId == t.WorkItemSyncInfo.SyncId)).ToList();

        // Ensure new tasks have an Id so parent references can be assigned before persisting
        foreach (var t in toAdd)
        {
            if (t.Id == Guid.Empty)
                t.Id = Guid.NewGuid();
        }

        // Build a map of SyncId -> CrystaTask.Id that includes existing DB items and the current batch (toAdd)
        var syncIdToGuid = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

        foreach (var existing in existingWorkItemSyncItems)
        {
            var sid = existing.SyncInfo?.SyncId;
            if (!string.IsNullOrEmpty(sid) && existing.Id.HasValue)
            {
                syncIdToGuid[sid] = existing.Id.Value;
            }
        }

        foreach (var item in toAdd)
        {
            var sid = item.WorkItemSyncInfo?.SyncId;
            if (!string.IsNullOrEmpty(sid))
            {
                // Prefer existing DB id if present; otherwise use the in-memory id (for toAdd)
                if (!syncIdToGuid.ContainsKey(sid))
                    syncIdToGuid[sid] = item.Id;
            }
        }

        // Assign ParentId for tasks that have a ProviderParentId
        // First, collect missing parent sync ids that are not present in the current map
        var missingParentSyncIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var t in toAddOrUpdate)
        {
            if (string.IsNullOrEmpty(t.ProviderParentId))
                continue;

            var parentSyncId = $"{config.Organization}/{config.Project}/{t.ProviderParentId}";
            if (!syncIdToGuid.ContainsKey(parentSyncId))
            {
                missingParentSyncIds.Add(parentSyncId);
            }
        }

        if (missingParentSyncIds.Count > 0)
        {
            // Try to load missing parents from DB in one call
            var parentSyncItems = await CrystaTaskRepository.GetWorkItemSyncItemsAsync(missingParentSyncIds.ToList());
            foreach (var p in parentSyncItems)
            {
                var sid = p.SyncInfo?.SyncId;
                if (!string.IsNullOrEmpty(sid) && p.Id.HasValue && !syncIdToGuid.ContainsKey(sid))
                {
                    syncIdToGuid[sid] = p.Id.Value;
                }
            }
        }

        // Now assign ParentId using the updated map
        foreach (var t in toAddOrUpdate)
        {
            if (string.IsNullOrEmpty(t.ProviderParentId))
                continue;

            var parentSyncId = $"{config.Organization}/{config.Project}/{t.ProviderParentId}";
            if (syncIdToGuid.TryGetValue(parentSyncId, out var parentGuid))
            {
                t.ParentId = parentGuid;
            }
        }

        if (toAdd.Count > 0)
            await CrystaTaskRepository.AddCrystaTasksAsync(toAdd);

        if (toUpdate.Count > 0)
            await CrystaTaskRepository.UpdateCrystaTasksAsync(toUpdate);

        // Build combined list: existing + newly added (toAddList)
        //var combined = new List<SyncItem>(existingWorkItemSyncItems.Count + toAddList.Count);
        //combined.AddRange(existingWorkItemSyncItems);
        //combined.AddRange(toAdd.Select(a => new SyncItem() { Id = a.Id, SyncInfo = a.WorkItemSyncInfo }));

        var result = new SyncResult
        {
            AddCount = toAddList.Count,
            UpdateCount = toUpdateList.Count,
            SameCount = sameList.Count
        };

        return result;
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

        var providerStatus = workItem.Fields["System.State"]?.ToString();
        var status = providerStatus switch
        {
            "New" or "To Do" or "Design"=> CrystaTaskStatus.New,
            "Approved" or "In Progress" or "Committed" or "Ready" => CrystaTaskStatus.InProgress,
            "Done" or "Closed" => CrystaTaskStatus.Done,
            "Canceled" or "Removed" => CrystaTaskStatus.Canceled,
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

        var taskChangeDateTime = workItem.Fields["System.ChangedDate"]?.ToString() switch
        {
            string s => DateTimeOffset.Parse(s),
            _ => throw new Exception("Invalid date")
        };

        var areaPath = workItem.Fields["System.AreaPath"]?.ToString();
        var iterationPath = workItem.Fields["System.IterationPath"]?.ToString();
        
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

        object? remainingWorkObj = workItem.Fields.GetValueOrDefault("Microsoft.VSTS.Scheduling.RemainingWork");
        double? remainingWork = null;
        if (remainingWorkObj is double d)
        {
            remainingWork = d;
        }
        else if (remainingWorkObj is int i)
        {
            remainingWork = Convert.ToDouble(i);
        }
        else if (remainingWorkObj is string s && double.TryParse(s, out var result))
        {
            remainingWork = result;
        }

        var changedBy = workItem.Fields.GetValueOrDefault("System.ChangedBy") switch
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
            ProviderStatus = providerStatus,
            TaskCreateDateTime = taskCreateDateTime,
            TaskChangedDateTime = taskChangeDateTime,
            ProviderTaskUrl = $"https://dev.azure.com/{config.Organization}/{config.Project}/_workitems/edit/{workItem.Id}",
            WorkItemSyncInfo = syncInfo,
            AssignedToText = assignedToText,
            Revision = workItem.Rev?.ToString() ?? string.Empty,
            RawJson = json,
            WorkItemType = workItem.Fields["System.WorkItemType"]?.ToString(),
            AreaPath = areaPath,
            IterationPath = iterationPath,
            CreatedByText = createdBy,
            Tags = workItem.Fields.GetValueOrDefault("System.Tags")?.ToString(),
            ProjectName = workItem.Fields.GetValueOrDefault("System.TeamProject")?.ToString(),
            RemainingWork = remainingWork,
            ProviderParentId = workItem.Fields.GetValueOrDefault("System.Parent")?.ToString(),
            ChangedBy = changedBy,
            BoardColumn = workItem.Fields.GetValueOrDefault("System.BoardColumn")?.ToString(),
            BoardColumnDone = workItem.Fields.GetValueOrDefault("System.BoardColumnDone") as bool? ?? false,
            Reason = workItem.Fields.GetValueOrDefault("System.Reason")?.ToString(),
            Priority = workItem.Fields.GetValueOrDefault("Microsoft.VSTS.Common.Priority") switch
            {
                int p => p,
                string ps when int.TryParse(ps, out var pi) => pi,
                _ => null
            },
            CommentCount = workItem.Fields.GetValueOrDefault("System.CommentCount") switch
            {
                int c => c,
                string cs when int.TryParse(cs, out var ci) => ci,
                _ => null
            },
            AttachmentsCount = workItem.Fields.GetValueOrDefault("System.AttachedFileCount") switch
            {
                int c => c,
                string cs when int.TryParse(cs, out var ci) => ci,
                _ => null
            },
            IsDeleted = false
        };

        return task;
    }

    private CrystaTaskComment? ToCrystaTaskComment(WorkItemComment comment, Guid crystaTaskId, int workItemId, AzureBoardSyncConfig config)
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
            Revision = comment.Revision.ToString(), // Will be populated from context if available
            CreatedDate = DateTimeOffset.Now,
            RawJson = json,
            SyncInfo = new SyncInfo
            {
                SyncId = $"{config.Organization}/{config.Project}/{workItemId.ToString()}/comment/{hash.Substring(0, 8)}",
                ContentHash = hash,
                LastSyncDateTime = DateTimeOffset.Now,
                SyncStatus = SyncStatus.Success
            },
            ContentHtml = comment.RenderedText
        };
    }

    private List<CrystaTaskUpdate> ToCrystaTaskUpdate(WorkItemUpdate update, Guid crystaTaskId, int workItemId, AzureBoardSyncConfig config)
    {
        var result = new List<CrystaTaskUpdate>();

        if (update == null || update.Fields == null || update.Fields.Count == 0)
        {
            return result;
        }

        var json = JsonSerializer.Serialize(update);
        var baseHash = json.Sha();

        string FormatValue(object? v)
        {
            if (v == null) return null!;
            if (v is IdentityRef idRef)
            {
                return $"{idRef.DisplayName} ({idRef.UniqueName})";
            }
            if (v is string s) return s;
            if (v is JsonElement je)
            {
                try
                {
                    if (je.ValueKind == JsonValueKind.String) return je.GetString() ?? string.Empty;
                    return je.ToString() ?? string.Empty;
                }
                catch
                {
                    return je.ToString() ?? string.Empty;
                }
            }
            return v.ToString() ?? string.Empty;
        }

        foreach (var fieldChange in update.Fields)
        {
            var fieldName = fieldChange.Key;
            var oldVal = fieldChange.Value?.OldValue is object ov ? FormatValue(ov) : null;
            var newVal = fieldChange.Value?.NewValue is object nv ? FormatValue(nv) : null;

            var updateObj = new CrystaTaskUpdate
            {
                CrystaTaskId = crystaTaskId,
                ProviderTaskId = workItemId.ToString(),
                ProviderUpdateId = update.Id.ToString(),
                Revision = update.Rev.ToString(),
                ChangedBy = update.RevisedBy?.DisplayName,
                ChangedById = update.RevisedBy?.Id.ToString(),
                ChangedDate = update.RevisedDate,
                FieldName = fieldName,
                OldValue = oldVal,
                NewValue = newVal,
                RawJson = json,
                SyncInfo = new SyncInfo
                {
                    SyncId = $"{config.Organization}/{config.Project}/{workItemId}/{update.Id}",
                    ContentHash = baseHash,
                    LastSyncDateTime = DateTimeOffset.Now,
                    SyncStatus = SyncStatus.Success,
                    SyncGroup = "SyncService"
                }
            };

            result.Add(updateObj);
        }

        return result;
    }

    private CrystaTaskRevision? ToCrystaTaskRevision(WorkItem revision, Guid crystaTaskId, int workItemId, AzureBoardSyncConfig config)
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

        var areaPath = revision.Fields?.GetValueOrDefault("System.AreaPath")?.ToString();
        var iterationPath = revision.Fields?.GetValueOrDefault("System.IterationPath")?.ToString();

        var createdBy = revision.Fields?.GetValueOrDefault("System.CreatedBy") switch
        {
            IdentityRef identityRef => $"{identityRef.DisplayName} ({identityRef.UniqueName})",
            _ => ""
        };

        CrystaTaskStatus? status = state switch
        {
            "New" or "To Do" => CrystaTaskStatus.New,
            "Approved" or "In Progress" or "Committed" => CrystaTaskStatus.InProgress,
            "Done" or "Closed" => CrystaTaskStatus.Done,
            "Canceled" or "Removed" => CrystaTaskStatus.Canceled,
            _ => null
        };

        var syncInfo = new SyncInfo
        {
            SyncId = $"{config.Organization}/{config.Project}/{revision.Id.ToString()}/{revisionNumber}",
            ContentHash = hash,
            LastSyncDateTime = DateTimeOffset.Now,
            LastSyncOffset = revision.Id.ToString(),
            SyncGroup = "SyncService"
        };

        return new CrystaTaskRevision
        {
            CrystaTaskId = crystaTaskId,
            ProviderTaskId = workItemId.ToString(),
            Revision = revisionNumber,

            // CrystaTask properties (mapped similar to ToCrystaTask)
            Title = title,
            Description = description,
            DescriptionHtml = description,
            Status = status,
            TaskCreateDateTime = createdDate,
            ProviderTaskUrl = revision.Url,
            AssignedToText = revision.Fields?.GetValueOrDefault("System.AssignedTo") switch
            {
                IdentityRef identityRef => $"{identityRef.DisplayName} ({identityRef.UniqueName})",
                _ => ""
            },
            RawJson = json,
            WorkItemType = revision.Fields?.GetValueOrDefault("System.WorkItemType")?.ToString(),
            AreaPath = areaPath,
            IterationPath = iterationPath,
            CreatedDate = createdDate,
            CreatedByText = createdBy,
            Tags = revision.Fields?.GetValueOrDefault("System.Tags")?.ToString(),
            ProjectName = revision.Fields?.GetValueOrDefault("System.TeamProject")?.ToString(),

            // Revision-specific
            ChangedBy = changedBy?.DisplayName,
            ChangedDate = changedDate,
            WorkItemSyncInfo = syncInfo

        };
    }
}
